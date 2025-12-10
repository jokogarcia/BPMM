import { Request, Response } from 'express';
import { 
    Article, 
    getAllArticles, 
    getArticleById, 
    createArticle, 
    updateArticle, 
    deleteArticle, 
    searchArticlesByTags 
} from '../articlesData';

export const readAllArticles = async (req: Request, res: Response) => {
    try {
        const articles: Article[] = await getAllArticles();
        
        res.status(200).json(articles);
    } catch (error) {
        console.error('Error fetching all articles:', error);
        res.status(500).json({ 
            error: 'Internal server error while fetching articles' 
        });
    }
};

export const readArticle = async (req: Request, res: Response) => {
    try {
        const { id } = req.params;
        
        if (!id) {
            return res.status(400).json({ 
                error: 'Missing article ID parameter' 
            });
        }
        
        const article: Article | null = await getArticleById(id);
        
        if (!article) {
            return res.status(404).json({ 
                error: 'Article not found' 
            });
        }
        
        res.status(200).json(article);
    } catch (error) {
        console.error('Error fetching article:', error);
        res.status(500).json({ 
            error: 'Internal server error while fetching article' 
        });
    }
};

export const createNewArticle = async (req: Request, res: Response) => {
    try {
        const articleData = req.body;
        
        // Validate required fields (handle and templateId are optional since they're not in DB)
        if (!articleData.title || !articleData.htmlContent || 
            !articleData.author || !articleData.subtitle) {
            return res.status(400).json({ 
                error: 'Missing required fields: title, htmlContent, author, subtitle' 
            });
        }
        
        const newArticle: Article = await createArticle(articleData);
        
        res.status(201).json(newArticle);
    } catch (error) {
        console.error('Error creating article:', error);
        if (error instanceof Error && error.message.includes('UNIQUE constraint failed')) {
            res.status(409).json({ 
                error: 'Article with this handle already exists' 
            });
        } else {
            res.status(500).json({ 
                error: 'Internal server error while creating article' 
            });
        }
    }
};

export const updateExistingArticle = async (req: Request, res: Response) => {
    try {
        const articleData = req.body;
        
        // Validate required fields (handle and templateId are optional since they're not in DB)
        if (!articleData.articleId || !articleData.title || 
            !articleData.htmlContent || !articleData.author || !articleData.subtitle) {
            return res.status(400).json({ 
                error: 'Missing required fields: articleId, title, htmlContent, author, subtitle' 
            });
        }
        
        const updatedArticle: Article = await updateArticle(articleData);
        
        res.status(200).json(updatedArticle);
    } catch (error) {
        console.error('Error updating article:', error);
        if (error instanceof Error && error.message === 'Article not found') {
            res.status(404).json({ 
                error: 'Article not found' 
            });
        } else {
            res.status(500).json({ 
                error: 'Internal server error while updating article' 
            });
        }
    }
};

export const deleteExistingArticle = async (req: Request, res: Response) => {
    try {
        const { id } = req.params;
        
        if (!id) {
            return res.status(400).json({ 
                error: 'Missing article ID parameter' 
            });
        }
        
        await deleteArticle(id);
        
        res.status(200).json({ 
            success: true, 
            message: 'Article deleted successfully' 
        });
    } catch (error) {
        console.error('Error deleting article:', error);
        if (error instanceof Error && error.message === 'Article not found') {
            res.status(404).json({ 
                error: 'Article not found' 
            });
        } else {
            res.status(500).json({ 
                error: 'Internal server error while deleting article' 
            });
        }
    }
};

export const searchByTags = async (req: Request, res: Response) => {
    try {
        const { tags } = req.params;
        
        if (!tags) {
            return res.status(400).json({ 
                error: 'Missing tags parameter' 
            });
        }
        
        const articles: Article[] = await searchArticlesByTags(tags);
        
        res.status(200).json(articles);
    } catch (error) {
        console.error('Error searching articles by tags:', error);
        res.status(500).json({ 
            error: 'Internal server error while searching articles' 
        });
    }
};