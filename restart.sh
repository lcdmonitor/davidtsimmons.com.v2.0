#!/bin/bash
echo "restarting environment"

if [ "$1" = 'prod' ]
then
    ENV_FILE=env.prod
    echo Production environment Restart
else
    ENV_FILE=env.prod
    echo Local Dev environment Restart
fi


docker compose --env-file=$ENV_FILE restart
echo "environment restart complete"