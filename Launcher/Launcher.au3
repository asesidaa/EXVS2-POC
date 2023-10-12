#Region ;**** Directives created by AutoIt3Wrapper_GUI ****
#AutoIt3Wrapper_UseX64=y
#AutoIt3Wrapper_Res_Fileversion=1.2.0
#AutoIt3Wrapper_Res_Fileversion_AutoIncrement=p
#AutoIt3Wrapper_Run_After=mkdir "%scriptdir%\verau3"
#AutoIt3Wrapper_Run_After=mkdir "%scriptdir%\verexe"
#AutoIt3Wrapper_Run_After=copy /V /Y "%in%" "%scriptdir%\verau3\%scriptfile%_%fileversion%.au3"
#AutoIt3Wrapper_Run_After=copy /V /Y "%out%" "%scriptdir%\verexe\%scriptfile%_%fileversion%.exe"
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
Global $currentIPEnabled = IniRead($filedir, "Config", "IpAddress", "default") ; Variable that reads current value of IpAddress in Config.ini
Global $currentIPDisabled = IniRead($filedir, "Config", "# IpAddress", "default"); Variable that reads current value of Commented IpAddress in Config.ini
Global $currentServer = IniRead($filedir, "Config", "Server", "default") ; Variable that reads current value of Server in Config.ini
Global $currentInterfaceNameEnabled = IniRead($filedir, "Config", "InterfaceName", "default") ; Variable that reads current value of InterfaceName in Config.ini
Global $currentInterfaceNameDisabled = IniRead($filedir, "Config", "# InterfaceName", "default") ; Variable that reads current value of Commented InterfaceName in Config.ini
Global $defaultIP = "192.168.0.100" ; Variable for Default Value of IpAddress in Config.ini
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
Global $currentDeviceId = IniRead($filedir, "dinput", "DeviceId", "default") ; Variable that reads current value of DeviceID in Config.ini for DirectInput
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
Global $currentKill = IniRead($filedir, "keyboard", "Kill", "default") ; Variable that reads the current value of Kill in Config.ini for Keyboard
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
Global $defaultKeyKill = "Esc" ; Variable for the Default Value of Kill in Config.ini for Keyboard
Global $Clicked = False ; Boolean Variable for confirming if Switch Language button is pressed

#EndRegion ### Global Variables Section End ###

#Region ### GUI Variables used for Language Switching ###

Global $GUI_baseElements = 8
Global $GUI_configElements = 11
Global $GUI_controllerElements = 17
Global $GUI_keyboardElements = 20
Global $currentLang = 0
Global $baseGUI[$GUI_baseElements]
Global $configGUI[$GUI_configElements]
Global $controllerGUI[$GUI_controllerElements]
Global $keyboardGUI[$GUI_keyboardElements]
Global $baseGUILang[$GUI_baseElements][2] = [["XBoost Launcher", "XBoost 启动器"], ["Start Server.exe", "开始 Server.exe"], ["Start LM Mode", "启动 LM"], ["Start Client Mode", "启动 Client"], ["Open Config.ini", "打开Config.ini"], ["Card Webpage", "卡片网页"], ["Switch Language", "改变语言"], ["Exit", "退出"]]
Global $configGUILang[$GUI_configElements][2] = [["", ""], ["Config", "配置"], ["IP Address", "IP 地址"], ["InterfaceName", "网卡名称"], ["Server.exe Address", "Server.exe 地址"], ["Windowed Mode", "窗口模式"], ["Run ipconfig.bat", "运行 ipconfig.bat"], ["Initialize iauthdll.bat", "初始化 iauthdll.bat"], ["Save", "保存"], ["Restore defaults", "恢复默认选项"], ["How To Setup", "如何设置"]]
Global $controllerGUILang[$GUI_controllerElements][2] = [["", ""], ["Controller Settings", "按键 设置"], ["DirectInput+Keyboard", "DInput+键盘"], ["DirectInputOnly", "仅 DInput"], ["Windows USB Game Controller Options", "Windows USB 按键设置"], ["Joystick Detection Tool", "操纵杆 检测 工具"], ["Button A", "按钮 A"], ["Button B", "按钮 B"], ["Button C", "按钮 C"], ["Button D", "按钮 D"], ["Start", "启动"], ["Coin", "硬币"], ["Card", "卡片"], ["Device ID", "设备 ID"], ["Save", "保存"], ["Restore defaults", "恢复默认选项"], ["How To Setup", "如何设置"]]
Global $keyboardGUILang[$GUI_keyboardElements][2] = [["", ""], ["Keyboard/XInput", "键盘/X Input"], ["Keyboard", "键盘"], ["Link for Input Mappings", "按键设定参考网址"], ["Up", "上"], ["Down", "下"], ["Left", "左"], ["Right", "右"], ["Button A", "按钮 A"], ["Button B", "按钮 B"], ["Button C", "按钮 C"], ["Button D", "按钮 D"], ["Start", "启动"], ["Coin", "硬币"], ["Test", "测试"], ["Card", "卡片"], ["Exit Program", "退出程序"], ["Save", "保存"], ["Restore defaults", "恢复默认选项"], ["How To Setup", "如何设置"]]

