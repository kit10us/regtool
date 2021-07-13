Visual Studio's commandline compiler scripts (batch files) rely on reg.exe to query setup information, such as install directories. It is only read-only, however, reg.exe might not be enabled for your accessibly users in Windows (controlled via IT). To get around this issue, we have RegTool.exe. It supports the reg.exe query operation, and returns equivalent results.

Within this directory are copies of the VS batch files modified in a way to use regtool.exe instead of reg.exe. Please use as a reference, as your needs may change. It is best to dif these changes against your actual batch files, and integrate manually.

The following are the locations within Visual Studio, on a particular install, so these locations might be different for you:

vcvarsall.bat			C:\Program Files (x86)\Microsoft Visual Studio 11.0\VC\vcvarsall.bat
bin_amd_vcvars64.bat	C:\Program Files (x86)\Microsoft Visual Studio 11.0\VC\bin\amd64\vcvars64.bat
