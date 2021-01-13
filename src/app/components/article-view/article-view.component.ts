import { Component, OnInit, SecurityContext } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Article } from 'src/app/models/article';
import { ArticlesService } from 'src/app/services/articles.service';
import { DomSanitizer } from '@angular/platform-browser'


@Component({
  selector: 'app-article-view',
  templateUrl: './article-view.component.html',
  styleUrls: ['./article-view.component.scss']
})
export class ArticleViewComponent implements OnInit {
  public article:Article;
  public content:any;

  constructor(private activatedroute:ActivatedRoute, 
    private articlesService:ArticlesService,
    private sanitized: DomSanitizer) { }
  
  ngOnInit(): void {
    this.article = this.articlesService.getArticle(this.activatedroute.snapshot.paramMap.get("handle"));
    this.content = this.sanitized.sanitize(SecurityContext.HTML, this.article.htmlContent);
  }

}