Global $ENconfigHowTo = "Click the 'iauthdll.bat' button first as it is required on first install." &@CRLF&@CRLF& _
			"Click the 'Run ipconfig.bat' button to run the ipconfig batch script and it will then open the output values in a text document." &@CRLF&@CRLF& _
			"Use this document to fill in the fields properly for the Internet Network Adapter used." &@CRLF&@CRLF& _
			"Please select IpAddress or InterfaceName Radio Button for your Network settings, and enter the IP Address of computer running Server.exe in the Server field, if you run it on this, leave it as 127.0.0.1"
Global $ENcontrollerHowTo = "When using DirectInput+Keyboard or DirectInputOnly, please plug in your Gamepad/Arcadestick and click the 'Windows USB Game Controller Options' Button." &@CRLF&@CRLF& _
			"This will bring up the Game Controllers control panel, select your Gamepad/Arcadestick and choose properties." &@CRLF &@CRLF& _
			"Press the buttons you wish to use and remember the buttons number on the ." &@CRLF&@CRLF& _
			"If you wish to use a specific DeviceID for your Gamepad/Arcadestick, click the 'Joystick Detection Tool' Button and press a button to find 'Joystick ID = #' "
Global $ENkeyboardHowTo = "If you wish to use Keyboard, select the radio button to adjust the InputMode in Config.ini to Keyboard." &@CRLF&@CRLF& _
			"To obtain the correct mappings, please click the button 'Link for Input Mappings' to be taken to website with ID mappings." &@CRLF&@CRLF& _
			"You can use a comma ',' in between mappings to have each command map to more than 1 input."  &@CRLF&@CRLF& _
			"Example: Start  Q,W,E,R,T,Y - All the letters will be mapped to Start."
Global $CNconfigHowTo = "如果是第一次运行，请按下初始化 iauthdll.bat" &@CRLF&@CRLF& _
			"请按下 '运行 ipconfig.bat' 按钮运行 ipconfig.bat 脚本，完成后 Ip 信息将会被写在 .txt 文档里." &@CRLF&@CRLF& _
			"请选择Ip地址或者网卡名称，然后根据你选的选项填入相对的信息." &@CRLF&@CRLF& _
			"完成后，请别忘记填入网卡服务器地址 (Server.exe)，自行运行的 Server.exe 都默认为 127.0.0.1。"
Global $CNcontrollerHowTo = "Enter Translation here"
Global $CNkeyboardHowTo = "Enter Translation here"

#EndRegion


#Region ### BaseGUI Variables in Koda ###

