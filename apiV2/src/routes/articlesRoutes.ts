import { Router } from 'express';
import { 
    readAllArticles, 
    readArticle, 
    createNewArticle, 
    updateExistingArticle, 
    deleteExistingArticle, 
    searchByTags 
} from '../controllers/articlesController';

const router = Router();

// GET /api/article - Get all articles
router.get('/', readAllArticles);

// GET /api/article/:id - Get article by ID
router.get('/:id', readArticle);

// POST /api/article - Create new article
router.post('/', createNewArticle);

// PUT /api/article - Update existing article
router.put('/', updateExistingArticle);

// DELETE /api/article/:id - Delete article by ID
router.delete('/:id', deleteExistingArticle);

// GET /api/article/tags/:tags - Search articles by tags
router.get('/tags/:tags', searchByTags);

export default router;