#Region ;**** Directives created by AutoIt3Wrapper_GUI ****
#AutoIt3Wrapper_Icon=
#AutoIt3Wrapper_Outfile=D:\POC Launcher\x86\XBoost Launcher.exe
#AutoIt3Wrapper_Outfile_x64=D:\POC Launcher\x64\XBoost Launcher.exe
#AutoIt3Wrapper_Compile_Both=y
#AutoIt3Wrapper_UseX64=y
#EndRegion ;**** Directives created by AutoIt3Wrapper_GUI ****
#include <ButtonConstants.au3>
#include <EditConstants.au3>
#include <GUIConstantsEx.au3>
#include <ColorConstants.au3>
#include <StaticConstants.au3>
#include <TabConstants.au3>
#include <WindowsConstants.au3>
#include <File.au3>


#Region ## Global Variables Section Starts##
Global $Filename = "config.ini" ; Variable for Config file
Global $filedir = @ScriptDir & "\" & $Filename ; Variable for file directory to store path of config.ini
Global $ipconfigout = @ScriptDir & "\TOOLS\ipconfig-output.txt" ; Variable for the ipconfig output file path from batch file
Global $iFileExists = FileExists($filedir) ; Varuiable used to check if config.ini file is in the game directory
Global $currentIPEnabled = IniRead($filedir, "Config", "IpAddress", "default")
Global $currentIPDisabled = IniRead($filedir, "Config", "# IpAddress", "default"); Variable that reads current value of IpAddress in Config.ini
;~ Global $currentGateway = IniRead($filedir, "Config", "Gateway", "default") ; Variable that reads current value of Gateway in Config.ini
Global $currentServer = IniRead($filedir, "Config", "Server", "default") ; Variable that reads current value of Server in Config.ini
Global $currentInterfaceNameEnabled = IniRead($filedir, "Config", "InterfaceName", "default")
Global $currentInterfaceNameDisabled = IniRead($filedir, "Config", "# InterfaceName", "default") ; Variable that reads current value of Commented InterfaceName
Global $defaultIP = "192.168.0.100" ; Variable for Default Value of IpAddress in Config.ini
;~ Global $defaultGateway = "192.168.0.1" ; Variable for Default Value of Gateway in Config.ini
Global $defaultServer = "127.0.0.1" ; Variable for Default Value of Server in Config.ini
Global $defaultInterfaceName = "Ethernet" ; Variable for Default Value of InterfaceName in Config.ini
Global $currentB1 = IniRead($filedir, "dinput", "A", "default") ; Variable that reads current value of ArcadeButton1 in Config.ini for DirectInput
Global $currentB2 = IniRead($filedir, "dinput", "B", "default") ; Variable that reads current value of ArcadeButton2 in Config.ini for DirectInput
Global $currentB3 = IniRead($filedir, "dinput", "C", "default") ; Variable that reads current value of ArcadeButton3 in Config.ini for DirectInput
Global $currentB4 = IniRead($filedir, "dinput", "D", "default") ; Variable that reads current value of ArcadeButton4 in Config.ini for DirectInput
Global $currentBS = IniRead($filedir, "dinput", "Start", "default") ; Variable that reads current value of ArcadeStartButton in Config.ini for DirectInput
Global $currentBC = IniRead($filedir, "dinput", "Coin", "default") ; Variable that reads current value of ArcadeCoin in Config.ini for DirectInput
Global $currentBT = IniRead($filedir, "dinput", "Test", "default") ; Variable that reads current value of ArcadeTest in Config.ini for DirectInput
Global $currentBCard = IniRead($filedir, "dinput", "Card", "default") ; Variable that reads current value of ArcadeCard in Config.ini for DirectInput
Global $currentDeviceId = IniRead($filedir, "dinput", "DeviceId", "default")
Global $currentKeyB1 = IniRead($filedir, "keyboard", "A", "default") ; Variable that reads current value of Button1 in Config.ini for Keyboard
Global $currentKeyB2 = IniRead($filedir, "keyboard", "B", "default") ; Variable that reads current value of Button2 in Config.ini for Keyboard
Global $currentKeyB3 = IniRead($filedir, "keyboard", "C", "default") ; Variable that reads current value of Button3 in Config.ini for Keyboard
Global $currentKeyB4 = IniRead($filedir, "keyboard", "D", "default") ; Variable that reads current value of Button4 in Config.ini for Keyboard
Global $currentKeyBS = IniRead($filedir, "keyboard", "Start", "default") ; Variable that reads current value of Start in Config.ini for Keyboard
Global $currentKeyBC = IniRead($filedir, "keyboard", "Coin", "default") ; Variable that reads current value of Coin in Config.ini for Keyboard
Global $currentKeyBT = IniRead($filedir, "keyboard", "Test", "default") ; Variable that reads current value of Test in Config.ini for Keyboard
Global $currentKeyCard = IniRead($filedir, "keyboard", "Card", "defautl") ; Variable that reads current value of Card in the Config.ini for Keyboard
Global $currentKeyUp = IniRead($filedir, "keyboard", "Up", "default") ; Variable that reads current value of Up in Config.ini for Keyboard
Global $currentKeyDown = IniRead($filedir, "keyboard", "Down", "default") ; Variable that reads current value of Down in Config.ini for Keyboard
Global $currentKeyLeft = IniRead($filedir, "keyboard", "Left", "default") ; Variable that reads current value of Left in Config.ini for Keyboard
Global $currentKeyRight = IniRead($filedir, "keyboard", "Right", "default") ; Variable that reads current value of Right in Config.ini for Keyboard
Global $currentKill = IniRead($filedir, "keyboard", "Kill", "default")
Global $defaultB1 = "1" ; Variable for Default Value of ArcadeButton1 in Config.ini for DirectInput
Global $defaultB2 = "4" ; Variable for Default Value of ArcadeButton2 in Config.ini for DirectInput
Global $defaultB3 = "6" ; Variable for Default Value of ArcadeButton3 in Config.ini for DirectInput
Global $defaultB4 = "2" ; Variable for Default Value of ArcadeButton4 in Config.ini for DirectInput
Global $defaultBS = "10" ; Variable for Default Value of ArcadeStartButton in Config.ini for DirectInput
Global $defaultBC = "13" ; Variable for Default Value of ArcadeCoin in Config.ini for DirectInput
Global $defaultBT = "9" ; Variable for Default Value of ArcadeTest in Config.ini for DirectInput
Global $defaultBCard = "12" ; Variable for Default Value of ArcadeCard in Config.ini for DirectInput
Global $defaultKeyUp = "UpArr,W" ; Variable for Default Value of Up in Config.ini for Keyboard
Global $defaultKeyDown = "DownArr,S" ; Variable for efault Value of Down in Config.ini for Keyboard
Global $defaultKeyLeft = "LeftArr,A" ; Variable for Default Value of Left in Config.ini for Keyboard
Global $defaultKeyRight = "RightArr,D" ; Variable for Default Value of Right in Config.ini for Keyboard
Global $defaultKeyB1 = "Z,7 (NumPad)" ; Variable for Default Value of Button1 in Config.ini for Keyboard
Global $defaultKeyB2 = "X,8 (NumPad)" ; Variable for Default Value of Button2 in Config.ini for Keyboard
Global $defaultKeyB3 = "C,9 (NumPad)" ; Variable for Default Value of Button3 in Config.ini for Keyboard
Global $defaultKeyB4 = "V,0 (NumPad)" ; Variable for Default Value of Button4 in Config.ini for Keyboard
Global $defaultKeyBS = "1" ; Variable for Default Value of Start in Config.ini for Keyboard
Global $defaultKeyBC = "5" ; Variable for Default Value of Coin in Config.ini for Keyboard
Global $defaultKeyBT = "F1" ; Variable for Default Value of Test in Config.ini for Keyboard
Global $defaultKeyCard = "F3" ; Variable for Default Value of Card in Config.ini for Keyboard
Global $defaultKeyKill = "Esc"
#EndRegion ## Global Variables Section End ##