$baseGUI[0] = GUICreate($baseGUILang[0][$currentLang], 800, 720, -1, -1)
$baseGUI[1] = GUICtrlCreateButton($baseGUILang[1][$currentLang], 592, 144, 131, 33)
GUICtrlSetFont(-1, 10, 400, 0, "MS Sans Serif")
$baseGUI[2] = GUICtrlCreateButton($baseGUILang[2][$currentLang], 592, 200, 131, 33)
GUICtrlSetFont(-1, 10, 400, 0, "MS Sans Serif")
$baseGUI[3] = GUICtrlCreateButton($baseGUILang[3][$currentLang], 592, 256, 131, 33)
GUICtrlSetFont(-1, 10, 400, 0, "MS Sans Serif")
$baseGUI[4] = GUICtrlCreateButton($baseGUILang[4][$currentLang], 592, 312, 131, 33)
GUICtrlSetFont(-1, 10, 400, 0, "MS Sans Serif")
$baseGUI[5] = GUICtrlCreateButton($baseGUILang[5][$currentLang], 592, 370, 131, 33)
GUICtrlSetFont(-1, 10, 400, 0, "MS Sans Serif")
$baseGUI[6] = GUICtrlCreateButton($baseGUILang[6][$currentLang], 592, 430, 131, 33)
GUICtrlSetFont(-1, 10, 400, 0, "MS Sans Serif")
$baseGUI[7] = GUICtrlCreateButton($baseGUILang[7][$currentLang], 592, 480, 131, 33)
GUICtrlSetFont(-1, 10, 400, 0, "MS Sans Serif")
$tConfig = GUICtrlCreateTab(8, 24, 569, 680)
GUICtrlSetFont(-1, 12, 400, 0, "MS Sans Serif")

#EndRegion

#Region ### ConfigGUI Variables in Koda ###

$configGUI[1] = GUICtrlCreateTabItem($configGUILang[1][$currentLang])
GUICtrlSetState(-1,$GUI_SHOW)
$configGUI[2] = GUICtrlCreateLabel($configGUILang[2][$currentLang], 56, 81, 114, 24)
GUICtrlSetFont(-1, 12, 400, 0, "MS Sans Serif")
$configGUI[3] = GUICtrlCreateLabel($configGUILang[3][$currentLang], 56, 121, 166, 24)
GUICtrlSetFont(-1, 12, 400, 0, "MS Sans Serif")
$configGUI[4]= GUICtrlCreateLabel($configGUILang[4][$currentLang], 56, 161, 166, 24)
GUICtrlSetFont(-1, 12, 400, 0, "MS Sans Serif")
$configGUI[5] = GUICtrlCreateLabel($configGUILang[5][$currentLang], 56, 201, 146, 24)
GUICtrlSetFont(-1, 12, 400, 0, "MS Sans Serif")
$configGUI[6]= GUICtrlCreateButton($configGUILang[6][$currentLang], 50, 300, 200, 40) ; ipconfig
GUICtrlSetFont(-1, 10, 400, 0, "MS Sans Serif")
$configGUI[7] = GUICtrlCreateButton($configGUILang[7][$currentLang], 50, 250, 200, 40) ; iauthdll
GUICtrlSetFont(-1, 10, 400, 0, "MS Sans Serif")
$configGUI[8] = GUICtrlCreateButton($configGUILang[8][$currentLang], 50, 650, 150, 40) ; Save; Setup for Multilanguage array
GUICtrlSetFont(-1, 10, 400, 0, "MS Sans Serif")
$configGUI[9] = GUICtrlCreateButton($configGUILang[9][$currentLang], 400, 650, 150, 40) ; Restore defaults
GUICtrlSetFont(-1, 10, 400, 0, "MS Sans Serif")
$configGUI[10] = GUICtrlCreateButton($configGUILang[10][$currentLang], 590, 70, 150, 50) ; How to setup
GUICtrlSetFont(-1, 10, 400, 0, "MS Sans Serif")
Global $IPRadio = GUICtrlCreateRadio("", 35, 81, 17, 17)
Global $InterfaceRadio = GUICtrlCreateRadio("", 35, 121, 17, 17)
Global $iIPAddress = GUICtrlCreateInput($currentIPEnabled, 248, 81, 193, 28)
GUICtrlSetFont(-1, 12, 400, 0, "MS Sans Serif")
Global $combo = GUICtrlCreateCombo("", 248, 121, 150, 120)
GUICtrlSetFont(-1, 12, 400, 0, "MS Sans Serif")
Global $iServer = GUICtrlCreateInput($currentServer, 248, 161, 193, 28)
GUICtrlSetFont(-1, 12, 400, 0, "MS Sans Serif")
Global $WindowCheck = GUICtrlCreateCheckbox("", 259, 201, 20, 20)
GUICtrlSetState(-1, $GUI_CHECKED)
GUICtrlSetFont(-1, 100, 400, 0, "MS Sans Serif")

