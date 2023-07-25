#include "JvsEmu.h"

#include <format>
#include <Windows.h>
#include <string>
#include <queue>
#include <sstream>

#include "log.h"
#include "MinHook.h"

using namespace std::string_literals;

#define JVS_TRUE				0x01
#define JVS_FALSE				0x00
#define JVS_SYNC_CODE			0xE0
#define JVS_REPORT_CODE			0x01
#define JVS_COMMAND_REV			0x13
#define JVS_BOARD_REV			0x30
#define JVS_COMM_REV			0x10
#define JVS_ADDR_MASTER			0x00
#define JVS_STATUS_OK			0x01
#define JVS_STATUS_UNKNOWN_CMD	0x02
#define JVS_STATUS_SUM_ERROR	0x03
#define JVS_STATUS_ACK_OVERFLOW	0x04
#define JVS_REPORT_OK			0x01
#define JVS_REPORT_INVAL_PARAM1	0x02
#define JVS_REPORT_INVAL_PARAM2	0x03
#define JVS_REPORT_BUSY			0x04
#define JVS_STREAM_SIZE			1024

#define JVS_OP_RESET			0xF0
#define JVS_OP_ADDRESS			0xF1
#define JVS_OP_CHECKSUM_ERROR	0x2F

#define JVS_IOFUNC_SWINPUT		0x01
#define JVS_IOFUNC_COINTYPE		0x02
#define JVS_IOFUNC_EXITCODE		0x00

#define JVS_SUPPORT_PLAYERS		2
#define JVS_SUPPORT_SLOTS		2

#define LOG_COMMANDS	1
#if LOG_COMMANDS
#define logcmd	logmsg
#else
#define logcmd(str, __VA_LIST__)
#endif

std::queue<BYTE> replyBuffer;
typedef signed char SINT8;
typedef signed short SINT16;
typedef signed short SINT32;
typedef unsigned char UINT8;
typedef unsigned short UINT16;
static HANDLE hConnection = (HANDLE)0x1337;

static const char *Rfid_IO_Id = "DE-JV PCB";

struct jvs_command_def {
	UINT8 params;
	UINT8 reports;
};

#define __ARG__(n)	((DWORD) pfunc[n])

class jprot_encoder {
	BYTE buffer[1024];
	BYTE *ptr;
	DWORD statusaddr;
	DWORD sizeaddr;
	DWORD sumaddr;
	DWORD startaddr;
	DWORD xpos;
	DWORD size_;
	DWORD nreplys;
public:
	DWORD size() {
		return size_;
	}
	jprot_encoder() {
		memset(buffer, 0, 1024);
		xpos = 0;
	}
	~jprot_encoder() {
		memset(buffer, 0, 1024);
		xpos = 0;
	}

	void addreply() {
		nreplys++;
	}


	void set_status(BYTE v) {
		buffer[statusaddr] = v;
	}
	void clear()
	{
		memset(buffer, 0, 1024);
		size_ = 0;
		xpos = 0;
	}
	void begin_stream() {

		pushz(JVS_SYNC_CODE);
		sumaddr = xpos;
		pushz(JVS_ADDR_MASTER);
		sizeaddr = xpos;
		push(0);
		statusaddr = xpos;
		// STATUS
		push(JVS_STATUS_OK);

	}

	void pushz(BYTE v) {
		buffer[xpos++] = v;
	}

	void push(BYTE v) {
#if 0
		buffer[xpos] = v;
		++xpos;
#else
		if (v == 0xD0) {
			buffer[xpos++] = 0xD0;
			buffer[xpos++] = 0xCF;
		}
		else
			if (v == 0xE0) {
				buffer[xpos++] = 0xD0;
				buffer[xpos++] = 0xDF;
			}
			else
				buffer[xpos++] = v;
#endif
	}
	void report(BYTE v) {
		push(v);
	}
	void end_stream() {
		if (xpos == (statusaddr + 1)) {
			clear();
			return;
		}

		DWORD sizeK = 0;
		for (DWORD i = sizeaddr; i<xpos; i++)
			if (buffer[i] != 0xD0)
				++sizeK;
		// encode the size of the stream
		buffer[sizeaddr] = sizeK;

		// calculate the checksum
		DWORD sum = 0;
		for (DWORD i = sumaddr, inc = 0; i<xpos; i++) {
			if (buffer[i] == 0xD0) {
				inc = 1;
			}
			else {
				sum += (DWORD)((buffer[i] + inc) & 0xFF);
				if (inc)
					inc = 0;
			}
		}
		sum &= 0xFF;
		push(sum);
		size_ = xpos;
	}


