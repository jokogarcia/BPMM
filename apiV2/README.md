# Express API V2

This is a scaffolded ExpressJS API using Node.js and TypeScript.

## Getting Started

### Prerequisites

- Node.js
- npm

### Installation

1. Clone the repository
2. Install dependencies:
   ```bash
   npm install
   ```
3. Copy `.env.example` to `.env`:
   ```bash
   cp .env.example .env
   ```

### Scripts

- `npm run dev`: Run the server in development mode with nodemon.
- `npm run build`: Build the project for production.
- `npm start`: Run the built project.

## Project Structure

- `src/`: Source code
  - `controllers/`: Request handlers
  - `routes/`: Route definitions
  - `middleware/`: Custom middleware
  - `models/`: Data models
  - `index.ts`: Entry point
