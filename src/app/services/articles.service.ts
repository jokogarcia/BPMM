import { Injectable } from '@angular/core';
import {Article} from '../models/article'

@Injectable({
  providedIn: 'root'
})
export class ArticlesService {

  constructor() { }
  articlesMockRepos:Article[] = [
    {
      handle:"01",
      title:"Arturo Marasso",
      htmlContent:"lorem ipsum",
      tags:"nuestros",
      mainImageUrl:"amarasso.gif",
    },
    {
      handle:"02",
      title:"Rosa Bazán de Cámara",
      htmlContent:"lorem ipsum doloor ipso. Francamente estoy inventándome cualquier cosa.",
      tags:"nuestros",
      mainImageUrl:"rosa.gif",
  },
  {
    handle:"03",
    title:"La buena memoria",
    htmlContent:"lorem ipsum",
    tags:"coleccion",
    mainImageUrl:"tapa1.jpg",
  },
  {
    handle:"04",
    title:"A Valid article", 
    htmlContent:"I am some <b>content</b>.",
    mainImageUrl:"naranjo.jpg",
  }
  ]
  getArticle(handle:string):Article{
    const result = this.articlesMockRepos.find(x=>x.handle===handle)
    if(result){
      return result;
    }
    else{
      throw new Error("404");
    }
  }
  getArticlesWithTag(tags:string):Article[]{
    const tagsArray = tags.split(",");
    const result:Article[] = new Array();
    return this.articlesMockRepos.filter(x=>(x.tags && x.tags.split(",").some(x=>tagsArray.indexOf(x)>=0)));
  }
}
