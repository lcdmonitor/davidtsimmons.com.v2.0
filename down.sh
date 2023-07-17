#!/bin/bash
echo "Bringing down environment"

if [ "$1" = 'prod' ]
then
    ENV_FILE=env.prod
    echo Production environment Stop
else
    ENV_FILE=env.prod
    echo Local Dev environment Stop
fi

docker compose --env-file=$ENV_FILE down
echo "environment shut down complete"