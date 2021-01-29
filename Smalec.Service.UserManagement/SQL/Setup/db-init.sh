# koniec linii musi by� LF
sleep 20s

echo "running set up script"

for sql_file in /setupScripts/Migrations/*.sql; do
	/opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P Admin123#@= -d master -i $sql_file
done