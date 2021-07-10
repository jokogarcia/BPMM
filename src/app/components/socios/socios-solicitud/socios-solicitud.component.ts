import { Component, OnInit } from '@angular/core';
import { SolicitudSocio } from '../../../models/solicitud-socio'
import { SolicitudSocioService} from '../../../services/solicitud-socio.service'


@Component({
  selector: 'app-socios-solicitud',
  templateUrl: './socios-solicitud.component.html',
  styleUrls: ['./socios-solicitud.component.scss']
})
export class SociosSolicitudComponent implements OnInit {

  constructor(private service:SolicitudSocioService) {
   }
  public status="new";
  private solicitud:SolicitudSocio;
  formSubmitted(solicitud: SolicitudSocio){
    this.solicitud=solicitud;
    this.status="nota";
  }
  ngOnInit(): void {
  }
  goBack=()=>{
    this.status="new";
  }
  enviar=()=>{
    this.status ="sending";
    this.service.create(this.solicitud)
    .subscribe(
      r=>{
        if(r.solicitudId && r.solicitudId > 0){
          this.status="exito";
        }else{
          this.status="error";
        }
      },
      error=>{
        this.status="error";
      }
      );
  }

}