;~ #Region ### START Koda GUI section ### Form=
;~ Global $XB_Launcher = GUICreate("XBoost Launcher", 812, 664, -1, -1)
;~ Global $bStartLM = GUICtrlCreateButton("Start LM Mode", 592, 144, 131, 33)
;~ GUICtrlSetFont(-1, 10, 800, 0, "MS Sans Serif")
;~ Global $bStartClient = GUICtrlCreateButton("Start Client Mode", 592, 200, 131, 33)
;~ GUICtrlSetFont(-1, 10, 800, 0, "MS Sans Serif")
;~ Global $bStartServer = GUICtrlCreateButton("Start Server.exe", 592, 312, 131, 33)
;~ GUICtrlSetFont(-1, 10, 800, 0, "MS Sans Serif")
;~ Global $bOpenConfig = GUICtrlCreateButton("Open Config.ini", 592, 256, 131, 33)
;~ GUICtrlSetFont(-1, 10, 800, 0, "MS Sans Serif")
;~ Global $bCardWebpage = GUICtrlCreateButton("Card Webpage", 592, 370, 131, 33)
;~ GUICtrlSetFont(-1, 10, 800, 0, "MS Sans Serif")
;~ Global $bExit = GUICtrlCreateButton("Exit", 592, 440, 131, 33)
;~ GUICtrlSetFont(-1, 10, 800, 0, "MS Sans Serif")
;~ Global $tConfig = GUICtrlCreateTab(8, 24, 569, 625)
;~ GUICtrlSetFont(-1, 12, 800, 0, "MS Sans Serif")
;~ Global $TabSheet1 = GUICtrlCreateTabItem("Config")
;~ Global $IP = GUICtrlCreateLabel("IP Address", 60, 78, 82, 24)
;~ GUICtrlSetFont(-1, 12, 400, 0, "MS Sans Serif")
;~ Global $Gateway = GUICtrlCreateLabel("Default Gateway", 60, 118, 123, 24)
;~ GUICtrlSetFont(-1, 12, 400, 0, "MS Sans Serif")
;~ Global $TenpoRouter = GUICtrlCreateLabel("TenpoRouter", 60, 158, 98, 24)
;~ GUICtrlSetFont(-1, 12, 400, 0, "MS Sans Serif")
;~ Global $Server = GUICtrlCreateLabel("Server.exe Address", 60, 198, 142, 24)
;~ GUICtrlSetFont(-1, 12, 400, 0, "MS Sans Serif")
;~ Global $Windowed = GUICtrlCreateLabel("Windowed Mode", 60, 254, 122, 24)
;~ GUICtrlSetFont(-1, 12, 400, 0, "MS Sans Serif")
;~ Global $iIPAddress = GUICtrlCreateInput($currentIP, 252, 78, 193, 28)
;~ GUICtrlSetFont(-1, 12, 400, 0, "MS Sans Serif")
;~ Global $iGateway = GUICtrlCreateInput($currentGateway, 252, 118, 193, 28)
;~ GUICtrlSetFont(-1, 12, 400, 0, "MS Sans Serif")
;~ Global $iTenpoR = GUICtrlCreateInput($currentGateway, 252, 158, 193, 28)
;~ GUICtrlSetFont(-1, 12, 400, 0, "MS Sans Serif")
;~ Global $iServer = GUICtrlCreateInput($currentServer, 252, 198, 193, 28)
;~ GUICtrlSetFont(-1, 12, 400, 0, "MS Sans Serif")
;~ Global $WindowCheck = GUICtrlCreateCheckbox("", 263, 254, 20, 20)
;~ GUICtrlSetFont(-1, 100, 400, 0, "MS Sans Serif")
;~ Global $bIPconfig = GUICtrlCreateButton("IPCONFIG", 204, 311, 83, 25)
;~ GUICtrlSetFont(-1, 10, 400, 0, "MS Sans Serif")
;~ Global $bIPHowTo = GUICtrlCreateButton("How To Setup", 448, 400, 121, 33)
;~ GUICtrlSetFont(-1, 12, 400, 0, "MS Sans Serif")
;~ Global $bSave = GUICtrlCreateButton("Save", 128, 368, 75, 25)
;~ GUICtrlSetFont(-1, 10, 400, 0, "MS Sans Serif")
;~ Global $bDefault = GUICtrlCreateButton("Default", 296, 368, 75, 25)
;~ GUICtrlSetFont(-1, 10, 400, 0, "MS Sans Serif")
;~ Global $TabSheet2 = GUICtrlCreateTabItem("Controller Settings")
;~ Global $lDirectInput = GUICtrlCreateLabel("DirectInput", 20, 62, 99, 24)
;~ GUICtrlSetFont(-1, 12, 800, 0, "MS Sans Serif")
;~ Global $DIRadio = GUICtrlCreateRadio("", 140, 62, 17, 17)
;~ Global $lXinput = GUICtrlCreateLabel("XInput", 212, 62, 75, 24)
;~ GUICtrlSetFont(-1, 12, 800, 0, "MS Sans Serif")
;~ Global $XInputRadio = GUICtrlCreateRadio("", 300, 62, 25, 17)
;~ Global $bDinputHowTo = GUICtrlCreateButton("How To Setup", 448, 464, 121, 33)
;~ GUICtrlSetFont(-1, 12, 400, 0, "MS Sans Serif")
;~ Global $bJoystickRelease = GUICtrlCreateButton("Joystick Detection Tool", 392, 56, 163, 33)
;~ GUICtrlSetFont(-1, 10, 400, 0, "MS Sans Serif")
;~ Global $lArcadeB1 = GUICtrlCreateLabel("ArcadeButton1 (Shoot)", 28, 120, 169, 24)
;~ GUICtrlSetFont(-1, 12, 400, 0, "MS Sans Serif")
;~ Global $lArcadeB3 = GUICtrlCreateLabel("ArcadeButton3 (Boost)", 28, 200, 168, 24)
;~ GUICtrlSetFont(-1, 12, 400, 0, "MS Sans Serif")
;~ Global $lArcadeB2 = GUICtrlCreateLabel("ArcadeButton2 (Melee)", 28, 160, 169, 24)
;~ GUICtrlSetFont(-1, 12, 400, 0, "MS Sans Serif")
;~ Global $lArcadeB4 = GUICtrlCreateLabel("ArcadeButton4 (Change Target)", 28, 240, 232, 24)
;~ GUICtrlSetFont(-1, 12, 400, 0, "MS Sans Serif")
;~ Global $lArcadeStart = GUICtrlCreateLabel("ArcadeButton Start", 28, 280, 142, 24)
;~ GUICtrlSetFont(-1, 12, 400, 0, "MS Sans Serif")
;~ Global $lArcadeCoin = GUICtrlCreateLabel("ArcadeButton Coin", 28, 320, 139, 24)
;~ GUICtrlSetFont(-1, 12, 400, 0, "MS Sans Serif")
;~ Global $lArcadeTest = GUICtrlCreateLabel("ArcadeButton Test Menu", 28, 360, 182, 24)
;~ GUICtrlSetFont(-1, 12, 400, 0, "MS Sans Serif")
;~ Global $lArcadeCard = GUICtrlCreateLabel("ArcadeButton Card", 28, 400, 141, 24)
;~ GUICtrlSetFont(-1, 12, 400, 0, "MS Sans Serif")
;~ Global $AB1 = GUICtrlCreateInput($currentB1, 316, 118, 121, 28)
;~ GUICtrlSetFont(-1, 12, 400, 0, "MS Sans Serif")
;~ Global $AB2 = GUICtrlCreateInput($currentB2, 316, 158, 121, 28)
;~ GUICtrlSetFont(-1, 12, 400, 0, "MS Sans Serif")
;~ Global $AB3 = GUICtrlCreateInput($currentB3, 316, 198, 121, 28)
;~ GUICtrlSetFont(-1, 12, 400, 0, "MS Sans Serif")
;~ Global $AB4 = GUICtrlCreateInput($currentB4, 316, 238, 121, 28)
;~ GUICtrlSetFont(-1, 12, 400, 0, "MS Sans Serif")
;~ Global $ABSt = GUICtrlCreateInput($currentBS, 316, 278, 121, 28)
;~ GUICtrlSetFont(-1, 12, 400, 0, "MS Sans Serif")
;~ Global $ABCoin = GUICtrlCreateInput($currentBC, 316, 318, 121, 28)
;~ GUICtrlSetFont(-1, 12, 400, 0, "MS Sans Serif")
;~ Global $ABTest = GUICtrlCreateInput($currentBT, 316, 358, 121, 28)
;~ GUICtrlSetFont(-1, 12, 400, 0, "MS Sans Serif")
;~ Global $ABCard = GUICtrlCreateInput($currentBCard, 316, 398, 121, 28)
;~ GUICtrlSetFont(-1, 12, 400, 0, "MS Sans Serif")
;~ Global $ControlSave = GUICtrlCreateButton("Save", 128, 464, 75, 25)
;~ GUICtrlSetFont(-1, 10, 400, 0, "MS Sans Serif")
;~ Global $ControlDefault = GUICtrlCreateButton("Default", 296, 464, 75, 25)
;~ GUICtrlSetFont(-1, 10, 400, 0, "MS Sans Serif")
;~ Global $TabSheet3 = GUICtrlCreateTabItem("Keyboard")
;~ Global $lKeyboard = GUICtrlCreateLabel("Keyboard", 16, 64, 99, 24)
;~ GUICtrlSetFont(-1, 12, 800, 0, "MS Sans Serif")
;~ Global $KeyRadio = GUICtrlCreateRadio("", 136, 64, 17, 17)
;~ GUICtrlSetFont(-1, 8, 800, 0, "MS Sans Serif")
;~ Global $lUp = GUICtrlCreateLabel("Up", 28, 114, 169, 24)
;~ GUICtrlSetFont(-1, 12, 400, 0, "MS Sans Serif")
;~ Global $lDown = GUICtrlCreateLabel("Down", 28, 154, 169, 24)
;~ GUICtrlSetFont(-1, 12, 400, 0, "MS Sans Serif")
;~ Global $lLeft = GUICtrlCreateLabel("Left", 28, 194, 168, 24)
;~ GUICtrlSetFont(-1, 12, 400, 0, "MS Sans Serif")
;~ Global $lRight = GUICtrlCreateLabel("Right", 28, 234, 232, 24)
;~ GUICtrlSetFont(-1, 12, 400, 0, "MS Sans Serif")
;~ Global $lKeyB1 = GUICtrlCreateLabel("Button1", 28, 274, 169, 24)
;~ GUICtrlSetFont(-1, 12, 400, 0, "MS Sans Serif")
;~ Global $lKeyB2 = GUICtrlCreateLabel("Button2", 28, 314, 169, 24)
;~ GUICtrlSetFont(-1, 12, 400, 0, "MS Sans Serif")
;~ Global $lKeyB3 = GUICtrlCreateLabel("Button3", 28, 354, 168, 24)
;~ GUICtrlSetFont(-1, 12, 400, 0, "MS Sans Serif")
;~ Global $lKeyB4 = GUICtrlCreateLabel("Button4", 28, 394, 232, 24)
;~ GUICtrlSetFont(-1, 12, 400, 0, "MS Sans Serif")
;~ Global $lKeyStart = GUICtrlCreateLabel("ArcadeButton Start", 28, 434, 142, 24)
;~ GUICtrlSetFont(-1, 12, 400, 0, "MS Sans Serif")
;~ Global $lKeyCoin = GUICtrlCreateLabel("ArcadeButton Coin", 28, 474, 139, 24)
;~ GUICtrlSetFont(-1, 12, 400, 0, "MS Sans Serif")
;~ Global $lKeyTest = GUICtrlCreateLabel("ArcadeButton Test Menu", 28, 514, 182, 24)
;~ GUICtrlSetFont(-1, 12, 400, 0, "MS Sans Serif")
;~ Global $Up = GUICtrlCreateInput($currentKeyUp, 316, 112, 121, 28)
;~ GUICtrlSetFont(-1, 12, 400, 0, "MS Sans Serif")
;~ Global $Down = GUICtrlCreateInput($currentKeyDown, 316, 152, 121, 28)
;~ GUICtrlSetFont(-1, 12, 400, 0, "MS Sans Serif")
;~ Global $Left = GUICtrlCreateInput($currentKeyLeft, 316, 192, 121, 28)
;~ GUICtrlSetFont(-1, 12, 400, 0, "MS Sans Serif")
;~ Global $Right = GUICtrlCreateInput($currentKeyRight, 316, 232, 121, 28)
;~ GUICtrlSetFont(-1, 12, 400, 0, "MS Sans Serif")
;~ Global $KeyB1 = GUICtrlCreateInput($currentKeyB1, 316, 272, 121, 28)
;~ GUICtrlSetFont(-1, 12, 400, 0, "MS Sans Serif")
;~ Global $KeyB2 = GUICtrlCreateInput($currentKeyB2, 316, 312, 121, 28)
;~ GUICtrlSetFont(-1, 12, 400, 0, "MS Sans Serif")
;~ Global $KeyB3 = GUICtrlCreateInput($currentKeyB3, 316, 352, 121, 28)
;~ GUICtrlSetFont(-1, 12, 400, 0, "MS Sans Serif")
;~ Global $KeyB4 = GUICtrlCreateInput($currentKeyB4, 316, 392, 121, 28)
;~ GUICtrlSetFont(-1, 12, 400, 0, "MS Sans Serif")
;~ Global $KeyStart = GUICtrlCreateInput($currentKeyBS, 316, 432, 121, 28)
;~ GUICtrlSetFont(-1, 12, 400, 0, "MS Sans Serif")
;~ Global $KeyCoin = GUICtrlCreateInput($currentKeyBC, 316, 472, 121, 28)
;~ GUICtrlSetFont(-1, 12, 400, 0, "MS Sans Serif")
;~ Global $KeyTest = GUICtrlCreateInput($currentKeyBT, 316, 512, 121, 28)
;~ GUICtrlSetFont(-1, 12, 400, 0, "MS Sans Serif")
;~ Global $Hyperlink = GUICtrlCreateButton("Link for Input Mappings", 280, 64, 250, 28)
;~ GUICtrlSetFont(-1, 12, 400, 0, "MS Sans Serif")
;~ Global $KeySave = GUICtrlCreateButton("Save", 108, 614, 75, 25)
;~ GUICtrlSetFont(-1, 10, 400, 0, "MS Sans Serif")
;~ Global $KeyDefault = GUICtrlCreateButton("Default", 268, 614, 75, 25)
;~ GUICtrlSetFont(-1, 10, 400, 0, "MS Sans Serif")
;~ Global $KeyHowTo = GUICtrlCreateButton("How To Setup", 436, 606, 121, 33)
;~ GUICtrlSetFont(-1, 12, 400, 0, "MS Sans Serif")
;~ GUISetBkColor($COLOR_LIGHTBLUE)
;~ GUICtrlCreateTabItem("")
;~ GUISetState(@SW_SHOW)
;~ #EndRegion ### END Koda GUI section ###


