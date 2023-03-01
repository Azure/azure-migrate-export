rem Save working directory so that we can restore it back after building everything. This will make developers happy and then 
rem switch to the folder this script resides in. Don't assume absolute paths because on the build host and on the dev host the locations may be different.
pushd "%~dp0"

echo "##[debug] Starting Azure-Migrate-Export.sln restore"

rem Restore dependencies. Yes, you must do this.
dotnet restore "..\..\src\Azure-Migrate-Export.sln"
echo DevOps: RESTORE STEP - Starting nuget restore...
nuget restore "..\..\src\Azure-Migrate-Export.sln" -NonInteractive

rem Save exit code for nuget restore
set EX=%ERRORLEVEL%

rem Check exit code and exit with non-zero exit code so that build will fail.
if "%EX%" neq "0" (
    popd
    echo "Failed to restore packages correctly."
	exit /b %EX%
)

echo DevOps: RESTORE STEP - Done...

echo "##[debug] Finished Azure-Migrate-Export.sln restore"