#EndRegion

#Region ### ControllerGUI Variables in Koda ###

$controllerGUI[1] = GUICtrlCreateTabItem($controllerGUILang[1][$currentLang])
$controllerGUI[2] = GUICtrlCreateLabel($controllerGUILang[2][$currentLang], 18, 65, 160, 24)
GUICtrlSetFont(-1, 12, 400, 0, "MS Sans Serif")
$controllerGUI[3] = GUICtrlCreateLabel($controllerGUILang[3][$currentLang], 18, 95, 160, 24)
GUICtrlSetFont(-1, 12, 400, 0, "MS Sans Serif")
$controllerGUI[4] = GUICtrlCreateButton($controllerGUILang[4][$currentLang], 268, 59, 283, 33)
GUICtrlSetFont(-1, 10, 400, 0, "MS Sans Serif")
$controllerGUI[5] = GUICtrlCreateButton($controllerGUILang[5][$currentLang], 388, 100, 163, 33)
GUICtrlSetFont(-1, 10, 400, 0, "MS Sans Serif")
$controllerGUI[6] = GUICtrlCreateLabel($controllerGUILang[6][$currentLang], 48, 158, 169, 24)
GUICtrlSetFont(-1, 12, 400, 0, "MS Sans Serif")
$controllerGUI[7] = GUICtrlCreateLabel($controllerGUILang[7][$currentLang], 48, 238, 168, 24)
GUICtrlSetFont(-1, 12, 400, 0, "MS Sans Serif")
$controllerGUI[8] = GUICtrlCreateLabel($controllerGUILang[8][$currentLang], 48, 198, 169, 24)
GUICtrlSetFont(-1, 12, 400, 0, "MS Sans Serif")
$controllerGUI[9] = GUICtrlCreateLabel($controllerGUILang[9][$currentLang], 48, 278, 232, 24)
GUICtrlSetFont(-1, 12, 400, 0, "MS Sans Serif")
$controllerGUI[10] = GUICtrlCreateLabel($controllerGUILang[10][$currentLang], 48, 318, 190, 24)
GUICtrlSetFont(-1, 12, 400, 0, "MS Sans Serif")
$controllerGUI[11] = GUICtrlCreateLabel($controllerGUILang[11][$currentLang], 48, 358, 139, 24)
GUICtrlSetFont(-1, 12, 400, 0, "MS Sans Serif")
$controllerGUI[12] = GUICtrlCreateLabel($controllerGUILang[12][$currentLang], 48, 398, 141, 24)
GUICtrlSetFont(-1, 12, 400, 0, "MS Sans Serif")
$controllerGUI[13] = GUICtrlCreateLabel($controllerGUILang[13][$currentLang], 48, 458, 141, 24)
GUICtrlSetFont(-1, 12, 400, 0, "MS Sans Serif")
$controllerGUI[14] = GUICtrlCreateButton($controllerGUILang[14][$currentLang], 50, 650, 150, 40) ; Save
GUICtrlSetFont(-1, 10, 400, 0, "MS Sans Serif")
$controllerGUI[15] = GUICtrlCreateButton($controllerGUILang[15][$currentLang], 400, 650, 150, 40) ; Restore defaults 
GUICtrlSetFont(-1, 10, 400, 0, "MS Sans Serif")
$controllerGUI[16] = GUICtrlCreateButton($controllerGUILang[16][$currentLang], 590, 70, 150, 50) ; How to setup
GUICtrlSetFont(-1, 10, 400, 0, "MS Sans Serif")
Global $DIRadioKey = GUICtrlCreateRadio("", 190, 65, 17, 17)
Global $DIRadio = GUICtrlCreateRadio("", 190, 95, 17, 17)
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

