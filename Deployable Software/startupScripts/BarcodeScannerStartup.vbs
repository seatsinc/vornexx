'copy the below code and edit accordingly
Set WshShell = CreateObject("WScript.Shell") 
WshShell.Run chr(34) & "C:\BarcodeScanner\BarcodeScanner.bat" & Chr(34), 0
Set WshShell = Nothing