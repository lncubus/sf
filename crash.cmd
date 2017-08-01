start Server/bin/Debug/Server.exe
rem >server.log

for %%i in ( 1 2 3 4 5 6 7 8 9 ) do start ClientTest/bin/Debug/ClientTest.exe
rem >client.%%i.log