#EndRegion

#Region ### KeyboardGUI Variables in Koda ###

$keyboardGUI[1] = GUICtrlCreateTabItem($keyboardGUILang[1][$currentLang])
$keyboardGUI[2] = GUICtrlCreateLabel($keyboardGUILang[2][$currentLang], 12, 67, 99, 24)
GUICtrlSetFont(-1, 12, 400, 0, "MS Sans Serif")
$keyboardGUI[3] = GUICtrlCreateButton($keyboardGUILang[3][$currentLang], 276, 67, 250, 28)
GUICtrlSetFont(-1, 10, 400, 0, "MS Sans Serif")
$keyboardGUI[4] = GUICtrlCreateLabel($keyboardGUILang[4][$currentLang], 48, 119, 169, 24)
GUICtrlSetFont(-1, 12, 400, 0, "MS Sans Serif")
$keyboardGUI[5] = GUICtrlCreateLabel($keyboardGUILang[5][$currentLang], 48, 159, 169, 24)
GUICtrlSetFont(-1, 12, 400, 0, "MS Sans Serif")
$keyboardGUI[6] = GUICtrlCreateLabel($keyboardGUILang[6][$currentLang], 48, 199, 168, 24)
GUICtrlSetFont(-1, 12, 400, 0, "MS Sans Serif")
$keyboardGUI[7] = GUICtrlCreateLabel($keyboardGUILang[7][$currentLang], 48, 239, 232, 24)
GUICtrlSetFont(-1, 12, 400, 0, "MS Sans Serif")
$keyboardGUI[8] = GUICtrlCreateLabel($keyboardGUILang[8][$currentLang], 48, 279, 169, 24)
GUICtrlSetFont(-1, 12, 400, 0, "MS Sans Serif")
$keyboardGUI[9] = GUICtrlCreateLabel($keyboardGUILang[9][$currentLang], 48, 319, 169, 24)
GUICtrlSetFont(-1, 12, 400, 0, "MS Sans Serif")
$keyboardGUI[10] = GUICtrlCreateLabel($keyboardGUILang[10][$currentLang], 48, 359, 168, 24)
GUICtrlSetFont(-1, 12, 400, 0, "MS Sans Serif")
$keyboardGUI[11] = GUICtrlCreateLabel($keyboardGUILang[11][$currentLang], 48, 399, 232, 24)
GUICtrlSetFont(-1, 12, 400, 0, "MS Sans Serif")
$keyboardGUI[12] = GUICtrlCreateLabel($keyboardGUILang[12][$currentLang], 48, 439, 142, 24)
GUICtrlSetFont(-1, 12, 400, 0, "MS Sans Serif")
$keyboardGUI[13] = GUICtrlCreateLabel($keyboardGUILang[13][$currentLang], 48, 479, 139, 24)
GUICtrlSetFont(-1, 12, 400, 0, "MS Sans Serif")
$keyboardGUI[14] = GUICtrlCreateLabel($keyboardGUILang[14][$currentLang], 48, 519, 182, 24)
GUICtrlSetFont(-1, 12, 400, 0, "MS Sans Serif")
$keyboardGUI[15] = GUICtrlCreateLabel($keyboardGUILang[15][$currentLang], 48, 560, 182, 24)
GUICtrlSetFont(-1, 12, 400, 0, "MS Sans Serif")
$keyboardGUI[16] = GUICtrlCreateLabel($keyboardGUILang[16][$currentLang], 48, 599, 182, 24)
GUICtrlSetFont(-1, 12, 400, 0, "MS Sans Serif")
$keyboardGUI[17] = GUICtrlCreateButton($keyboardGUILang[17][$currentLang], 50, 650, 150, 40) ; Save
GUICtrlSetFont(-1, 10, 400, 0, "MS Sans Serif")
$keyboardGUI[18] = GUICtrlCreateButton($keyboardGUILang[18][$currentLang], 400, 650, 150, 40) ; Restore defaults 
GUICtrlSetFont(-1, 10, 400, 0, "MS Sans Serif")
$keyboardGUI[19] = GUICtrlCreateButton($keyboardGUILang[19][$currentLang], 590, 70, 150, 50) ; How to setup
GUICtrlSetFont(-1, 10, 400, 0, "MS Sans Serif")
Global $KeyRadio = GUICtrlCreateRadio("", 132, 67, 17, 17)
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
GUICtrlCreateTabItem("")
GUISetState(@SW_SHOW)

