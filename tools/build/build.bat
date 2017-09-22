@echo off
SETLOCAL

@set PATH=C:\Program Files (x86)\Microsoft Visual Studio 12.0\Common7\IDE\CommonExtensions\Microsoft\TestWindow;C:\Program Files (x86)\Microsoft SDKs\F#\3.1\Framework\v4.0\;C:\Program Files (x86)\Microsoft SDKs\TypeScript\1.0;C:\Program Files (x86)\MSBuild\12.0\bin;C:\Program Files (x86)\Microsoft Visual Studio 12.0\Common7\IDE\;C:\Program Files (x86)\Microsoft Visual Studio 12.0\VC\BIN;C:\Program Files (x86)\Microsoft Visual Studio 12.0\Common7\Tools;C:\Windows\Microsoft.NET\Framework\v4.0.30319;C:\Program Files (x86)\Microsoft Visual Studio 12.0\VC\VCPackages;C:\Program Files (x86)\HTML Help Workshop;C:\Program Files (x86)\Microsoft Visual Studio 12.0\Team Tools\Performance Tools;C:\Program Files (x86)\Windows Kits\8.1\bin\x86;C:\Program Files (x86)\Microsoft SDKs\Windows\v8.1A\bin\NETFX 4.5.1 Tools\;%PATH%
@set INCLUDE=C:\Program Files (x86)\Microsoft Visual Studio 12.0\VC\INCLUDE;C:\Program Files (x86)\Microsoft Visual Studio 12.0\VC\ATLMFC\INCLUDE;C:\Program Files (x86)\Windows Kits\8.1\include\shared;C:\Program Files (x86)\Windows Kits\8.1\include\um;C:\Program Files (x86)\Windows Kits\8.1\include\winrt;
@set LIB=C:\Program Files (x86)\Microsoft Visual Studio 12.0\VC\LIB;C:\Program Files (x86)\Microsoft Visual Studio 12.0\VC\ATLMFC\LIB;C:\Program Files (x86)\Windows Kits\8.1\lib\winv6.3\um\x86;
@set LIBPATH=C:\Windows\Microsoft.NET\Framework\v4.0.30319;C:\Program Files (x86)\Microsoft Visual Studio 12.0\VC\LIB;C:\Program Files (x86)\Microsoft Visual Studio 12.0\VC\ATLMFC\LIB;C:\Program Files (x86)\Windows Kits\8.1\References\CommonConfiguration\Neutral;C:\Program Files (x86)\Microsoft SDKs\Windows\v8.1\ExtensionSDKs\Microsoft.VCLibs\12.0\References\CommonConfiguration\neutral;

REM Get the branch from the command line
set my_branch=%1

REM Determine the Build Environment Name.  This is used for tagging and proper config deployment

rem Get the build environment name from the command line
set my_target=%2



REM Validate Branch and Target
set FAIL=
IF "%my_branch%"=="" set FAIL=True
IF "%my_target%"=="" set FAIL=True
IF "%WORKSPACE%"=="" set FAIL=True
IF "%TEMP%"=="" set FAIL=True
IF "%BUILD_NUMBER%"=="" set FAIL=True
IF "%GITHUB_TOKEN%"=="" set FAIL=True


IF "%FAIL%" NEQ "" (
	ECHO.
	ECHO You must pass the branch and build environment names and to this script.
	ECHO USAGE:
	ECHO 	c:\Build.bat ^<branch^> ^<environment^>
	ECHO Additonally, the following environment variables must be set:
	ECHO 	WORKSPACE - directory containing the source code.
	ECHO	TEMP - Location for temporary files
	ECHO	BUILD_NUMBER - Build number ^(automatically generated/set by Jenkins^)
	ECHO	GITHUB_TOKEN - GitHub access token for the build user.
	GOTO :EOF
)

REM Determine the current Git commit hash.
FOR /f %%a IN ('git rev-parse --verify HEAD') DO SET COMMIT_ID=%%a

REM Inject placeholder configuration files.
set placeholder=%WORKSPACE%\tools\build\resources\placeholder.config
copy "%placeholder%" "build\CDEFramework\Libraries\NCILibrary\Code\NCILibrary.Services.Dictionary\Web.config"

copy "%placeholder%" "CDESites\CancerGov\SiteSpecific\CancerGov.Web\web.config"
copy "%placeholder%" "CDESites\DCEG\SiteSpecific\DCEG.Web\web.config"
copy "%placeholder%" "CDESites\Imaging\SiteSpecific\Imaging.Web\web.config"
copy "%placeholder%" "CDESites\MobileCancerGov\SiteSpecific\MobileCancerGov.Web\web.config"
copy "%placeholder%" "CDESites\Proteomics\SiteSpecific\Proteomics.Web\web.config"
copy "%placeholder%" "CDESites\TCGA\SiteSpecific\TCGA.Web\web.config"

copy "%placeholder%" "CDEFramework\Libraries\NCILibrary\Code\NCILibrary.Modules.Search\app.config"
copy "%placeholder%" "CDEFramework\Libraries\NCILibrary\Code\NCILibrary.Search.BestBets\app.config"
copy "%placeholder%" "CDEFramework\Libraries\NCILibrary\Code\NCILibrary.Web.CDE\app.config"
copy "%placeholder%" "CDEFramework\Libraries\NCILibrary\Code\NCILibrary.Web.CDE.UI\app.config"
copy "%placeholder%" "CDEFramework\Libraries\NCILibrary\UnitTests\NCILibrary.Web.CDE.Test\app.config"
copy "%placeholder%" "CDESites\CancerGov\CancerGovSpecific\CancerGov.Handlers\app.config"
copy "%placeholder%" "CDESites\CancerGov\CancerGovSpecific\CancerGov.Modules\app.config"
copy "%placeholder%" "CDESites\CancerGov\PresentationClasses\app.config"
copy "%placeholder%" "CDESites\CancerGov\SiteSpecific\Modules\CancerGov.APICTS.Test\app.config"
copy "%placeholder%" "CDESites\CancerGov\SiteSpecific\Modules\CancerGov.BasicCTS\app.config"
copy "%placeholder%" "CDESites\CancerGov\SiteSpecific\Modules\CancerGov.BasicCTSv2\app.config"
copy "%placeholder%" "CDESites\CancerGov\SiteSpecific\Modules\CancerGov.ClinicalTrialsAPI\app.config"
copy "%placeholder%" "CDESites\CancerGov\SiteSpecific\Modules\CancerGov.ClinicalTrialsAPI.Test\app.config"
copy "%placeholder%" "CDESites\CancerGov\SiteSpecific\Modules\CancerGov.Search.AutoSuggest\app.config"
copy "%placeholder%" "CDESites\CancerGov\SiteSpecific\Modules\CancerGov.Search.BestBets\app.config"
copy "%placeholder%" "CDESites\CancerGov\SiteSpecific\Modules\CancerGov.Search.Endeca\app.config"


ECHO Building for %my_target% using Branch %my_branch%
msbuild /fileLogger /t:All "/p:TargetEnvironment=%my_target%;Branch=%my_branch%"  "%WORKSPACE%\tools\build\BuildCDE.xml"