#Region ### START Koda GUI section ### Form=
Global $XB_Launcher = GUICreate("XBoost Launcher", 800, 720, -1, -1)
Global $bStartServer = GUICtrlCreateButton("Start Server.exe", 592, 144, 131, 33)
GUICtrlSetFont(-1, 10, 400, 0, "MS Sans Serif")
Global $bStartLM = GUICtrlCreateButton("Start LM Mode", 592, 200, 131, 33)
GUICtrlSetFont(-1, 10, 400, 0, "MS Sans Serif")
Global $bStartClient = GUICtrlCreateButton("Start Client Mode", 592, 256, 131, 33)
GUICtrlSetFont(-1, 10, 400, 0, "MS Sans Serif")
Global $bOpenConfig = GUICtrlCreateButton("Open Config.ini", 592, 312, 131, 33)
GUICtrlSetFont(-1, 10, 400, 0, "MS Sans Serif")
Global $bCardWebpage = GUICtrlCreateButton("Card Webpage", 592, 370, 131, 33)
GUICtrlSetFont(-1, 10, 400, 0, "MS Sans Serif")
Global $bExit = GUICtrlCreateButton("Exit", 592, 440, 131, 33)
GUICtrlSetFont(-1, 10, 400, 0, "MS Sans Serif")
Global $tConfig = GUICtrlCreateTab(8, 24, 569, 680)
GUICtrlSetFont(-1, 12, 400, 0, "MS Sans Serif")
Global $TabSheet1 = GUICtrlCreateTabItem("Config")
GUICtrlSetState(-1,$GUI_SHOW)
Global $IPRadio = GUICtrlCreateRadio("", 35, 81, 17, 17)
Global $IP = GUICtrlCreateLabel("IP Address", 56, 81, 114, 24)
GUICtrlSetFont(-1, 12, 400, 0, "MS Sans Serif")
Global $InterfaceRadio = GUICtrlCreateRadio("", 35, 121, 17, 17)
Global $InterfaceName = GUICtrlCreateLabel("InterfaceName", 56, 121, 166, 24)
GUICtrlSetFont(-1, 12, 400, 0, "MS Sans Serif")
Global $Server = GUICtrlCreateLabel("Server.exe Address", 56, 161, 166, 24)
GUICtrlSetFont(-1, 12, 400, 0, "MS Sans Serif")
Global $Windowed = GUICtrlCreateLabel("Windowed Mode", 56, 201, 146, 24)
GUICtrlSetFont(-1, 12, 400, 0, "MS Sans Serif")
Global $iIPAddress = GUICtrlCreateInput($currentIPEnabled, 248, 81, 193, 28)
GUICtrlSetFont(-1, 12, 400, 0, "MS Sans Serif")
;~ Global $iInterfaceName = GUICtrlCreateInput($currentInterfaceNameEnabled, 248, 121, 193, 28)
GUICtrlSetFont(-1, 12, 400, 0, "MS Sans Serif")
Global $combo = GUICtrlCreateCombo("", 248, 121, 150, 120)
GUICtrlSetFont(-1, 12, 400, 0, "MS Sans Serif")
Global $iServer = GUICtrlCreateInput($currentServer, 248, 161, 193, 28)
GUICtrlSetFont(-1, 12, 400, 0, "MS Sans Serif")
Global $WindowCheck = GUICtrlCreateCheckbox("", 259, 201, 20, 20)
GUICtrlSetState(-1, $GUI_CHECKED)
GUICtrlSetFont(-1, 100, 400, 0, "MS Sans Serif")
Global $bIPconfig = GUICtrlCreateButton("IPCONFIG", 200, 314, 83, 25)
GUICtrlSetFont(-1, 10, 400, 0, "MS Sans Serif")
Global $bIAuthdll = GUICtrlCreateButton("iauthdll.bat", 444, 314, 100, 25)
GUICtrlSetFont(-1, 10, 400, 0, "MS Sans Serif")
Global $bIPHowTo = GUICtrlCreateButton("How To Setup", 444, 371, 121, 33)
GUICtrlSetFont(-1, 10, 400, 0, "MS Sans Serif")
Global $bSave = GUICtrlCreateButton("Save", 124, 371, 75, 25)
GUICtrlSetFont(-1, 10, 400, 0, "MS Sans Serif")
Global $bDefault = GUICtrlCreateButton("Default", 292, 371, 75, 25)
GUICtrlSetFont(-1, 10, 400, 0, "MS Sans Serif")
Global $TabSheet2 = GUICtrlCreateTabItem("Controller Settings")
Global $lDirectKey = GUICtrlCreateLabel("DirectInput+Keyboard", 18, 65, 160, 24)
GUICtrlSetFont(-1, 12, 400, 0, "MS Sans Serif")
Global $DIRadioKey = GUICtrlCreateRadio("", 190, 65, 17, 17)
GUICtrlSetFont(-1, 12, 400, 0, "MS Sans Serif")
Global $lDirectInput = GUICtrlCreateLabel("DirectInputOnly", 18, 95, 160, 24)
GUICtrlSetFont(-1, 12, 400, 0, "MS Sans Serif")
Global $DIRadio = GUICtrlCreateRadio("", 190, 95, 17, 17)
GUICtrlSetFont(-1, 12, 400, 0, "MS Sans Serif")
Global $bWindowsUSBGameController = GUICtrlCreateButton("Windows USB Game Controller Options", 268, 59, 283, 33)
GUICtrlSetFont(-1, 10, 400, 0, "MS Sans Serif")
Global $bJoystickRelease = GUICtrlCreateButton("Joystick Detection Tool", 388, 100, 163, 33)
GUICtrlSetFont(-1, 10, 400, 0, "MS Sans Serif")
Global $lArcadeB1 = GUICtrlCreateLabel("Button A", 48, 158, 169, 24)
GUICtrlSetFont(-1, 12, 400, 0, "MS Sans Serif")
Global $lArcadeB3 = GUICtrlCreateLabel("Button C", 48, 238, 168, 24)
GUICtrlSetFont(-1, 12, 400, 0, "MS Sans Serif")
Global $lArcadeB2 = GUICtrlCreateLabel("Button B", 48, 198, 169, 24)
GUICtrlSetFont(-1, 12, 400, 0, "MS Sans Serif")
Global $lArcadeB4 = GUICtrlCreateLabel("Button D", 48, 278, 232, 24)
GUICtrlSetFont(-1, 12, 400, 0, "MS Sans Serif")
Global $lArcadeStart = GUICtrlCreateLabel("Start", 48, 318, 190, 24)
GUICtrlSetFont(-1, 12, 400, 0, "MS Sans Serif")
Global $lArcadeCoin = GUICtrlCreateLabel("Coin", 48, 358, 139, 24)
GUICtrlSetFont(-1, 12, 400, 0, "MS Sans Serif")
Global $lArcadeCard = GUICtrlCreateLabel("Card", 48, 398, 141, 24)
GUICtrlSetFont(-1, 12, 400, 0, "MS Sans Serif")
Global $lArcadeCard = GUICtrlCreateLabel("Device ID", 48, 458, 141, 24)
GUICtrlSetFont(-1, 12, 400, 0, "MS Sans Serif")
Global $AB1 = GUICtrlCreateInput($currentB1, 312, 156, 121, 28)
GUICtrlSetFont(-1, 12, 400, 0, "MS Sans Serif")
Global $AB2 = GUICtrlCreateInput($currentB2, 312, 196, 121, 28)
GUICtrlSetFont(-1, 12, 400, 0, "MS Sans Serif")
Global $AB3 = GUICtrlCreateInput($currentB3, 312, 236, 121, 28)
GUICtrlSetFont(-1, 12, 400, 0, "MS Sans Serif")
Global $AB4 = GUICtrlCreateInput($currentB4, 312, 276, 121, 28)
GUICtrlSetFont(-1, 12, 400, 0, "MS Sans Serif")
Global $ABSt = GUICtrlCreateInput($currentBS, 312, 316, 121, 28)
GUICtrlSetFont(-1, 12, 400, 0, "MS Sans Serif")
Global $ABCoin = GUICtrlCreateInput($currentBC, 312, 356, 121, 28)
GUICtrlSetFont(-1, 12, 400, 0, "MS Sans Serif")
Global $ABCard = GUICtrlCreateInput($currentBCard, 312, 396, 121, 28)
GUICtrlSetFont(-1, 12, 400, 0, "MS Sans Serif")
Global $DeviceID = GUICtrlCreateInput($currentDeviceID, 312, 456, 121, 28)
GUICtrlSetFont(-1, 12, 400, 0, "MS Sans Serif")
Global $ControlSave = GUICtrlCreateButton("Save", 124, 510, 75, 25)
GUICtrlSetFont(-1, 10, 400, 0, "MS Sans Serif")
Global $ControlDefault = GUICtrlCreateButton("Default", 298, 510, 75, 25)
GUICtrlSetFont(-1, 10, 400, 0, "MS Sans Serif")
Global $bDinputHowTo = GUICtrlCreateButton("How To Setup", 444, 510, 121, 33)
GUICtrlSetFont(-1, 10, 400, 0, "MS Sans Serif")
Global $TabSheet3 = GUICtrlCreateTabItem("Keyboard/XInput")
Global $lKeyboard = GUICtrlCreateLabel("Keyboard", 12, 67, 99, 24)
GUICtrlSetFont(-1, 12, 400, 0, "MS Sans Serif")
Global $KeyRadio = GUICtrlCreateRadio("", 132, 67, 17, 17)
GUICtrlSetFont(-1, 8, 400, 0, "MS Sans Serif")
Global $lUp = GUICtrlCreateLabel("Up", 48, 119, 169, 24)
GUICtrlSetFont(-1, 12, 400, 0, "MS Sans Serif")
Global $lDown = GUICtrlCreateLabel("Down", 48, 159, 169, 24)
GUICtrlSetFont(-1, 12, 400, 0, "MS Sans Serif")
Global $lLeft = GUICtrlCreateLabel("Left", 48, 199, 168, 24)
GUICtrlSetFont(-1, 12, 400, 0, "MS Sans Serif")
Global $lRight = GUICtrlCreateLabel("Right", 48, 239, 232, 24)
GUICtrlSetFont(-1, 12, 400, 0, "MS Sans Serif")
Global $lKeyB1 = GUICtrlCreateLabel("Button A", 48, 279, 169, 24)
GUICtrlSetFont(-1, 12, 400, 0, "MS Sans Serif")
Global $lKeyB2 = GUICtrlCreateLabel("Button B", 48, 319, 169, 24)
GUICtrlSetFont(-1, 12, 400, 0, "MS Sans Serif")
Global $lKeyB3 = GUICtrlCreateLabel("Button C", 48, 359, 168, 24)
GUICtrlSetFont(-1, 12, 400, 0, "MS Sans Serif")
Global $lKeyB4 = GUICtrlCreateLabel("Button D", 48, 399, 232, 24)
GUICtrlSetFont(-1, 12, 400, 0, "MS Sans Serif")
Global $lKeyStart = GUICtrlCreateLabel("Start", 48, 439, 142, 24)
GUICtrlSetFont(-1, 12, 400, 0, "MS Sans Serif")
Global $lKeyCoin = GUICtrlCreateLabel("Coin", 48, 479, 139, 24)
GUICtrlSetFont(-1, 12, 400, 0, "MS Sans Serif")
Global $lKeyTest = GUICtrlCreateLabel("Test", 48, 519, 182, 24)
GUICtrlSetFont(-1, 12, 400, 0, "MS Sans Serif")
Global $lKeyCard = GUICtrlCreateLabel("Card", 48, 560, 182, 24)
GUICtrlSetFont(-1, 12, 400, 0, "MS Sans Serif")
Global $lKill = GUICtrlCreateLabel("Exit Program", 48, 599, 182, 24)
GUICtrlSetFont(-1, 12, 400, 0, "MS Sans Serif")
Global $Up = GUICtrlCreateInput($currentKeyUp, 312, 115, 165, 28)
GUICtrlSetFont(-1, 12, 400, 0, "MS Sans Serif")
Global $Down = GUICtrlCreateInput($currentKeyDown, 312, 155, 165, 28)
GUICtrlSetFont(-1, 12, 400, 0, "MS Sans Serif")
Global $Left = GUICtrlCreateInput($currentKeyLeft, 312, 195, 165, 28)
GUICtrlSetFont(-1, 12, 400, 0, "MS Sans Serif")
Global $Right = GUICtrlCreateInput($currentKeyRight, 312, 235, 165, 28)
GUICtrlSetFont(-1, 12, 400, 0, "MS Sans Serif")
Global $KeyB1 = GUICtrlCreateInput($currentKeyB1, 312, 275, 165, 28)
GUICtrlSetFont(-1, 12, 400, 0, "MS Sans Serif")
Global $KeyB2 = GUICtrlCreateInput($currentKeyB2, 312, 315, 165, 28)
GUICtrlSetFont(-1, 12, 400, 0, "MS Sans Serif")
Global $KeyB3 = GUICtrlCreateInput($currentKeyB3, 312, 355, 165, 28)
GUICtrlSetFont(-1, 12, 400, 0, "MS Sans Serif")
Global $KeyB4 = GUICtrlCreateInput($currentKeyB4, 312, 395, 165, 28)
GUICtrlSetFont(-1, 12, 400, 0, "MS Sans Serif")
Global $KeyStart = GUICtrlCreateInput($currentKeyBS, 312, 435, 165, 28)
GUICtrlSetFont(-1, 12, 400, 0, "MS Sans Serif")
Global $KeyCoin = GUICtrlCreateInput($currentKeyBC, 312, 475, 165, 28)
GUICtrlSetFont(-1, 12, 400, 0, "MS Sans Serif")
Global $KeyTest = GUICtrlCreateInput($currentKeyBT, 312, 515, 165, 28)
GUICtrlSetFont(-1, 12, 400, 0, "MS Sans Serif")
Global $KeyCard = GUICtrlCreateInput($currentKeyCard, 312, 555, 165, 28)
GUICtrlSetFont(-1, 12, 400, 0, "MS Sans Serif")
Global $KeyKill = GUICtrlCreateInput($currentKill, 312, 595, 165, 28)
GUICtrlSetFont(-1, 12, 400, 0, "MS Sans Serif")
Global $Hyperlink = GUICtrlCreateButton("Link for Input Mappings", 276, 67, 250, 28)
GUICtrlSetFont(-1, 10, 400, 0, "MS Sans Serif")
Global $KeySave = GUICtrlCreateButton("Save", 104, 640, 75, 25)
GUICtrlSetFont(-1, 10, 400, 0, "MS Sans Serif")
Global $KeyDefault = GUICtrlCreateButton("Default", 264, 640, 75, 25)
GUICtrlSetFont(-1, 10, 400, 0, "MS Sans Serif")
Global $KeyHowTo = GUICtrlCreateButton("How To Setup", 432, 640, 121, 33)
GUICtrlSetFont(-1, 10, 400, 0, "MS Sans Serif")
GUICtrlCreateTabItem("")
GUISetState(@SW_SHOW)
#EndRegion ### END Koda GUI section ###


