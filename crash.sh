mono Server/bin/Debug/Server.exe >server.log &

for i in 1 2 3 4 5 6 7 8 9
do
    mono ClientTest/bin/Debug/ClientTest.exe >client.$i.log &
done

