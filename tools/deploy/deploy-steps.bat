if "%GIT_URL%"=="" (
    echo Required environment variable 'GIT_URL' not set.
    exit /b 1
)

rmdir /s/q _dist && mkdir _dist
powershell -executionpolicy unrestricted tools\build\build-tools\github-tools\download-release.ps1 -gitHubUsername nciocpl -gitHubRepository wcms-cde -releaseName %release_name% -saveToPath _dist\distribution.zip
powershell -executionpolicy unrestricted tools\build\build-tools\zip-tools\expand-zip -source _dist\distribution.zip -destinationPath _dist
rem _dist\cdeDeploy.bat

:: Get the hash for the release.
for /f %%a in ('git ls-remote -t %GIT_URL% %release_name%') do set RELEASE_HASH=%%a

echo '%RELEASE_HASH%'
