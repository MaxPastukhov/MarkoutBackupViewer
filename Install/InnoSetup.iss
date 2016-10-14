#define ApplicationName "Markout Backup Viewer"
#define ApplicationDir "Markout Backup Viewer"
#define ApplicationNameWithoutSpaces "MarkoutBackupViewer"
#define ApplicationBinaryDir "c:\MarkoutBackupViewer\trunk\Build"
#define InstallationStaticDir "c:\MarkoutBackupViewer\trunk\Static"
#define OutputDir "c:\MarkoutBackupViewer\trunk\Install\"
#define LicenseFile "c:\MarkoutBackupViewer\trunk\Install\License.txt"
#define HomePageUrl "https://markout.org/"
#define ExeFileName "MarkoutBackupViewer.exe"
#define ExeFileVersion GetFileVersion("c:\MarkoutBackupViewer\trunk\Build\MarkoutBackupViewer.exe")

[Setup]
AppName={#ApplicationName}
AppVerName={#ApplicationName} v{#ExeFileVersion}
AppPublisher=Markout.org
AppPublisherURL={#HomePageUrl}
AppSupportURL={#HomePageUrl}
AppUpdatesURL={#HomePageUrl}
DefaultDirName="{pf}\{#ApplicationDir}"
DisableDirPage=no
DefaultGroupName={#ApplicationDir}
AllowNoIcons=yes
LicenseFile="{#LicenseFile}"
OutputDir="{#OutputDir}"
OutputBaseFilename={#ApplicationNameWithoutSpaces}Install-{#ExeFileVersion}
ChangesAssociations=yes

[Tasks]
Name: "desktopicon"; Description: "Add shortcut to the desktop"; GroupDescription: "Addition shortcuts:";
Name: "quicklaunchicon"; Description: "Add shortcut to quicklaunch panel"; GroupDescription: "Addition shortcuts:";

[Files]
; основной исполняемый файл
Source: "{#ApplicationBinaryDir}\{#ExeFileName}"; DestDir: "{app}"; Flags: ignoreversion
; лицензия
Source: "{#LicenseFile}"; DestDir: "{app}"; DestName: "license.txt"; Flags: ignoreversion
; иконки
Source: "{#InstallationStaticDir}\icons.png"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#InstallationStaticDir}\icons.txt"; DestDir: "{app}"; Flags: ignoreversion

[INI]
Filename: "{app}\HomePage.url"; Section: "InternetShortcut"; Key: "URL"; String: "{#HomePageUrl}"

[Icons]
Name: "{group}\Launch {#ApplicationName}"; Filename: "{app}\{#ExeFileName}"; WorkingDir: "{app}"
; NOTE: The following entry contains an English phrase ("on the Web"). You are free to translate it into another language if required.
Name: "{group}\Home Page"; Filename: "{app}\HomePage.url"
; NOTE: The following entry contains an English phrase ("Uninstall"). You are free to translate it into another language if required.
Name: "{group}\Uninstall"; Filename: "{uninstallexe}"; WorkingDir: "{app}"
Name: "{userdesktop}\{#ApplicationName}"; Filename: "{app}\{#ExeFileName}"; Tasks: desktopicon; WorkingDir: "{app}"
Name: "{userappdata}\Microsoft\Internet Explorer\Quick Launch\{#ApplicationName}"; Filename: "{app}\{#ExeFileName}"; Tasks: quicklaunchicon; WorkingDir: "{app}"

[Registry]
Root: HKCR; Subkey: ".markout";                             ValueData: "{#ApplicationName}";          Flags: uninsdeletevalue; ValueType: string;  ValueName: ""
Root: HKCR; Subkey: "{#ApplicationName}";                     ValueData: "Program {#ApplicationName}";  Flags: uninsdeletekey;   ValueType: string;  ValueName: ""
Root: HKCR; Subkey: "{#ApplicationName}\DefaultIcon";             ValueData: "{app}\{#ExeFileName},0";               ValueType: string;  ValueName: ""
Root: HKCR; Subkey: "{#ApplicationName}\shell\open\command";  ValueData: """{app}\{#ExeFileName}"" ""%1""";  ValueType: string;  ValueName: ""

[Run]
Filename: "{app}\{#ExeFileName}"; Description: "Launch {#ApplicationName}"; Flags: nowait postinstall skipifsilent

[UninstallDelete]
Type: files; Name: "{app}\HomePage.url"

[CustomMessages]
dotnetmissing=This application needs Microsoft .Net 3.5 which is not yet installed. Do you like to download it now?
  
[Code]
function InitializeSetup(): Boolean;
var
  ErrorCode: Integer;
  netFrameWorkInstalled : Boolean;
  isInstalled: Cardinal;
begin
  result := true;
   
  // Check for the .Net 3.5 framework
  isInstalled := 0;
  netFrameworkInstalled := RegQueryDWordValue(HKLM, 'SOFTWARE\Microsoft\NET Framework Setup\NDP\v3.5', 'Install', isInstalled);
  if ((netFrameworkInstalled)  and (isInstalled <> 1)) then netFrameworkInstalled := false;
   
  if netFrameworkInstalled = false then
  begin
    if (MsgBox(ExpandConstant('{cm:dotnetmissing}'), mbConfirmation, MB_YESNO) = idYes) then
    begin
      ShellExec('open',
      'http://www.microsoft.com/downloads/details.aspx?FamilyID=333325fd-ae52-4e35-b531-508d977d32a6&DisplayLang=en',
      '','',SW_SHOWNORMAL,ewNoWait,ErrorCode);
    end;
    result := false;
  end;
   
end;