	void read(BYTE *dst, DWORD size) {
		if (size > size_)
			size = size_;
		memcpy(dst, &buffer[0], size);
	}

	void printReply()
	{
		static char printer[1024];
		memset(printer, 0, 1024);
		if (size()) {
#ifdef PrintRFIDReplies
			sprintf(printer, "R:");
			for (DWORD i = 0; i<size(); i++)
			{
				sprintf(printer + 2 + (i * 3), "%02X ", buffer[i]);
			}
			info(true, printer);
#endif
		}
	}

	void printSource(BYTE* srcbuffer, int strsize)
	{
		std::stringstream ss;
		ss << "Source: ";
		for (int i = 0; i < strsize; ++i)
		{
			ss << std::format("{:02X} ", srcbuffer[i]);
		}
		log(ss.str().c_str());
	}
};

static int isAddressed = 0;
int is_addressed() {
	return isAddressed;
}
void reset_addressed()
{
	isAddressed = 0;
}

static WORD p1coin = 0;
static WORD p2coin = 0;
static int coinstate[2] = { 0, 0 };

int handleBusReset()
{
	p1coin = 0;
	p2coin = 0;
	return 2;
}

// 0xF1 -- set address
int handleSetAddress(jprot_encoder *r)
{
	r->report(JVS_REPORT_OK);
	isAddressed = 1;

	return 2;
}

static bool cardInserted = false;

// 0x26 -- read general-purpose input
int handleReadGeneralPurposeInput(jprot_encoder *r, DWORD arg1)
{
	r->report(JVS_REPORT_OK);
	for(DWORD i = 0; i < arg1; i++)
	{
		if ( cardInserted)
		{
			r->push(0x19); // This should be only injected with first package of the 3, but does not seem to care.
		}
		else
		{
			r->push(0);
		}
	}
	return 2 + arg1;
}


// 0x32 -- read general-purpose output. This is very confusing 0x32 0x01 0x00 returns 0x01 (0x18 times 0x00) 0x01
// See JVSP manual for more information.
int handleReadGeneralPurposeOutput(jprot_encoder *r, DWORD arg1)
{
#ifdef _DEBUG
	//OutputDebugStringA("Requested card data!");
#endif
	
	r->report(JVS_REPORT_OK);
		for (DWORD i = 0; i < arg1 * 0x18; i++)
		{
			r->push(0);
		}
	r->report(JVS_REPORT_OK);
	return 2 + arg1;
}

int handleReTransmitDataInCaseOfChecksumFailure()
{
	return 1;
}

int handleReadIDData(jprot_encoder *r)
{
	const char *str = NULL;
	r->report(JVS_REPORT_OK);
	//if(gameType == X2Type::RFID)
	str = Rfid_IO_Id;
	//else if(gameType == X2Type::Digital)
	//	str = IO_Id;
	while (*str) { r->push(*str++); }
	r->push(0);
	return 1;
}

int handleGetCommandFormatVersion(jprot_encoder *r)
{
	r->report(JVS_REPORT_OK);
	r->push(JVS_COMMAND_REV);
	return 1;
}

int handleGetJVSVersion(jprot_encoder *r)
{
	r->report(JVS_REPORT_OK);
	r->push(JVS_BOARD_REV);
	return 1;
}

int handleGetCommunicationVersion(jprot_encoder *r)
{
	r->report(JVS_REPORT_OK);
	r->push(JVS_COMM_REV);
	return 1;
}

int handleGetSlaveFeatures(jprot_encoder *r)
{
	r->report(JVS_REPORT_OK);

	// Switch
	r->push(1);
	r->push(1);
	r->push(13);
	r->push(0);

	// Coin
	r->push(2);
	r->push(1);
	r->push(0);
	r->push(0);

	// Analog
	r->push(3);
	r->push(8);
	r->push(16);
	r->push(0);

	/*r->push(0x12);
	r->push(0x08);
	r->push(0);
	r->push(0);*/

	r->push(0);
	
	return 1;
}

