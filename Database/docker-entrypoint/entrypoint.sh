#!/bin/sh
./wait-for-it.sh -t 30 mysql:3306 --strict -- dotnet Database.dll