#EndRegion

#Region ### File Checker ##

$size = FileGetSize("bngrw.dll")
If $size <> "137728" Then

Else

Msgbox(16, "test", "Please check the bngrw.dll file, it is not the correct file size.")

EndIf

If Not $iFileExists Then ; Used to check if config.ini exists, if it does not it will Error with Message Box
 	MsgBox (16, "Error", $Filename & " is not found. Please make sure you copied the config.ini file over to Game Directory and restart the launcher.")
EndIf

#EndRegion

#Region ### Process to get InterfaceNames  ###

; Run Powershell to get InterfaceNames on computer an
Local $PSCommand = Run("powershell Get-NetAdapter -Name * | Where-Object { $_.Status -eq 'Up' } |  Select-Object -ExpandProperty Name", @WindowsDir, @SW_HIDE, 0x2)
Local $INames = ""

While 1
	$INames &= StdoutRead($PSCommand)
		If @error Then
			ExitLoop
		EndIf
WEnd

FileOpen(@ScriptDir & "\Temp.txt", 0)
FileWrite(".\Temp.txt", $INames)

Local $ComboItems
_FileReadToArray(".\Temp.txt", $ComboItems)
_PopulateCombo($combo, $ComboItems)

FileClose(".\Temp.txt")
FileDelete(".\Temp.txt")

Func _PopulateCombo($hwndCTRLID, $vInfo)
    Local $sStoreForCombo = ''
    For $iCount = 1 To UBound($vInfo) - 1
        If $vInfo[$iCount] <> '' Then $sStoreForCombo &= $vInfo[$iCount] & '|'
    Next
    GUICtrlSetData($hwndCTRLID, $sStoreForCombo)
EndFunc

#EndRegion

#Region ### Switch Language Function ###

Func _SwitchLanguage()
	If $currentLang = 0 Then
		$currentLang = 1
	Else
		$currentLang = 0
	EndIf

	WinSetTitle($baseGUI[0], "", $baseGUILang[0][$currentLang])

	For $i = 1 To UBound($baseGUI) - 1
		GUICtrlSetData($baseGUI[$i], $baseGUILang[$i][$currentLang])
	Next

	For $i = 1 To UBound($configGUI) - 1
		GUICtrlSetData($configGUI[$i], $configGUILang[$i][$currentLang])
	Next

	For $i = 1 To UBound($controllerGUI) - 1
	GUICtrlSetData($controllerGUI[$i], $controllerGUILang[$i][$currentLang])
	Next

	For $i = 1 To UBound($keyboardGUI) - 1
	GUICtrlSetData($keyboardGUI[$i], $keyboardGUILang[$i][$currentLang])
	Next
