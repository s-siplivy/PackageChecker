@ECHO OFF
SET fakeVersion=4.64.12
.nuget\nuget.exe install FAKE -Version %fakeVersion% -OutputDirectory packages

rmdir packages\FAKE
mklink /J packages\FAKE packages\FAKE.%fakeVersion%

packages\FAKE\tools\FAKE.exe build.fsx
