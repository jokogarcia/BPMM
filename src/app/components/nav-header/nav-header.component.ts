import { Component, OnInit, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';
import { AuthService } from 'src/app/auth/auth.service';

@Component({
  selector: 'app-nav-header',
  templateUrl: './nav-header.component.html',
  styleUrls: ['./nav-header.component.scss']
})
export class NavHeaderComponent implements OnInit, OnDestroy{
  isAuth:boolean;
  isAuthSubscription:Subscription;
  constructor(private authService:AuthService) { 
    this.isAuth=authService.isAuth();
  }

  ngOnInit(): void {
    this.isAuthSubscription=this.authService.authChange.subscribe(x=>{this.isAuth=x});
  }
  ngOnDestroy():void{
    this.isAuthSubscription.unsubscribe();
  }
  callLogout(){
    this.authService.logout();
  }

}
