import { Injectable } from '@angular/core';
import {SolicitudSocio} from '../models/solicitud-socio'
import {HttpClient} from '@angular/common/http';
import {Observable} from 'rxjs';
import { environment } from '../../environments/environment';


const baseURL = environment.apiUrl + 'SolicitudInscripcionSocio';

@Injectable({
  providedIn: 'root'
})
export class SolicitudSocioService {

  constructor(private httpClient: HttpClient) { }
  
  create(data:SolicitudSocio): Observable<any> {
    var j = JSON.stringify(data);
    return this.httpClient.post(baseURL, data);
  }

}
