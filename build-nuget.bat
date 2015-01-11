@echo off

SET PATH=.\3rd\NuGet
SET EnableNuGetPackageRestore=true

NuGet Update -self
NuGet Pack .\3rd\NuGet\DrivenAz.nuspec -OutputDirectory .\Release

ECHO.
ECHO.

SET /p PUBLISH="Publish to NuGet (y/n): " %=%

ECHO.
ECHO.

IF "%PUBLISH%" == "y" (
	ECHO Publishing to Nuget...
	NuGet Push .\release\DrivenAz.1.0.nupkg
	ECHO Publish complete.
) ELSE (
	ECHO Publish cancelled.
)

ECHO.
ECHO.

PAUSE