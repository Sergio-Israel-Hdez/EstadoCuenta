#!/bin/bash
# Esperar hasta que SQL Server esté listo
echo "Esperando a que SQL Server esté listo..."
/opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P "$SA_PASSWORD" -d master -i /scripts/script.sql

# Ejecutar el comando para iniciar SQL Server
/opt/mssql/bin/sqlservr
