import { Component, OnInit, Input } from '@angular/core';
import { Article } from '../../models/article';
@Component({
  selector: 'app-photo-article-view',
  templateUrl: './photo-article-view.component.html',
  styleUrls: ['./photo-article-view.component.scss']
})
export class PhotoArticleViewComponent implements OnInit {

  @Input() article!:Article;
  constructor() { }

  ngOnInit(): void {
    console.log(this.article.title);
  }

}
