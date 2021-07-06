import { Component, OnInit } from '@angular/core';
import {MatDatepickerModule} from '@angular/material/datepicker'; 
import { MatSelectModule } from '@angular/material/select';
import { NgForm } from '@angular/forms';
import { SolicitudSocio } from '../models/solicitud-socio'


@Component({
  selector: 'app-socios-form',
  templateUrl: './socios-form.component.html',
  styleUrls: ['./socios-form.component.scss']
})
export class SociosFormComponent implements OnInit {

  constructor() { }
  solicitud:SolicitudSocio = new SolicitudSocio();

  ngOnInit(): void {
  }
  onSubmit(form: NgForm){
    if(!form.valid){
      alert("Debe completar todos los campos obligatorios");
    }
    console.log(this.solicitud)
  }

}
