import { Injectable } from '@angular/core';
import {Article} from '../models/article'
import { HttpClient } from '@angular/common/http';
import {Observable} from 'rxjs';
import { environment } from '../../environments/environment';


const baseURL = environment.apiUrl + 'article';

@Injectable({
  providedIn: 'root'
})
export class ArticlesService {

  constructor(private httpClient: HttpClient) { }
  
  readAll(): Observable<Article[]> {
    return this.httpClient.get<Article[]>(baseURL);
  }

  read(id:string): Observable<Article> {
    return this.httpClient.get<Article>(`${baseURL}/${id}`);
  }

  create(data: Article): Observable<Article> {
    return this.httpClient.post<Article>(baseURL, data);
  }

  update(data: Article): Observable<Article> {
    return this.httpClient.put<Article>(`${baseURL}`, data);
  }

  delete(id: string): Observable<any> {
    return this.httpClient.delete(`${baseURL}/${id}`);
  }

  searchByTags(tags: string): Observable<Article[]> {
    return this.httpClient.get<Article[]>(`${baseURL}/tags/${tags}`);
  }
}
