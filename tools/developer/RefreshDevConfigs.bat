@echo off
SETLOCAL

@set PATH=C:\Program Files (x86)\Microsoft Visual Studio 12.0\Common7\IDE\CommonExtensions\Microsoft\TestWindow;C:\Program Files (x86)\Microsoft SDKs\F#\3.1\Framework\v4.0\;C:\Program Files (x86)\Microsoft SDKs\TypeScript\1.0;C:\Program Files (x86)\MSBuild\12.0\bin;C:\Program Files (x86)\Microsoft Visual Studio 12.0\Common7\IDE\;C:\Program Files (x86)\Microsoft Visual Studio 12.0\VC\BIN;C:\Program Files (x86)\Microsoft Visual Studio 12.0\Common7\Tools;C:\Windows\Microsoft.NET\Framework\v4.0.30319;C:\Program Files (x86)\Microsoft Visual Studio 12.0\VC\VCPackages;C:\Program Files (x86)\HTML Help Workshop;C:\Program Files (x86)\Microsoft Visual Studio 12.0\Team Tools\Performance Tools;C:\Program Files (x86)\Windows Kits\8.1\bin\x86;C:\Program Files (x86)\Microsoft SDKs\Windows\v8.1A\bin\NETFX 4.5.1 Tools\;%PATH%
@set INCLUDE=C:\Program Files (x86)\Microsoft Visual Studio 12.0\VC\INCLUDE;C:\Program Files (x86)\Microsoft Visual Studio 12.0\VC\ATLMFC\INCLUDE;C:\Program Files (x86)\Windows Kits\8.1\include\shared;C:\Program Files (x86)\Windows Kits\8.1\include\um;C:\Program Files (x86)\Windows Kits\8.1\include\winrt;
@set LIB=C:\Program Files (x86)\Microsoft Visual Studio 12.0\VC\LIB;C:\Program Files (x86)\Microsoft Visual Studio 12.0\VC\ATLMFC\LIB;C:\Program Files (x86)\Windows Kits\8.1\lib\winv6.3\um\x86;
@set LIBPATH=C:\Windows\Microsoft.NET\Framework\v4.0.30319;C:\Program Files (x86)\Microsoft Visual Studio 12.0\VC\LIB;C:\Program Files (x86)\Microsoft Visual Studio 12.0\VC\ATLMFC\LIB;C:\Program Files (x86)\Windows Kits\8.1\References\CommonConfiguration\Neutral;C:\Program Files (x86)\Microsoft SDKs\Windows\v8.1\ExtensionSDKs\Microsoft.VCLibs\12.0\References\CommonConfiguration\neutral;

REM If set, then set 
set my_configsrc=%1

REM If set, then set 
set my_subslist=%2

REM Get the current working directory
set workspace=%~dp0

REM Get the current source root of the CDE
CALL :RESOLVE "%workspace%\..\.." my_source

REM Get Current Branch
set my_branch=
for /f %%I in ('git.exe rev-parse --abbrev-ref HEAD 2^> NUL') do set my_branch=%%I

REM The configs will need a build number, but no GITHUB_TOKEN 
REM In the future they will need some sort of nexus login information

REM Validate Branch and Target
set FAIL=
set ADD_PARAMS=
IF "%my_branch%"=="" set FAIL=True
IF "%my_source%"=="" set FAIL=True
IF "%my_configsrc%" NEQ "" (
	IF "%my_subslist%"=="" ( 
		set FAIL=True		
	) ELSE (
		set ADD_PARAMS=-SubstituteList %my_subslist% -ConfigRepo %my_configsrc%
	)
)

IF "%FAIL%" NEQ "" (
	ECHO.
	ECHO You must call this script from within a local CDE git repository. 
	ECHO USAGE:
	ECHO 	c:\RefreahDevConfigs.bat [^<CONFIG_SRC_REPO^> ^<SUBSTITUTE_LIST_FILE^>]
	ECHO		CONFIG_SRC_REPO - The root of your wcms-cde-config clone
	ECHO		SUBSTITUTE_LIST_FILE - The path to the subsititue list file
	GOTO :EOF
)

ECHO.
ECHO Building Dev Configs for Branch %my_branch% targeting CDE folder %my_source%

REM Determine the current Git commit hash.
REM FOR /f %%a IN ('git rev-parse --verify HEAD') DO SET COMMIT_ID=%%a

REM ECHO Building for %my_target% using Branch %my_branch%
REM msbuild /fileLogger /t:All "/p:TargetEnvironment=%my_target%;Branch=%my_branch%"  "%WORKSPACE%\tools\build\BuildConfig.xml"



powershell -NonInteractive -ExecutionPolicy RemoteSigned %workspace%RefreshDevConfigs.ps1 -SourceRoot %my_source% -Branch %my_branch% %ADD_PARAMS%

timeout /t 30

GOTO :EOF

:RESOLVE
SET %2=%~f1 
GOTO :EOF