#!/bin/bash
WAIT_TIME=20s

echo creating resources in "$WAIT_TIME"
sleep "$WAIT_TIME"

if [[ ! -f /var/opt/mssql/.db_exist ]]; then
    echo "setting up database, name - [${DATABASE}]"
    /opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P "$SA_PASSWORD" -Q "CREATE DATABASE [${DATABASE}]"

    touch /var/opt/mssql/.db_exist
fi

echo Database sucessfully started!