$PSCommand = Run("powershell Get-NetAdapter -Name * | Where-Object { $_.Status -eq 'Up' } |  Select-Object -ExpandProperty Name", @WindowsDir, @SW_HIDE, 0x2)

$INames = ""

While 1
	$INames &= StdoutRead($PSCommand)
		If @error Then
			ExitLoop
		EndIf
WEnd


Func _PopulateCombo($hwndCTRLID, $vInfo)
    Local $sStoreForCombo = ''
    For $iCount = 1 To UBound($vInfo) - 1
        If $vInfo[$iCount] <> '' Then $sStoreForCombo &= $vInfo[$iCount] & '|'
    Next
    GUICtrlSetData($hwndCTRLID, $sStoreForCombo)
EndFunc

FileOpen(@ScriptDir & "\Temp.txt", 0)
FileWrite(".\Temp.txt", $INames)

Local $ComboItems
_FileReadToArray(".\Temp.txt", $ComboItems)
_PopulateCombo($combo, $ComboItems)

FileClose(".\Temp.txt")

FileDelete(".\Temp.txt")

$InterfaceArray = IniReadSection($filedir, "config")
$keyvalue4 = $InterfaceArray[4][0]
$keyvalue5 = $InterfaceArray[5][0]
If $keyvalue4 = "# InterfaceName" Then
	GUICtrlSetState($InterfaceRadio, $GUI_UNCHECKED)
  	GUICtrlSetData($combo, $currentInterfaceNameDisabled)
 	GUICtrlSetState($combo, $GUI_DISABLE)
