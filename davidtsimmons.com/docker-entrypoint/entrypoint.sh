#!/bin/sh
./wait-for-it.sh -t 30 dbup:9001 --strict -- dotnet davidtsimmons.com.dll