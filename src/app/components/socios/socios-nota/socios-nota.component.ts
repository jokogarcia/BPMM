import { Component, Input, OnChanges, OnInit, SimpleChanges } from '@angular/core';
import { SolicitudSocio } from '../../../models/solicitud-socio';

@Component({
  selector: 'app-socios-nota',
  templateUrl: './socios-nota.component.html',
  styleUrls: ['./socios-nota.component.scss']
})
export class SociosNotaComponent implements OnInit, OnChanges {

  constructor() { }
  @Input() data: SolicitudSocio;
  public solicitud:SolicitudSocio;
  public firmante = {clase : 0, edad: 0, tieneDomicilioLaboral:false, nombreCompleto:""};
  public getDateString(){
    const now = new Date(Date.now());
    const meses = ["Enero",'Febrero','Marzo','Abril','Mayo','Junio','Julio','Agosto','Septiembre','Octubre','Noviembre','Diciembre'];
    return `${now.getDate()} de ${meses[now.getMonth()]} de ${now.getFullYear()}`;
  }
  ngOnInit(): void {
  }
  ngOnChanges(changes: SimpleChanges){
    console.log(changes.data);
    this.solicitud=changes.data.currentValue;
    this.firmante.clase = this.solicitud.fnac.getFullYear();
    this.firmante.edad = this.calculateAge(this.solicitud.fnac);
    this.firmante.tieneDomicilioLaboral = !! this.solicitud.datosContactoLaboral
  }
  private calculateAge(birthdate:Date):number{
    let timeDiff = Math.abs(Date.now() - birthdate.getTime());
    let age = Math.floor((timeDiff / (1000 * 3600 * 24))/365.25);
    return age;
  }
}
