import { Component, OnInit } from '@angular/core';
import { Article } from 'src/app/models/article';
import { ArticlesService } from 'src/app/services/articles.service';

@Component({
  selector: 'app-coleccion-home',
  templateUrl: './coleccion-home.component.html',
  styleUrls: ['./coleccion-home.component.scss']
})
export class ColeccionHomeComponent implements OnInit {

  constructor( private articlesService:ArticlesService) { }
  articles:Article[];

  ngOnInit(): void {
    this.getArticles();
  }
  getArticles(){
    this.articlesService.searchByTags("coleccion")
    .subscribe(
      articles =>{
        this.articles=articles;
      }
    )
  }

}
