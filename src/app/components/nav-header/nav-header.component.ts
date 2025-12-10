import { Component, OnInit, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';
import { AuthService } from 'src/app/auth/auth.service';

@Component({
    selector: 'app-nav-header',
    templateUrl: './nav-header.component.html',
    styleUrls: ['./nav-header.component.scss'],
    standalone: false
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
  toggleMenu(){
    const navSmall = document.getElementById('navSmall');
    if (navSmall) {
      if (navSmall.classList.contains('w3-show')) {
        navSmall.classList.remove('w3-show');
      } else {
        navSmall.classList.add('w3-show');
      }
    }
  }

}
