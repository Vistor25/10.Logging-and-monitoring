"%ProgramFiles(x86)%\Log Parser 2.2\LogParser.exe" -i:CSV -o:NAT "SELECT distinct message FROM *.csv where level='Error'"
pause