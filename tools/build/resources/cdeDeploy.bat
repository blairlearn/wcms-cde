@echo off
rem Get paths into variables
rem Batpath gets the path to this bat file.
rem codepath gets the path the user is running from, which
rem should be the root of the code to be deployed.
set batpath=%~dp0
set codepath=%cd%

setLocal

REM Determine whether this system gets the full set of sites, or just the colo sites.
set my_target=
IF "%IS_COLO%"=="" (
	set my_target=FULL
) ELSE (
	set my_target=Colo
)

rem Do a backup first.
powershell -ExecutionPolicy BYPASS -FILE "%batpath%cdeBackup.ps1" -env %my_target%


powershell -ExecutionPolicy BYPASS -FILE "%batpath%cdeDeploy.ps1" -source %codepath% -env %my_target%
pause