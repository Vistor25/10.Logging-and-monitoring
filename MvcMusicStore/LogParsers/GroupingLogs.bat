"%ProgramFiles(x86)%\Log Parser 2.2\LogParser.exe" -i:CSV -o:NAT "SELECT level,Count(*) FROM *.csv group by level"
pause