int handleTaito01Call(jprot_encoder *r, DWORD arg1)
{
	r->report(JVS_REPORT_OK); 
	r->push(1);
	return 2;
}

int handleTaito02Call(jprot_encoder *r)
{
	r->report(JVS_REPORT_OK);
	r->push(0x52);
	return 2;
}

int handleTaito03Call(jprot_encoder *r)
{
	r->report(JVS_REPORT_OK);
	r->push(1);
	return 2;
}

int handleTaito04Call(jprot_encoder *r)
{
	r->report(JVS_REPORT_OK);
	return 1;
}

int handleTaito05Call(jprot_encoder *r)
{
	r->report(JVS_REPORT_OK);
	return 3;
}

jvs_key_bind key_bind;
int handleReadSwitchInputs(jprot_encoder *r)
{
	BYTE byte0 = 0;
	BYTE byte1 = 0;
	BYTE byte2 = 0;

	JOYINFOEX joy;
	joy.dwSize = sizeof(joy);
	joy.dwFlags = JOY_RETURNALL;
	for (UINT joystickIndex = 0; joystickIndex < 16; ++joystickIndex)
	{
		if(joyGetPosEx(joystickIndex, &joy) == JOYERR_NOERROR)
		{
			if(joy.dwPOV == 0)
			{
				log("Up Detected from Joystick");
				byte1 |= static_cast<char>(1 << 5);
			}
			if(joy.dwPOV == 4500)
			{
				log("Up Right Detected from Joystick");
				byte1 |= static_cast<char>(1 << 5);
				byte1 |= static_cast<char>(1 << 2);
			}
			if(joy.dwPOV == 9000)
			{
				log("Right Detected from Joystick");
				byte1 |= static_cast<char>(1 << 2);
			}
			if(joy.dwPOV == 13500)
			{
				log("Right Down Detected from Joystick");
				byte1 |= static_cast<char>(1 << 2);
				byte1 |= static_cast<char>(1 << 4);
			}
			if(joy.dwPOV == 18000)
			{
				log("Down Detected from Joystick");
				byte1 |= static_cast<char>(1 << 4);
			}
			if(joy.dwPOV == 22500)
			{
				log("Down Left Detected from Joystick");
				byte1 |= static_cast<char>(1 << 4);
				byte1 |= static_cast<char>(1 << 3);
			}
			if(joy.dwPOV == 27000)
			{
				log("Left Detected from Joystick");
				byte1 |= static_cast<char>(1 << 3);
			}
			if(joy.dwPOV == 31500)
			{
				log("Top Left Detected from Joystick");
				byte1 |= static_cast<char>(1 << 3);
				byte1 |= static_cast<char>(1 << 5);
			}
			int intJoyDwButtons = (int) joy.dwButtons;
			if (intJoyDwButtons & key_bind.ArcadeButton1)
			{
				log("Button 1 Detected from Joystick");
				byte1 |= static_cast<char> (1 << 1);
			}
			if (intJoyDwButtons & key_bind.ArcadeButton2)
			{
				log("Button 2 Detected from Joystick");
				byte1 |= static_cast<char> (1);
			}
			if (intJoyDwButtons & key_bind.ArcadeButton3)
			{
				log("Button 3 Detected from Joystick");
				byte2 |= static_cast<char> (1 << 7);
			}
			if (intJoyDwButtons & key_bind.ArcadeButton4)
			{
				log("Button 4 Detected from Joystick");
				byte2 |= static_cast<char> (1 << 6);
			}
			if (intJoyDwButtons & key_bind.ArcadeStartButton)
			{
				log("Start Button Detected from Joystick");
				byte1 |= static_cast<char>(1 << 7);
			}
		}
	}

	if (GetAsyncKeyState(key_bind.Test) & 0x8000)
	{
		log("Test Pressed");
		byte0 |= static_cast<char>(1 << 7);
	}

	if (GetAsyncKeyState(key_bind.Start) & 0x8000)
	{
		log("Start Pressed");
		byte1 |= static_cast<char>(1 << 7);
	}

	if (GetAsyncKeyState(key_bind.Service) & 0x8000)
	{
		log("Service Pressed");
		byte1 |= static_cast<char>(1 << 6);
	}

	if (GetAsyncKeyState(key_bind.Up) & 0x8000)
	{
		log("Up Pressed");
		byte1 |= static_cast<char>(1 << 5);
	}

	if (GetAsyncKeyState(key_bind.Left) & 0x8000)
	{
		log("Left Pressed");
		byte1 |= static_cast<char>(1 << 3);
	}

	if (GetAsyncKeyState(key_bind.Down) & 0x8000)
	{
		log("Down Pressed");
		byte1 |= static_cast<char>(1 << 4);
	}

	if (GetAsyncKeyState(key_bind.Right) & 0x8000)
	{
		log("Right Pressed");
		byte1 |= static_cast<char>(1 << 2);
	}

	if (GetAsyncKeyState(key_bind.Button1) & 0x8000)
	{
		log("Button 1 Pressed");
		byte1 |= static_cast<char> (1 << 1);
	}

	if (GetAsyncKeyState(key_bind.Button2) & 0x8000)
	{
		log("Button 2 Pressed");
		byte1 |= static_cast<char> (1);
	}

	if (GetAsyncKeyState(key_bind.Button3) & 0x8000)
	{
		log("Button 3 Pressed");
		byte2 |= static_cast<char> (1 << 7);
	}

	if (GetAsyncKeyState(key_bind.Button4) & 0x8000)
	{
		log("Button 4 Pressed");
		byte2 |= static_cast<char> (1 << 6);
	}

	r->report(JVS_REPORT_OK);
	r->push(byte0);
	r->push(byte1);
	r->push(byte2);
	return 3;
}

