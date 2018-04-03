mkdir _dist
powershell -executionpolicy unrestricted tools\build\build-tools\github-tools\download-release.ps1 -gitHubUsername nciocpl -gitHubRepository wcms-cde -releaseName %release_name% -saveToPath _dist\distribution.zip
powershell -executionpolicy unrestricted tools\build\build-tools\zip-tools\expand-zip -source _dist\distribution.zip -destinationPath _dist
rem _dist\cdeDeploy.bat