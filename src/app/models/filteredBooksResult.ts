import {Book} from '../models/book'

export interface FilteredBooksResult{
    success: boolean;
    count: number;
    data: Book[];
    pagination: {
        pageNumber: number;
        pageSize: number;
        hasMore: boolean;
    };
}