int handleReadCoinInputs(jprot_encoder *r)
{
	int currstate = 0;// inputMgr.GetState(P1_COIN);
	if (!coinstate[0] && (currstate)) {
		++p1coin;
	}
	coinstate[0] = currstate;

	currstate = 0;// inputMgr.GetState(P2_COIN);
	if (!coinstate[1] && (currstate)) {
		++p2coin;

	}
	coinstate[1] = currstate;

	//logcmd("Lendo o ficheiro... \n");
	r->report(JVS_REPORT_OK);
	r->push(p1coin >> 8);
	r->push(p1coin & 0xFF);
	/*r->push(p2coin >> 8);
	r->push(p2coin & 0xFF);*/
	return 2;
}

int handleReadAnalogInputs(jprot_encoder *r, int channelCount)
{
	r->report(JVS_REPORT_OK);
	for (int i = 0; i < channelCount; ++i)
	{
		r->push(0);
		r->push(0);
	}
	return 2;
}

int handleDecreaseNumberOfCoins(jprot_encoder *r, DWORD arg1, DWORD arg2, DWORD arg3)
{
	WORD val = ((arg2 & 0xFF) << 8) | (arg3 & 0xFF);
	r->report(JVS_REPORT_OK);
#ifdef _DEBUG
	//logcmd("-coin %d, %d\n", arg1, val);
#endif
	switch (arg1)
	{
	case 1:
		if (val > p1coin)
			p1coin = 0;
		else
			p1coin -= val;
		break;
	case 2:
		if (val > p2coin)
			p2coin = 0;
		else
			p2coin -= val;
	}
	return 4;
}

int 	handlePayouts(jprot_encoder *r, DWORD arg1, DWORD arg2, DWORD arg3)
{
	WORD val = ((arg2 & 0xFF) << 8) | (arg3 & 0xFF);
	r->report(JVS_REPORT_OK);
#ifdef _DEBUG
	//logcmd("+coin %d, %d\n", arg1, val);
#endif
	switch (arg1)
	{
	case 1: p1coin += val; break;
	case 2: p2coin += val; break;
	}
	return 4;
}

