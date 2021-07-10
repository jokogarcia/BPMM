import { Component, OnInit } from '@angular/core';
import { Output, EventEmitter } from '@angular/core';
import {MatDatepickerModule} from '@angular/material/datepicker'; 
import { MatSelectModule } from '@angular/material/select';
import { NgForm } from '@angular/forms';

import { SolicitudSocio } from '../../../models/solicitud-socio'


@Component({
  selector: 'app-socios-form',
  templateUrl: './socios-form.component.html',
  styleUrls: ['./socios-form.component.scss']
})
export class SociosFormComponent implements OnInit {

  constructor() { }
  solicitud:SolicitudSocio = {
    apellido:"garcia",
    nombre:"joaquin", 
    dniNum:"30415559", 
    dniTipo:"DNI",
    email:"jokogarcia@gmail.com",
    fnac:new Date(Date.now()),
    tipoSolicitud:"ORDINARIO",
    profesion:"ing",
    datosContactoPersonal:{
      calle:"copiapo",
      calleNumero:"258",
      telefono:"4421984",
    },
    solicitudId:0,
    estado:"N"
};
  @Output() formSubmitEvent = new EventEmitter<SolicitudSocio>();

  ngOnInit(): void {
  }
  onSubmit(form: NgForm){
    if(!form.valid){
      alert("Debe completar todos los campos obligatorios");
      return;
    }
    this.formSubmitEvent.emit(this.solicitud);
  }

}
