#!/bin/bash
set -e

# Build the Docker image
echo "Building Docker image..."
docker build -t bpmm .

# Create a temporary container to extract the built files
echo "Creating temporary container to extract built files..."
CONTAINER_ID=$(docker create bpmm)
rm -rf ./dist
# Create dist directory on host if it doesn't exist
mkdir -p ./dist

# Copy the built application from container to host
echo "Copying built files from container to host..."
docker cp $CONTAINER_ID:/app/dist/bibmamo/browser ./dist/bibmamo

# Clean up the temporary container
echo "Cleaning up temporary container..."
docker rm $CONTAINER_ID

echo "Syncing files to server..."
rsync -avz --delete ./dist/bibmamo/ joaquin@irazu.com.ar:/www/bpmm/