unsigned long process_stream(unsigned char *stream, unsigned long srcsize, unsigned char *dst, unsigned long dstsize)
{
	jprot_encoder r;
	BYTE *pstr = stream;
	BYTE *pfunc = NULL;
	BYTE node = 0;
	BYTE pktsize = 0;
	int pktcount = 0;
	WORD pktaddr = 0;

	r.clear();

	// Ignore weird packages
	if (pstr[1] != 0x00 && pstr[1] != 0x01 && pstr[1] != 0xFF)
	{
#ifdef _DEBUG
		OutputDebugStringA("Invalid package received!");
#endif
		return 0;
	}

	if (pstr[0] != JVS_SYNC_CODE) {
#ifdef _DEBUG
		log("Invalid Sync code!\n");
#endif
	}
#ifdef _DEBUG
	r.printSource(stream, srcsize);
#endif
	node = pstr[1];
	pktsize = pstr[2];
	pfunc = &pstr[3];

	pktcount = (int)pktsize - 1;

	r.begin_stream();

	while (pktcount > 0)
	{
		int increment = 0;
		log("Type is %02X", pfunc[0] & 0xFF);
		switch (pfunc[0] & 0xFF)
		{
		case 0xF0:
			increment = handleBusReset();
			break;
		case 0xF1:
			increment = handleSetAddress(&r);
			break;
		case 0x01:
			increment = handleTaito01Call(&r, __ARG__(1));
			break;
		case 0x02:
			increment = handleTaito02Call(&r);
			break;
		case 0x03:
			increment = handleTaito03Call(&r);
			break;
		case 0x04:
			increment = handleTaito04Call(&r);
			break;
		case 0x05:
			increment = handleTaito05Call(&r);
			break;
		case 0x10:
			increment = handleReadIDData(&r);
			break;
		case 0x11:
			increment = handleGetCommandFormatVersion(&r);
			break;
		case 0x12:
			increment = handleGetJVSVersion(&r);
			break;
		case 0x13:
			increment = handleGetCommunicationVersion(&r);
			break;
		case 0x14:
			increment = handleGetSlaveFeatures(&r);
			break;
		case 0x20:
			increment = handleReadSwitchInputs(&r);
			break;
		case 0x21:
			increment = handleReadCoinInputs(&r);
			break;
		case 0x22:
			increment = handleReadAnalogInputs(&r, __ARG__(1));
			break;
		case 0x26:
			increment = handleReadGeneralPurposeInput(&r, __ARG__(1));
			break;
		case 0x2F:
			increment = handleReTransmitDataInCaseOfChecksumFailure();
			break;
		case 0x30:
			increment = handleDecreaseNumberOfCoins(&r, __ARG__(1), __ARG__(2), __ARG__(3));
			break;
		case 0x31:
			increment = handlePayouts(&r, __ARG__(1), __ARG__(2), __ARG__(3));
			break;
		case 0x32:
			increment = handleReadGeneralPurposeOutput(&r, __ARG__(1));
			break;
		default:
#ifdef _DEBUG
			log("Unknown command %X\n", __ARG__(0));
#endif
			r.report(JVS_REPORT_OK);
			increment = 1;
			break;
		}

		pktcount -= increment;
		pfunc += increment;
	}

	r.set_status(JVS_STATUS_OK);
	r.end_stream();
	r.read(dst, dstsize);
#ifdef _DEBUG
	r.printReply();
#endif
	return r.size();
}

BOOL(__stdcall *g_origGetCommModemStatus)(HANDLE hFile, LPDWORD lpModemStat);
BOOL __stdcall GetCommModemStatusWrap(HANDLE hFile, LPDWORD lpModemStat)
{
	if (hFile != hConnection) {
		return g_origGetCommModemStatus(hFile, lpModemStat);
	}
	if (is_addressed())
		*lpModemStat = 0x10;
	else
		*lpModemStat = 0x10;
#ifdef _DEBUG
	log("GetCommModemStatus(hFile=%d, lpModemStat=%p) -> result=%08X", hFile, lpModemStat, TRUE);
#endif
	return TRUE;
}


BOOL(__stdcall *g_origGetCommState)(HANDLE hFile, LPDCB lpDCB);
BOOL __stdcall GetCommStateWrap(HANDLE hFile, LPDCB lpDCB)
{
	if (hFile != hConnection) {
		return g_origGetCommState(hFile, lpDCB);
	}
#ifdef _DEBUG
	log("GetCommState(hFile=%d, lpDCB=%p) -> result=%08X", hFile, lpDCB, 1);
#endif
	return TRUE;
}

