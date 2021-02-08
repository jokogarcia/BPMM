import { NgModule } from '@angular/core';
import { Routes, RouterModule, ExtraOptions } from '@angular/router';
import {CatalogoComponent} from './components/catalogo/catalogo.component';
import {AdminHomeComponent} from './components/admin-home/admin-home.component';
import {ColeccionHomeComponent} from './components/coleccion-home/coleccion-home.component';
import {SociosHomeComponent} from './components/socios-home/socios-home.component';
import { HomeComponent } from './components/home/home.component';
import { NuestrosHomeComponent } from './components/nuestros-home/nuestros-home.component';
import { ArticleViewComponent } from './components/article-view/article-view.component';
import { SignInComponent } from './auth/sign-in/sign-in.component';
import { SignUpComponent } from './auth/sign-up/sign-up.component';
import { AuthGuard} from './auth/auth.guard'


const routes: Routes = [
  {path:'catalogo', component:CatalogoComponent },
  {path:'admin', component:AdminHomeComponent},
  {path:'coleccionlcdln', component:ColeccionHomeComponent },
  {path:'socios', component:SociosHomeComponent , canActivate:[AuthGuard]},
  {path:'nuestros', component:NuestrosHomeComponent },
  {path:'article/:handle', component:ArticleViewComponent},
  {path:'home', component:HomeComponent },
  {path:'', redirectTo:'home', pathMatch:'full'},
  {path:'login', component:SignInComponent },
  {path:'register', component:SignUpComponent },
];

const routerOptions: ExtraOptions = {
  scrollPositionRestoration: 'enabled',
  anchorScrolling: 'enabled',
  onSameUrlNavigation:"reload"
};

@NgModule({
  imports: [RouterModule.forRoot(routes, routerOptions)],
  exports: [RouterModule],
  providers:[AuthGuard]
})
export class AppRoutingModule { }
