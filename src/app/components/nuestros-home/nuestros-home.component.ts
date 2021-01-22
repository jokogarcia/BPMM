import { Component, OnInit } from '@angular/core';
import { Article } from 'src/app/models/article';
import { ArticlesService } from 'src/app/services/articles.service';

@Component({
  selector: 'app-nuestros-home',
  templateUrl: './nuestros-home.component.html',
  styleUrls: ['./nuestros-home.component.scss']
})
export class NuestrosHomeComponent implements OnInit {

  constructor( private articlesService:ArticlesService) { }
  articles:Article[];


  ngOnInit(): void {
    this.getArticles();
  }
  getArticles(){
    this.articlesService.searchByTags("nuestros")
    .subscribe(
      articles =>{
        this.articles=articles;
      }
    )
  }

}
