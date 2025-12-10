import { Router } from 'express';
import { searchByFilters, getCategories } from '../controllers/librosController';

const router = Router();

// GET /api/libros/filtered?author=value&title=value&categories=value&pageNumber=0&pageSize=25
router.get('/filtered', searchByFilters);

// GET /api/libros/categories
router.get('/categories', getCategories);

export default router;