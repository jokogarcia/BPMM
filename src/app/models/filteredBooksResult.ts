import {Book} from '../models/book'

export interface FilteredBooksResult{
    totalCount:number;
    results:Book[];
}