BOOL(__stdcall *g_origSetCommState)(HANDLE hFile, LPDCB lpDCB);
BOOL __stdcall SetCommStateWrap(HANDLE hFile, LPDCB lpDCB)
{
	if (hFile != hConnection) {
		return g_origSetCommState(hFile, lpDCB);
	}
#ifdef _DEBUG
	log("SetCommState(hFile=%d, lpDCB=%p) -> result=%08X", hFile, lpDCB, 1);
#endif
	return TRUE;
}

BOOL(__stdcall *g_origSetCommTimeouts)(HANDLE hFile, LPCOMMTIMEOUTS lpCommTimeouts);
BOOL __stdcall SetCommTimeoutsWrap(HANDLE hFile, LPCOMMTIMEOUTS lpCommTimeouts)
{

	if (hFile != hConnection) {
		return g_origSetCommTimeouts(hFile, lpCommTimeouts);
	}
#ifdef _DEBUG
	log("SetCommTimeouts(hFile=%d, lpCommTimeouts=%p) -> result=%08X", hFile, lpCommTimeouts, TRUE);
#endif
	return TRUE;
}

BOOL (*gOrigPurgeComm)(HANDLE hFile, DWORD  dwFlags);
BOOL PurgeCommWrap(
  HANDLE hFile,
  DWORD  dwFlags
)
{
	if (hFile != hConnection)
	{
		return gOrigPurgeComm(hFile, dwFlags);
	}
	log("PurgeComm");			
	return TRUE;
}

BOOL(__stdcall *g_origWriteFile)(HANDLE hFile,
	LPVOID lpBuffer,
	DWORD nNumberOfBytesToWrite,
	LPDWORD lpNumberOfBytesWritten,
	LPOVERLAPPED lpOverlapped);
BOOL __stdcall WriteFileWrap(HANDLE hFile,
	LPVOID lpBuffer,
	DWORD nNumberOfBytesToWrite,
	LPDWORD lpNumberOfBytesWritten,
	LPOVERLAPPED lpOverlapped)
{
	if (hFile != hConnection) {
		return g_origWriteFile(hFile, lpBuffer, nNumberOfBytesToWrite, lpNumberOfBytesWritten, lpOverlapped);
	}
	static BYTE rbuffer[1024];
	static BYTE logger[1024];

	DWORD sz = process_stream((LPBYTE)lpBuffer, nNumberOfBytesToWrite, rbuffer, 1024);
	if (sz != 1) {
		for (DWORD i = 0; i < sz; i++)
		{
			replyBuffer.push(rbuffer[i]);
		}
	}

#ifdef LogRFID
	info(true, "");
	info(true, "--------------------------------------------");
#endif
	return TRUE;
}

BOOL(__stdcall *g_origReadFile)(HANDLE hFile,
	LPVOID lpBuffer,
	DWORD nNumberOfBytesToRead,
	LPDWORD lpNumberOfBytesRead,
	LPOVERLAPPED lpOverlapped);
BOOL __stdcall  ReadFileWrap(HANDLE hFile,
	LPVOID lpBuffer,
	DWORD nNumberOfBytesToRead,
	LPDWORD lpNumberOfBytesRead,
	LPOVERLAPPED lpOverlapped)
{
	if (hFile != hConnection) {
		return g_origReadFile(hFile, lpBuffer, nNumberOfBytesToRead, lpNumberOfBytesRead, lpOverlapped);
	}

#ifdef LogRFID
	info(true, "ReadFile(hFile=%d, lpBuffer='%08X', nNumberOfBytesToRead=%08X) -> result=%08X", hFile, lpBuffer, nNumberOfBytesToRead, TRUE);
#endif
	if (replyBuffer.size())
	{
		if (nNumberOfBytesToRead >= replyBuffer.size())
			nNumberOfBytesToRead = replyBuffer.size();
		*lpNumberOfBytesRead = nNumberOfBytesToRead;
		BYTE *ptr = (BYTE*)lpBuffer;
		for (DWORD i = 0; i < nNumberOfBytesToRead; i++) {
			if (!replyBuffer.empty()) {
				*ptr++ = replyBuffer.front();
				replyBuffer.pop();
			}
			else
				*ptr++ = 0;
		}
	}
	else {
		*lpNumberOfBytesRead = 0;
		return TRUE;
	}
#ifdef LogRFID
	info(true, "");
	info(true, "--------------------------------------------");
#endif
	return TRUE;
}

