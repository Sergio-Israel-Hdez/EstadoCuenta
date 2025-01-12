#!/bin/bash
# Esperar hasta que SQL Server est� listo
echo "Esperando a que SQL Server est� listo..."
/opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P "$SA_PASSWORD" -d master -i /scripts/script.sql

# Ejecutar el comando para iniciar SQL Server
/opt/mssql/bin/sqlservr
