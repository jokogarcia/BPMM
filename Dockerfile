FROM node:22-alpine

WORKDIR /app

COPY package.json .
COPY package-lock.json .

# Install dependencies including devDependencies for building
RUN npm ci

COPY . .

# Build the application
RUN npm run build

# Install http-server to serve static files
RUN npm install -g http-server

EXPOSE 4200
CMD ["http-server", "dist/bibmamo/browser", "-p", "4200"]