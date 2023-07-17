#!/bin/bash
echo "Bringing up environment"

if [ "$1" = 'prod' ]
then
    ENV_FILE=env.prod
    echo Production environment Start
else
    ENV_FILE=env.prod
    echo Local Dev environment Start
fi

docker compose --env-file=$ENV_FILE up --build -d
echo "environment up complete"