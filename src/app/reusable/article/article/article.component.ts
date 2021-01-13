import { Component, OnInit, SecurityContext } from '@angular/core';
import {Article} from '../../../models/article'
import {ArticlesService} from '../../../services/articles.service'
import { DomSanitizer } from '@angular/platform-browser'


@Component({
  selector: 'app-article',
  templateUrl: './article.component.html',
  styleUrls: ['./article.component.scss']
})
export class ArticleComponent implements OnInit {
  public article:Article;
  private articleHandle:string;
  content: any;
  constructor(private articlesService:ArticlesService, 
    private sanitized: DomSanitizer) {
   }
  

  ngOnInit(): void {
    this.articleHandle="validArticleIsValid";
    this.article = this.articlesService.getArticle(this.articleHandle);
    this.content = this.sanitized.sanitize(SecurityContext.HTML, this.article.htmlContent);
  }

}
