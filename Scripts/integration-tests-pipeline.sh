#!/bin/sh

set -e

echo "Waiting for the database. Sleeping for 30s."
# TODO for some reason wait-for-it.sh does not work in .NET Core SDK environment
sleep 30

echo "Executing tests"
dotnet test