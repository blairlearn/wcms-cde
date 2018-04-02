@echo off
rem Get paths into variables
rem Batpath gets the path to this bat file.
set batpath=%~dp0

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
if errorlevel 1 (
	echo An error has occured.
	exit /b 1
)

powershell -ExecutionPolicy BYPASS -FILE "%batpath%cdeDeploy.ps1" -source "%batpath%" -env %my_target%
if errorlevel 1 (
	echo An error has occured.
	exit /b 1
)
