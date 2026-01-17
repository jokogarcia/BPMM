# Use Node.js LTS version
FROM node:24-alpine
# install curl
RUN apk add --no-cache curl
# Set working directory
WORKDIR /app

# Copy package files
COPY package*.json ./

# Install dependencies
RUN npm ci 

# Copy source code
COPY . .

# Install TypeScript globally and build the app
RUN npm install -g typescript
RUN npm run build

# Create data directory for SQLite database
RUN mkdir -p data

# Expose port
EXPOSE 6000

# Set environment variable for port
ENV PORT=6000

# Start the application
CMD ["npm", "start"]