BOOL(__stdcall *g_origCloseHandle)(HANDLE hObject);
BOOL __stdcall CloseHandleWrap(HANDLE hObject)
{
	if (hObject != hConnection) {
		return g_origCloseHandle(hObject);
	}
#ifdef LogRFID
	info(true, "CloseHandle(hObject=%d) -> result=%08X", hObject, TRUE);
	info(true, "--------------------------------------------");
#endif
	return TRUE;
}

/*HANDLE(__stdcall *g_origCreateFileA)(LPCSTR lpFileName,
	DWORD dwDesiredAccess,
	DWORD dwShareMode,
	LPSECURITY_ATTRIBUTES lpSecurityAttributes,
	DWORD dwCreationDisposition,
	DWORD dwFlagsAndAttributes,
	HANDLE hTemplateFile);
HANDLE __stdcall CreateFileAWrap(LPCSTR lpFileName,
	DWORD dwDesiredAccess,
	DWORD dwShareMode,
	LPSECURITY_ATTRIBUTES lpSecurityAttributes,
	DWORD dwCreationDisposition,
	DWORD dwFlagsAndAttributes,
	HANDLE hTemplateFile)
{
	if (!strcmp(lpFileName, "COM2"))
	{
#ifdef LogRFID
		info(true, "CreateFile(lpFileName=%s) -> result=%08X", lpFileName, hConnection);
		info(true, "--------------------------------------------");
#endif
		return hConnection;
	}
	
	return g_origCreateFileA(lpFileName,
		dwDesiredAccess,
		dwShareMode,
		lpSecurityAttributes,
		dwCreationDisposition,
		dwFlagsAndAttributes,
		hTemplateFile);
}

HANDLE(__stdcall *g_origCreateFileW)(LPCWSTR lpFileName,
	DWORD dwDesiredAccess,
	DWORD dwShareMode,
	LPSECURITY_ATTRIBUTES lpSecurityAttributes,
	DWORD dwCreationDisposition,
	DWORD dwFlagsAndAttributes,
	HANDLE hTemplateFile);
HANDLE __stdcall CreateFileWWrap(LPCWSTR lpFileName,
	DWORD dwDesiredAccess,
	DWORD dwShareMode,
	LPSECURITY_ATTRIBUTES lpSecurityAttributes,
	DWORD dwCreationDisposition,
	DWORD dwFlagsAndAttributes,
	HANDLE hTemplateFile)
{
	if (!wcscmp(lpFileName, L"COM2"))
	{
#ifdef LogRFID
		info(true, "CreateFile(lpFileName=%s) -> result=%08X", lpFileName, hConnection);
		info(true, "--------------------------------------------");
#endif
		return hConnection;
	}

	return g_origCreateFileW(lpFileName,
		dwDesiredAccess,
		dwShareMode,
		lpSecurityAttributes,
		dwCreationDisposition,
		dwFlagsAndAttributes,
		hTemplateFile);
}*/

void InitializeJvs(jvs_key_bind keyBind)
{
	key_bind = keyBind;

	MH_Initialize();
	MH_CreateHookApi(L"kernel32.dll", "WriteFile", WriteFileWrap, reinterpret_cast<void**>(&g_origWriteFile));
	MH_CreateHookApi(L"kernel32.dll", "ReadFile", ReadFileWrap, reinterpret_cast<void**>(&g_origReadFile));
	MH_CreateHookApi(L"kernel32.dll", "CloseHandle", CloseHandleWrap, reinterpret_cast<void**>(&g_origCloseHandle));
	MH_CreateHookApi(L"kernel32.dll", "GetCommModemStatus", GetCommModemStatusWrap, reinterpret_cast<void**>(&g_origGetCommModemStatus));
	MH_CreateHookApi(L"kernel32.dll", "GetCommState", GetCommStateWrap, reinterpret_cast<void**>(&g_origGetCommState));
	MH_CreateHookApi(L"kernel32.dll", "SetCommState", SetCommStateWrap, reinterpret_cast<void**>(&g_origSetCommState));
	MH_CreateHookApi(L"kernel32.dll", "SetCommTimeouts", SetCommTimeoutsWrap, reinterpret_cast<void**>(&g_origSetCommTimeouts));

	MH_EnableHook(nullptr);
}
