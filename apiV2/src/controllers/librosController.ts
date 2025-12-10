import { Request, Response } from 'express';
import { search, LibroUnificado, getCategorias } from '../data';

export const searchByFilters = async (req: Request, res: Response) => {
    try {
        const { author, title, categories, pageNumber = '0', pageSize = '25' } = req.query;
        
        const page = parseInt(pageNumber as string, 10);
        const size = parseInt(pageSize as string, 10);
        const skip = page * size;
        if (isNaN(page) || isNaN(size) || page < 0 || size <= 0 || size > 100) {
            console.log('Invalid pagination parameters:', { pageNumber, pageSize });
            return res.status(400).json({ 
                error: 'Invalid pagination parameters. pageNumber must be >= 0, pageSize must be 1-100' 
            });
        }
        
        const libros: LibroUnificado[] = await search(
            author as string || '',
            title as string || '',
            categories as string || '',
            skip,
            size
        );
        
        res.status(200).json({
            success: true,
            count: libros.length,
            data: libros,
            pagination: {
                pageNumber: page,
                pageSize: size,
                hasMore: libros.length === size
            }
        });
    } catch (error) {
        console.error('Error in filtered search:', error);
        res.status(500).json({ 
            error: 'Internal server error while searching books' 
        });
    }
};

export const getCategories = async (req: Request, res: Response) => {
    try {
        const categorias: string[] = await getCategorias();
        
        res.status(200).json({
            success: true,
            data: categorias.filter(cat => cat && cat.trim() !== '')
        });
    } catch (error) {
        console.error('Error fetching categories:', error);
        res.status(500).json({ 
            error: 'Internal server error while fetching categories' 
        });
    }
};

