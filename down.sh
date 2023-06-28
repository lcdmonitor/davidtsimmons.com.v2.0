#!/bin/bash
echo "Bringing down environment"
docker compose --env-file=env.dev down
echo "environment shut down complete"