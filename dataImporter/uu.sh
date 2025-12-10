#!/bin/sh
echo Updating and Uploading database
REMOTE_USER=joaquin
REMOTE_HOST=irazu.com.ar
REMOTE_LOCATION=/www/bpmm/data/libros.db
rm libros.db
npm run dev

rsync -avz --progress libros.db $REMOTE_USER@$REMOTE_HOST:$REMOTE_LOCATION
echo Database uploaded successfully