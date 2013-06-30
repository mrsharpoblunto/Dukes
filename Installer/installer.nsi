;Includes

  !include "MUI2.nsh"
  !include "FileFunc.nsh" 
  
;--------------------------------

; The name of the installer
	Name "Dukes"
	OutFile "DukesSetup.exe"

; The default installation directory
	InstallDir "$PROGRAMFILES\Dukes"

	RequestExecutionLevel admin

;--------------------------------
;Interface Configuration

  !define MUI_ICON "Dukes.ico"
  !define MUI_UNICON "Dukes.ico"


  !define MUI_HEADERIMAGE
  !define MUI_HEADER_TRANSPARENT_TEXT
  !define MUI_BGCOLOR 000000
  !define MUI_HEADERIMAGE_BITMAP "DukesHeader.bmp"
  !define MUI_HEADERIMAGE_BITMAP_NOSTRETCH
  !define MUI_HEADERIMAGE_UNBITMAP "DukesHeader.bmp"
  !define MUI_HEADERIMAGE_UNBITMAP_NOSTRETCH
  !define MUI_ABORTWARNING

;--------------------------------
;Constants

  !define CSIDL_APPDATA '0x1A' ;Application Data path

;--------------------------------
;Pages

  !insertmacro MUI_PAGE_DIRECTORY
  !insertmacro MUI_PAGE_INSTFILES
  
  !insertmacro MUI_UNPAGE_CONFIRM
  !insertmacro MUI_UNPAGE_INSTFILES

;--------------------------------
;Languages
 
  !insertmacro MUI_LANGUAGE "English"
  
;--------------------------------
; check for and install .net 4.5
Section "Install .NET Framework 4.5"
	Call CheckAndInstallDotNet
SectionEnd

Section "Install Dukes"
	SetOutPath $INSTDIR
	File /r /x *.svn /x *.pdb /x *.vshost.exe.config /x *.vshost.exe /x *.exe.manifest /x *.xml "..\DukesServer\bin\Release\*"
	File "Dukes.ico"

	SetOutPath $INSTDIR

;--- Desktop shortcut
	CreateShortCut "$DESKTOP\Dukes Server.lnk" "$INSTDIR\DukesServer.exe"
	
;--- Start Menu
	CreateDirectory "$SMPROGRAMS\Dukes"
	CreateShortCut "$SMPROGRAMS\Dukes\Dukes Server.lnk" "$INSTDIR\DukesServer.exe" "" "$INSTDIR\dukes.ico"
	
;--- write out the add/remove program's registry keys
	SetRegView 64
	WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\Dukes" "UninstallString" "$INSTDIR\Uninstall.exe"
	WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\Dukes" "DisplayName" "The Dukebox of Hazzard"	
	WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\Dukes" "DisplayIcon" "$INSTDIR\dukes.ico"
	WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\Dukes" "InstallLocation" "$INSTDIR"
	WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\Dukes" "URLInfoAbout" "www.sharpoblunto.com"
	
	${GetSize} "$INSTDIR" "/S=0K" $0 $1 $2
	IntFmt $0 "0x%08X" $0
	WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\Dukes" "EstimatedSize" "$0"
	
	WriteUninstaller $INSTDIR\Uninstall.exe	
SectionEnd



Section "Uninstall"
;--- Remove Start Menu launcher
	Delete "$SMPROGRAMS\Dukes\Dukes Server.lnk"
;--- Try to remove the Start Menu folder - this will only happen if it is empty
	RMDIR "$SMPROGRAMS\Dukes"
	
;--- remove Desktop shortcut
	Delete "$DESKTOP\Dukes Server.lnk"

;--- remove uninstaller ---
	Delete $INSTDIR\Uninstall.exe
	
	RMDIR /r $INSTDIR
SectionEnd

Function CheckAndInstallDotNet
;--- Magic numbers from http://msdn.microsoft.com/en-us/library/ee942965.aspx
    ClearErrors
    ReadRegDWORD $0 HKLM "SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full" "Release"

    IfErrors NotDetected

    ${If} $0 >= 378389

        DetailPrint "Microsoft .NET Framework 4.5 is installed ($0)"
    ${Else}
    NotDetected:
        DetailPrint "Installing Microsoft .NET Framework 4.5"
        SetDetailsPrint listonly
		SetOutPath $TEMP\Dukes
        ExecWait '"$TEMP\Dukes\dotNetFx45_Full_setup.exe" /passive /norestart' $0
        ${If} $0 == 3010 
        ${OrIf} $0 == 1641
            DetailPrint "Microsoft .NET Framework 4.5 installer requested reboot"
            SetRebootFlag true
        ${EndIf}
        SetDetailsPrint lastused
        DetailPrint "Microsoft .NET Framework 4.5 installer returned $0"
		Delete $TEMP\Dukes\DotNetFx45_Full_Setup.exe
    ${EndIf}

FunctionEnd
