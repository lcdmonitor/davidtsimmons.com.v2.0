#!/bin/bash
echo "Bringing up environment"
docker compose --env-file=env.dev up --build -d
echo "environment up complete"