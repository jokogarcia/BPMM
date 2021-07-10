export class SolicitudSocio{
    solicitudId:number;
    apellido:string;
    nombre:string;
    email:string;
    dniTipo:string;
    dniNum:string;
    fnac:Date;
    datosContactoPersonal:DatosDeContacto;
    datosContactoLaboral?:DatosDeContacto;
    tipoSolicitud:string;
    profesion?:string;
    estado:string;
}
export class DatosDeContacto{
    calle:string
    calleNumero:string
    barrio?:string;
    piso?:string;
    departamento?:string;
    telefono?:string;
    
}