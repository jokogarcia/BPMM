import { Injectable } from '@angular/core';
import {Article} from '../models/article'
import {HttpClient} from '@angular/common/http';
import {Observable} from 'rxjs';

const baseURL ='http://localhost:57035/api/article';

@Injectable({
  providedIn: 'root'
})
export class ArticlesService {

  constructor(private httpClient: HttpClient) { }
  
  readAll(): Observable<any> {
    return this.httpClient.get(baseURL);
  }

  read(id): Observable<any> {
    return this.httpClient.get(`${baseURL}/${id}`);
  }

  create(data): Observable<any> {
    return this.httpClient.post(baseURL, data);
  }

  update(data): Observable<any> {
    return this.httpClient.put(`${baseURL}`, data);
  }

  delete(id): Observable<any> {
    return this.httpClient.delete(`${baseURL}/${id}`);
  }

  searchByTags(tags): Observable<any> {
    return this.httpClient.get(`${baseURL}/tags/${tags}`);
  }
}
