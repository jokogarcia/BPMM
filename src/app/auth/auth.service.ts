import { Injectable } from "@angular/core";
import { Router } from "@angular/router";
import { Subject } from "rxjs";
import { AuthData } from "./auth-data.model";
import { User } from "./user.model";
@Injectable({
    providedIn: 'root',
  })
export class AuthService{
    private user: User;
    authChange = new Subject<boolean>();

    constructor(private router:Router){}

    registerUser(authdata:AuthData){
        this.user = {
            email: authdata.email,
            userId: Math.round(Math.random()*10000).toString()
        };
        this.setAuth(true);
    }
    login(authdata:AuthData){
        this.user = {
            email: authdata.email,
            userId: Math.round(Math.random()*10000).toString()
        };
        this.setAuth(true);
    }
    logout(){
        this.setAuth(false);
    }
    getUser(){
        return {... this.user};
    }
    isAuth(){
        return this.user != null;
    }
    private setAuth(value:boolean){
        if(value){
            this.authChange.next(true);
            this.router.navigate(["/socios"]);
        }else{
            this.authChange.next(false);
            this.router.navigate(["/"]);
            this.user=null;
        }
    }
}