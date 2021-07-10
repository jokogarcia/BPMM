import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { ArticleComponent } from './reusable/article/article/article.component';
import { CatalogoComponent } from './components/catalogo/catalogo.component';
import { SociosHomeComponent } from './components/socios/socios-home/socios-home.component';
import { NuestrosHomeComponent } from './components/nuestros-home/nuestros-home.component';
import { ColeccionHomeComponent } from './components/coleccion-home/coleccion-home.component';
import { AdminHomeComponent } from './components/admin-home/admin-home.component';
import { HomeComponent } from './components/home/home.component';
import { NavHeaderComponent } from './components/nav-header/nav-header.component';
import { PhotoArticleViewComponent } from './reusable/photo-article-view/photo-article-view.component';
import { ArticleViewComponent } from './components/article-view/article-view.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations'
import { HttpClientModule } from '@angular/common/http';
import { MaterialModule } from './material.module';
import { SignInComponent } from './auth/sign-in/sign-in.component';
import { SignUpComponent } from './auth/sign-up/sign-up.component'
import { FlexLayoutModule} from '@angular/flex-layout';
import { FormsModule } from '@angular/forms';
import { AuthService} from './auth/auth.service';
import { SociosFormComponent } from './components/socios/socios-form/socios-form.component';
import { SociosSolicitudComponent } from './components/socios/socios-solicitud/socios-solicitud.component';
import { SociosNotaComponent } from './components/socios/socios-nota/socios-nota.component'
import { NumbersOnlyDirective} from './reusable/directives/numbers-only.directive';
import {MAT_DATE_LOCALE } from '@angular/material/core'


@NgModule({
  declarations: [
    AppComponent,
    ArticleComponent,
    CatalogoComponent,
    SociosHomeComponent,
    NuestrosHomeComponent,
    ColeccionHomeComponent,
    AdminHomeComponent,
    HomeComponent,
    NavHeaderComponent,
    PhotoArticleViewComponent,
    ArticleViewComponent,
    SignInComponent,
    SignUpComponent,
    SociosFormComponent,
    SociosSolicitudComponent,
    SociosNotaComponent,
    NumbersOnlyDirective
    ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    HttpClientModule,
    MaterialModule,
    FlexLayoutModule,
    FormsModule
  ],
  providers: [AuthService,{provide: MAT_DATE_LOCALE, useValue: 'es-AR'}],
  bootstrap: [AppComponent]
})
export class AppModule { }
