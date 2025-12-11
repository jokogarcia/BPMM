#!/bin/sh
set -ex
echo Updating and Uploading database
REMOTE_USER=joaquin
REMOTE_HOST=irazu.com.ar
REMOTE_LOCATION=repos/BPMM/apiV2/data/libros.db
rm libros.db
npm run dev

rsync -avz --progress libros.db $REMOTE_USER@$REMOTE_HOST:$REMOTE_LOCATION
ssh $REMOTE_USER@$REMOTE_HOST "cd repos/BPMM/apiV2 && docker compose down && docker compose up -d"
echo Database uploaded successfully