Else
	GUICtrlSetState($InterfaceRadio, $GUI_CHECKED)
	GUICtrlSetData($combo, $currentInterfaceNameEnabled)
EndIf

If $keyvalue5 = "# IpAddress" Then
	GUICtrlSetState($IPRadio, $GUI_UNCHECKED)
 	GUICtrlSetData($iIPAddress, $currentIPDisabled)
	GUICtrlSetState($iIPAddress, $GUI_DISABLE)
Else
 	GUICtrlSetState($IPRadio, $GUI_CHECKED)
EndIf


$size = FileGetSize("bngrw.dll")
If $size <> "137728" Then

Else

Msgbox(16, "test", "Please check the bngrw.dll file, it is not the correct file size.")

EndIf

If Not $iFileExists Then ; Used to check if config.ini exists, if it does not it will Error with Message Box
 	MsgBox (16, "Error", $Filename & " is not found. Please make sure you copied the config.ini file over to Game Directory and restart the launcher.")
EndIf

Local $Wtrue = IniRead($filedir, "Config", "windowed", "default") ; Variable use to read value of windowed in config.ini

If $Wtrue = "true" Then ; Used to check if windowed value is true in config.ini, changes state of the checkbox based on value.
	GUICtrlSetState($WindowCheck,$GUI_CHECKED)
