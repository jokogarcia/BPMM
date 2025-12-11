#!/bin/bash
set -ex

cd ../dataImporter
rm libros.db
npm run dev

ssh joaquin@irazu.com.ar "cd ~/repos/BPMM && git checkout main && git fetch && git pull"
rsync -av --progress libros.db joaquin@irazu.com.ar:~/repos/BPMM/apiV2/data/libros.db

ssh joaquin@irazu.com.ar "cd ~/repos/BPMM/apiV2 && docker compose down && docker compose up -d --build"