#!/bin/bash

cd ../dataImporter
rm libros.db
result=$(npm run dev)
if [ $? -ne 0 ]; then
  echo "Data import failed. Aborting deployment."
  exit 1
fi
echo "$result" | grep "Database created successfully. Uploading to server..."
result=$(rsync -av --progress libros.db joaquin@irazu.com.ar:~/repos/BPMM/apiV2/data/libros.db)
if [ $? -ne 0 ]; then
  echo "Data upload failed. Aborting deployment."
  exit 1
fi


ssh joaquin@irazu.com.ar "cd ~/repos/BPMM && git checkout main && git fetch && git pull && docker-compose down && docker-compose up -d --build"