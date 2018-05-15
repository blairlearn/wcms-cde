@echo off
setlocal

::
::  Usage:
::
::     deploy-cde <release-name> <target-environment>
::
::          release-name - The release name.
::          target-environment - The WCMS environment being targeted.
::
::  Required environment variables:
::
::      NEXUS_USER - UserID for accessing the Nexus repository.
::      NEXUS_PASS - Password for the Nexus repository.
::
:: Assumptions:
::  1. The release name is the same as the tag name.
::  2. The ZIP file containing the executables is the same as the tag and release names
::      (With a .zip extension)
::  3. The ZIP with config files in the nexus repository with:
::      a. The name of the tag/relase.
::      b. The hash of the tag
::      c. A .zip extension
::         So for the pepperoni-blue-29 release, the config file would be
::         pepperoni-blue-29-0a9521...61d6.zip (But with more digits).


if "%1"=="" GOTO Usage
if "%2"=="" GOTO Usage
set RELEASE_NAME=%1
set TARGET_ENV=%2

if "%GIT_URL%"=="" (
    echo Required environment variable 'GIT_URL' not set.
    exit /b 1
)

@if "%NEXUS_USER%"=="" (
    echo Nexus login credentials not set correctly ^(NEXUS_USER^).
    exit /b 1
)

@if "%NEXUS_PASS%"=="" (
    echo Nexus login credentials not set correctly ^(NEXUS_PASS^).
    exit /b 1
)


rmdir /s/q _dist
mkdir _dist\code && mkdir _dist\config

:: Download and deploy CDE code.
echo Downloading code archive '%RELEASE_NAME%.zip'.
powershell -executionpolicy unrestricted tools\build\build-tools\github-tools\download-release.ps1 -gitHubUsername %GH_ORGANIZATION_NAME% -gitHubRepository %GH_REPO_NAME% -releaseName %RELEASE_NAME% -saveToPath _dist\code\distribution.zip
if errorlevel 1 goto Error

powershell -executionpolicy unrestricted tools\build\build-tools\zip-tools\expand-zip -source _dist\code\distribution.zip -destinationPath _dist\code\
if errorlevel 1 goto Error

call _dist\code\cdeDeploy.bat
if errorlevel 1 goto Error

:: Download and deploy CDE configuration

:: Get the hash for the release.
for /f %%a in ('git ls-remote -t %GIT_URL% %RELEASE_NAME%') do set RELEASE_HASH=%%a

:: Build name of config .zip file.
set CONFIG_ZIP=%RELEASE_NAME%-%RELEASE_HASH%.zip

:: Download
echo Downloading configuration archive '%CONFIG_ZIP%'.
powershell -executionpolicy unrestricted tools\build\build-tools\nexus-tools\nexus-download.ps1 -Filename %CONFIG_ZIP% -UserID %NEXUS_USER% -UserPass %NEXUS_PASS% -SaveToPath _dist\config\config.zip
if errorlevel 1 goto Error

powershell -executionpolicy unrestricted tools\build\build-tools\zip-tools\expand-zip -source _dist\config\config.zip -destinationPath _dist\config\
if errorlevel 1 goto Error

call _dist\config\configDeploy.bat %NODE_NAME%

goto :EOF
:Usage
echo.
echo. Usage
echo.   %~nx0 ^<release-name^> ^<target-environment^>
echo.
exit /b 1
:Error
echo An error has occured.
exit /b 1
pause