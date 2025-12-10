import express, { Request, Response } from 'express';
import dotenv from 'dotenv';
import helmet from 'helmet';
import cors from 'cors';

import healthRoutes from './routes/healthRoutes';
import librosRoutes from './routes/librosRoutes';
import articlesRoutes from './routes/articlesRoutes';
import { errorHandler } from './middleware/errorHandler';
import { notFoundHandler } from './middleware/notFoundHandler';
import  './data';

dotenv.config();

const app = express();
const port = process.env.PORT || 3000;

app.use(helmet());
app.use(cors());
app.use(express.json());



app.use('/health', healthRoutes);
app.use('/api/book', librosRoutes);
app.use('/api/article', articlesRoutes);

app.get('/', (req: Request, res: Response) => {
    res.send('Express + TypeScript Server');
});

app.use(notFoundHandler);
app.use(errorHandler);


app.listen(port, () => {
    console.log(`[server]: Server is running at http://localhost:${port}`);
});
