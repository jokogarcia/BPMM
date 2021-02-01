import { Injectable } from '@angular/core';
import {Book} from '../models/book'
import {HttpClient} from '@angular/common/http';
import {Observable} from 'rxjs';
import {FilteredBooksResult} from '../models/filteredBooksResult';
import { environment } from '../../environments/environment';


const baseURL = environment.apiUrl + 'book';

@Injectable({
  providedIn: 'root'
})
export class BooksService {

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
  searchByFilters(author:string, title:string, categories:string, pageNumber:number=0, pageSize:number=25): Observable<FilteredBooksResult> {
      let url =`${baseURL}/filtered?pageNumber=${pageNumber}&pageSize=${pageSize}`;
    if(author){
        url+=`&author=${author}`;
    }
    if(title){
        url+=`&title=${title}`;
    }
    if(categories && categories !== "todas" ){
      url+=`&categories=${categories}`;
    }
    var result =  this.httpClient.get(url) as Observable<FilteredBooksResult>;
    return result;
  }
  getCategories():Observable<string[]>{
      return this.httpClient.get(`${baseURL}/categories`) as Observable<string[]>;
  }
}