Else
	GUICtrlSetState($WindowCheck,$GUI_UNCHECKED)
EndIf


While 1

	$nMsg = GUIGetMsg()
	Select
		Case $nMsg = $bExit ; Case structure for Exit Button in GUI
			Exit

		Case $nMsg = BitAND(GUICtrlRead($iPRadio), $GUI_CHECKED) = $GUI_CHECKED
			GUICtrlSetState($iIPAddress, $GUI_DISABLE)
			GUICtrlSetState($combo, $GUI_ENABLE)

		Case $nMsg = BitAND(GUICtrlRead($combo), $GUI_CHECKED) = $GUI_CHECKED
			GUICtrlSetState($combo, $GUI_DISABLE)
			GUICtrlSetState($iIPAddress, $GUI_ENABLE)

		Case $nMsg = $bDefault ; Case structure for Default button on Config Tab, resets all current data in Input fields to default value
   			GUICtrlSetData($iIPAddress, $defaultIP)
			GUICtrlSetData($combo, $defaultInterfaceName)
			GUICtrlSetData($iServer, $defaultServer)
			GUICtrlSetState($WindowCheck, $GUI_CHECKED)

		Case $nMsg = $ControlDefault ; Case structure for Default button on Controller Settings Tab, resets all current data in Input fields to default value
			GUICtrlSetData($AB1, $defaultB1)
			GUICtrlSetData($AB2, $defaultB2)
			GUICtrlSetData($AB3, $defaultB3)
			GUICtrlSetData($AB4, $defaultB4)
			GUICtrlSetData($ABSt, $defaultBS)
			GUICtrlSetData($ABCoin, $defaultBC)
			GUICtrlSetData($ABCard, $defaultBCard)

		Case $nMsg = $KeyDefault ; Case structure for Default button on the Keyboard tab, resets all current data in Input fields to default value
			GUICtrlSetData($Up, $defaultKeyUp)
			GUICtrlSetData($Down, $defaultKeyDown)
			GUICtrlSetData($Left, $defaultKeyLeft)
			GUICtrlSetData($Right, $defaultKeyRight)
			GUICtrlSetData($KeyB1, $defaultKeyB1)
			GUICtrlSetData($KeyB2, $defaultKeyB2)
			GUICtrlSetData($KeyB3, $defaultKeyB3)
			GUICtrlSetData($KeyB4, $defaultKeyB4)
			GUICtrlSetData($KeyStart, $defaultKeyBS)
			GUICtrlSetData($KeyCoin, $defaultKeyBC)
			GUICtrlSetData($KeyTest, $defaultKeyBT)
			GUICtrlSetData($KeyCard, $defaultKeyCard)
			GUICtrlSetData($KeyKill, $defaultKeyKill)

		Case $nMsg = $bSave ; Case structure for Save button on Config Tab, takes all of the data in Input fields and saves to config.ini
			If GUICtrlRead($IPRadio) = 1 AND $keyvalue5 = "# IPAddress" Then
				_ReplaceStringInFile($Filename, "# IpAddress =", "IpAddress =")
				_ReplaceStringInFile($Filename, "InterfaceName =", "# InterfaceName =")
				IniWrite($Filename, "Config", "IpAddress", " " & GuiCtrlRead($iIPAddress))
			ElseIf GUICtrlRead($InterfaceRadio) = 1 AND $keyvalue4 = "# InterfaceName" Then
				_ReplaceStringInFile($Filename, "# InterfaceName =", "InterfaceName =")
				_ReplaceStringInFile($Filename, "IpAddress =", "# IpAddress =")
 				IniWrite($Filename, "Config", "InterfaceName", " " & GUICtrlRead($combo))
			ElseIf GUICtrlRead($IPRadio) = 1 AND $keyvalue5 = "IPAddress" Then
				IniWrite($Filename, "Config", "IpAddress", " " & GuiCtrlRead($iIPAddress))
			ElseIf GUICtrlRead($InterfaceRadio) = 1 AND $keyvalue4 = "InterfaceName" Then
				IniWrite($Filename, "Config", "InterfaceName", " " & GUICtrlRead($combo))
			EndIf


			IniWrite($Filename, "Config", "Server", " " & GUICtrlRead($iServer))

			If GUICtrlRead($WindowCheck) = $GUI_CHECKED Then
				IniWrite($Filename, "Config", "windowed", " " & "true")
			Else
				IniWrite($Filename, "Config", "windowed", " " & "false")
			EndIf

			MsgBox (0, "Success", "Settings Saved")
			$2ndReadArray = IniReadSection($filedir, "config")
			$keyvalue4 = $2ndReadArray[4][0]
			$keyvalue5 = $2ndReadArray[5][0]

		Case $nMsg = $ControlSave ; Case structure for Save button on Controllers Settings Tab, takes all of the data in Input fields and saves to config.ini
			IniWrite($Filename, "dinput", "A", " " & GUICtrlRead($AB1))
			IniWrite($Filename, "dinput", "B", " " & GUICtrlRead($AB2))
			IniWrite($Filename, "dinput", "C", " " & GUICtrlRead($AB3))
			IniWrite($Filename, "dinput", "D", " " & GUICtrlRead($AB4))
			IniWrite($Filename, "dinput", "Start", " " & GUICtrlRead($ABSt))
			IniWrite($Filename, "dinput", "Coin", " " & GUICtrlRead($ABCoin))
			IniWrite($Filename, "dinput", "Card", " " & GUICtrlRead($ABCard))

			If GUICtrlRead($DIRadio) = 1 Then
				IniWrite($Filename, "Config", "InputMode", " " & "DirectInputOnly")
			EndIf

			If GUICtrlRead($DIRadioKey) = 1 Then
				IniWrite($Filename, "Config", "InputMode", " " & "DirectInput")
			EndIf

			MsgBox (0, "Success", "Settings Saved")

		Case $nMsg = $KeySave ; Case structure for Save button on Keyboard Tab, takes all of the data in Input fields and saves to config.ini
			IniWrite($Filename, "keyboard", "Up", " " & GUICtrlRead($Up))
			IniWrite($Filename, "keyboard", "Down", " " & GUICtrlRead($Down))
			IniWrite($Filename, "keyboard", "Left", " " & GUICtrlRead($Left))
			IniWrite($Filename, "keyboard", "Right", " " & GUICtrlRead($Right))
			IniWrite($Filename, "keyboard", "A", " " & GUICtrlRead($KeyB1))
			IniWrite($Filename, "keyboard", "B", " " & GUICtrlRead($KeyB2))
			IniWrite($Filename, "keyboard", "C", " " & GUICtrlRead($KeyB3))
			IniWrite($Filename, "keyboard", "D", " " & GUICtrlRead($KeyB4))
			IniWrite($Filename, "keyboard", "Start", " " & GUICtrlRead($KeyStart))
			IniWrite($Filename, "keyboard", "Coin", " " & GUICtrlRead($KeyCoin))
			IniWrite($Filename, "keyboard", "Test", " " & GUICtrlRead($KeyTest))
			IniWrite($Filename, "keyboard", "Card", " " & GUICtrlRead($KeyCard))
			IniWrite($Filename, "keyboard", "Kill", " " & GUICtrlRead($KeyKill))

			If GUICtrlRead($KeyRadio) = 1 Then
				IniWrite($Filename, "Config", "InputMode", " " & "Keyboard")
			EndIf

			MsgBox (0, "Success", "Settings Saved")

		Case $nMsg = $bIAuthdll
			If FileExists(".\AMCUS\iauthdll.bat") Then
				ShellExecute(".\AMCUS\iauthdll.bat","","","runas")
			EndIf

		Case $nMsg = $bIPconfig ; Case structure for the IPCONFIG button in the Config Tab to run the ipconfig batch file and open the outputfile.
			If FileExists(".\TOOLS\ipconfig.bat") Then
			Run(".\TOOLS\ipconfig.bat")
			Sleep(1000)
			Run("notepad.exe " & $ipconfigout, @WindowsDir)
			Else
			MsgBox (16, "Error", "The file ipconfig.bat is not found. Please make sure you copied the files over to the Game Directory")
			EndIf

		Case $nMsg = $bJoystickRelease ; Case structure for the JoystickDetection button to run the JoystickDetection_Realease application
			If FileExists(".\TOOLS\JoystickDetection_Release.exe") Then
			Run(".\TOOLS\JoystickDetection_Release.exe")
			Else
			MsgBox (16, "Error", "The file JoystickDetection_Release.exe is not found. Please make sure you copied the TOOLS folder over to the Game Directory")
			EndIf

		Case $nMsg = $Hyperlink ; Case structure for the Link for Input Mappings button that will open browser link to Input Mappings for Keyboard
			ShellExecute("https://gist.github.com/emilianavt/f4b2d4e221235f55e8e5d3fb8ea769ed")

		Case $nMsg = $bOpenConfig ; Case structure for the Open Config.ini button
			Run("notepad.exe " & $filedir, @WindowsDir)

		Case $nMsg = $bStartLM ; Case structure for the Start LM Mode button, verifies the file is there before booting.
			If FileExists("run_xboost_LM_mode_v3.exe") Then
			Run("run_xboost_LM_mode_v3.exe")
			Else
			MsgBox (16, "Error", "The file run_xboost_LM_mode_v3.exe is not found. Please make sure you copied the files over to Game Directory")
			EndIf

		Case $nMsg = $bStartClient ; Case structure for the Start Client Mode button, verifies the file is there before booting.
			If FileExists("run_xboost_CLIENT_mode_v3.exe") Then
			Run("run_xboost_CLIENT_mode_v3.exe")
			Else
			MsgBox (16, "Error", "The file run_xboost_CLIENT_mode_v3.exe is not found. Please make sure you copied the files over to Game Directory")
			EndIf

		Case $nMsg = $bStartServer ; Case structure for the Start Server button, verifies the file is there before booting.
			If FileExists("run_server.bat") Then
			Run("run_server.bat", @ScriptDir)
			Else
			MsgBox (16, "Error", "The file run_server.bat is not found. Please make sure you copied the files over to Game Directory")
			EndIf

		Case $nMsg = $bCardWebpage  ; Case structure for accessing the Card Webpage button.
			ShellExecute("http://" & $currentServer & "/index")

		Case $nMsg = $bWindowsUSBGameController ; Case structure for the JoystickDetection button to run the JoystickDetection_Realease application
			Run("control.exe joy.cpl,,4")

		Case $nMsg = $bIPHowTo ; Case structure for the How To button in the Config Tab
			MsgBox(32, "How To", "Click the 'iauthdll.bat' button first as it is required on first install." &@CRLF&@CRLF& _
			"Click the 'IPCONFIG' button to run the ipconfig batch and it will then open the output values in a text document." &@CRLF&@CRLF& _
			"Use this document to fill in the fields properly for the Internet Network Adapter used." &@CRLF&@CRLF& _
			"Please select IpAddress or InterfaceName Radio Button for your Network settings, and enter the IP Address of computer running Server.exe in the Server field, if you run it on this, leave it as 127.0.0.1")

		Case $nMsg = $bDinputHowTo ; Case structure for the How To button in the Controller Settings Tab
			MsgBox(32, "How To", "When using DirectInput+Keyboard or DirectInputOnly, please plug in your Gamepad/Arcadestick and click the 'Windows USB Game Controller Options' Button." &@CRLF&@CRLF& _
			"This will bring up the Game Controllers control panel, select your Gamepad/Arcadestick and choose properties." &@CRLF &@CRLF& _
			"Press the buttons you wish to use and remember the buttons number on the ." &@CRLF&@CRLF& _
			"If you wish to use a specific DeviceID for your Gamepad/Arcadestick, click the 'Joystick Detection Tool' Button and press a button to find 'Joystick ID = #' ")

		Case $nMsg = $KeyHowTo ; Case structure for the How To button in the Keyboard Tab
			MsgBox(32, "How To", "If you wish to use Keyboard, select the radio button to adjust the InputMode in Config.ini to Keyboard." &@CRLF&@CRLF& _
			"To obtain the correct mappings, please click the button 'Link for Input Mappings' to be taken to website with ID mappings." &@CRLF&@CRLF& _
			"You can use a comma ',' in between mappings to have each command map to more than 1 input."  &@CRLF&@CRLF& _
			"Example: Start  Q,W,E,R,T,Y - All the letters will be mapped to Start.")


	EndSelect


	Switch $nMsg
		Case $GUI_EVENT_CLOSE ; when GUI is closing, to use Exit command
			Exit

	EndSwitch

WEnd


