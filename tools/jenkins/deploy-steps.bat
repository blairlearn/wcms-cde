mkdir _dist
powershell -executionpolicy unrestricted github-tools\download-release.ps1 -gitHubUsername nciocpl -gitHubRepository wcms-cde -releaseName %release_name% -saveToPath _dist\distribution.zip
powershell -executionpolicy unrestricted zip-tools\expand-zip -source _dist\distribution.zip -destinationPath _dist
_dist\cdeDeploy.bat