EndFunc

#EndRegion

#Region ### Find IPAddress/InterfaceName in INI ###

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

#EndRegion


#Region ### Windowed Mode Checkbox Confirm ###

Local $Wtrue = IniRead($filedir, "Config", "windowed", "default") ; Variable use to read value of windowed in config.ini

If $Wtrue = "true" Then
	GUICtrlSetState($WindowCheck,$GUI_CHECKED)
Else
	GUICtrlSetState($WindowCheck,$GUI_UNCHECKED)
EndIf

#EndRegion

#Region ### When GUI buttons/radios/checkboxes are used ###

While 1

	$nMsg = GUIGetMsg()
	Select
		Case $nMsg = $InterfaceRadio ; Case structure for the InterfaceNames Radio Button
			GUICtrlSetState($iIPAddress, $GUI_DISABLE)
			GUICtrlSetState($combo, $GUI_ENABLE)

		Case $nMsg = $IPRadio ; Case structure for the IP Address Readio Button
			GUICtrlSetState($combo, $GUI_DISABLE)
			GUICtrlSetState($iIPAddress, $GUI_ENABLE)

		Case $nMsg = $baseGUI[1] ; Case structure for the Start Server button, verifies the file is there before booting.
			If FileExists("run_server.bat") Then
				Run("run_server.bat", @ScriptDir)
			Else
				MsgBox (16, "Error", "The file run_server.bat is not found. Please make sure you copied the files over to Game Directory")
			EndIf

		Case $nMsg = $baseGUI[2] ; Case structure for the Start LM Mode button, verifies the file is there before booting.
			If FileExists("run_xboost_LM_mode_v3.exe") Then
				Run("run_xboost_LM_mode_v3.exe")
			Else
				MsgBox (16, "Error", "The file run_xboost_LM_mode_v3.exe is not found. Please make sure you copied the files over to Game Directory")
			EndIf

		Case $nMsg = $baseGUI[3] ; Case structure for the Start Client Mode button, verifies the file is there before booting.
			If FileExists("run_xboost_CLIENT_mode_v3.exe") Then
			Run("run_xboost_CLIENT_mode_v3.exe")
			Else
			MsgBox (16, "Error", "The file run_xboost_CLIENT_mode_v3.exe is not found. Please make sure you copied the files over to Game Directory")
			EndIf

		Case $nMsg = $baseGUI[4] ; Case structure for the Open Config.ini button
			Run("notepad.exe " & $filedir, @WindowsDir)

		Case $nMsg = $baseGUI[6] ; Case structure for the Switch Language Button
			_SwitchLanguage()
			If $Clicked = True Then
				$Clicked = False
			Else
				$Clicked = True
			EndIf

		Case $nMsg = $baseGUI[5]  ; Case structure for accessing the Card Webpage button.
			ShellExecute("http://" & $currentServer & "/index")

		Case $nMsg = $baseGUI[7] ; Case structure for Exit Button in GUI
			Exit

		Case $nMsg = $configGUI[6] ; Case structure for the IPCONFIG button in the Config Tab to run the ipconfig batch file and open the outputfile.
			If FileExists(".\TOOLS\ipconfig.bat") Then
			Run(".\TOOLS\ipconfig.bat")
			Sleep(1000)
			Run("notepad.exe " & $ipconfigout, @WindowsDir)
			Else
			MsgBox (16, "Error", "The file ipconfig.bat is not found. Please make sure you copied the files over to the Game Directory")
			EndIf

		Case $nMsg = $configGUI[7] ; Case structure for running the iauthdll.bat button
			If FileExists(".\AMCUS\iauthdll.bat") Then
				ShellExecute(".\AMCUS\iauthdll.bat","","","runas")
			EndIf

		Case $nMsg = $configGUI[8] ; Case structure for Save button on Config Tab, takes all of the data in Input fields and saves to config.ini
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

			MsgBox (0, "Success", "Settings Saved")

		Case $nMsg = $configGUI[9] ; Case structure for Default button on Config Tab, resets all current data in Input fields to default value
   			GUICtrlSetData($iIPAddress, $defaultIP)
			GUICtrlSetData($combo, $defaultInterfaceName)
			GUICtrlSetData($iServer, $defaultServer)
			GUICtrlSetState($WindowCheck, $GUI_CHECKED)

		Case $nMsg = $configGUI[10] ; Case structure for the How To button in the Config Tab

			If $Clicked = True Then
			MsgBox(32, "How To", $CNconfigHowTo)
			Else
			MsgBox(32, "How To", $ENconfigHowTo )
			EndIf

		Case $nMsg = $controllerGUI[4] ; Case structure for the JoystickDetection button to run the JoystickDetection_Realease application
			Run("control.exe joy.cpl,,4")

		Case $nMsg = $controllerGUI[5] ; Case structure for the JoystickDetection button to run the JoystickDetection_Realease application
			If FileExists(".\TOOLS\JoystickDetection_Release.exe") Then
			Run(".\TOOLS\JoystickDetection_Release.exe")
			Else
			MsgBox (16, "Error", "The file JoystickDetection_Release.exe is not found. Please make sure you copied the TOOLS folder over to the Game Directory")
			EndIf

		Case $nMsg = $controllerGUI[14] ; Case structure for Save button on Controllers Settings Tab, takes all of the data in Input fields and saves to config.ini
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

		Case $nMsg = $controllerGUI[15] ; Case structure for Default button on Controller Settings Tab, resets all current data in Input fields to default value
			GUICtrlSetData($AB1, $defaultB1)
			GUICtrlSetData($AB2, $defaultB2)
			GUICtrlSetData($AB3, $defaultB3)
			GUICtrlSetData($AB4, $defaultB4)
			GUICtrlSetData($ABSt, $defaultBS)
			GUICtrlSetData($ABCoin, $defaultBC)
			GUICtrlSetData($ABCard, $defaultBCard)

		Case $nMsg = $controllerGUI[16] ; Case structure for the How To button in the Controller Settings Tab
			If $Clicked = True Then
			MsgBox(32, "How To", $CNcontrollerHowTo)
			Else
			MsgBox(32, "How To", $ENcontrollerHowTo )
			EndIf


		Case $nMsg = $keyboardGUI[3] ; Case structure for the Link for Input Mappings button that will open browser link to Input Mappings for Keyboard
			ShellExecute("https://gist.github.com/emilianavt/f4b2d4e221235f55e8e5d3fb8ea769ed")

		Case $nMsg = $keyboardGUI[17] ; Case structure for Save button on Keyboard Tab, takes all of the data in Input fields and saves to config.ini
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

		Case $nMsg = $keyboardGUI[18] ; Case structure for Default button on the Keyboard tab, resets all current data in Input fields to default value
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


			IniWrite($Filename, "Config", "Server", " " & GUICtrlRead($iServer))

			If GUICtrlRead($WindowCheck) = $GUI_CHECKED Then
				IniWrite($Filename, "Config", "windowed", " " & "true")
			Else
				IniWrite($Filename, "Config", "windowed", " " & "false")
			EndIf

		Case $nMsg = $keyboardGUI[19] ; Case structure for the How To button in the Keyboard Tab
			If $Clicked = True Then
			MsgBox(32, "How To", $CNkeyboardHowTo)
			Else
			MsgBox(32, "How To", $ENkeyboardHowTo )
			EndIf

	EndSelect

#EndRegion

#Region ### Close Switch at top right of window ###

	Switch $nMsg
		Case $GUI_EVENT_CLOSE ; when GUI is closing, to use Exit command
			Exit
	EndSwitch

WEnd

#EndRegion

