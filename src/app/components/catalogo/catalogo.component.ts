import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { PageEvent } from '@angular/material/paginator';
import { Book } from '../../models/book'
import {BooksService} from '../../services/books.service'

@Component({
  selector: 'app-catalogo',
  templateUrl: './catalogo.component.html',
  styleUrls: ['./catalogo.component.scss']
})
export class CatalogoComponent implements OnInit {
  categorias:string[];
  books:Book[];
  currentPage=0;
  currentPageSize=10;
  currentAuthor="";
  currentTitle="";
  columnsToDisplay = ['title', 'author', 'publisher'];
  totalBookCount: number;
  pageSizeOptions: number[] = [10,20,50];
  constructor(private booksService:BooksService) { 
    
  }

  ngOnInit(): void {
    this.filterUpdate();
    this.booksService.getCategories().subscribe(x=>this.categorias = x);

  }
  onSubmit(form: NgForm){
    this.currentAuthor=form.value.author;
    this.currentTitle=form.value.title;
    this.filterUpdate();
  }
  private filterUpdate(){
    this.booksService.searchByFilters(this.currentAuthor,this.currentTitle,this.currentPage,this.currentPageSize).subscribe(
      
      x=>{
        this.books= x.results;
        this.totalBookCount=x.totalCount;
      }
    );
  }
  OnPageChange(pageoptions:PageEvent){
    this.currentPageSize=pageoptions.pageSize;
    this.currentPage=pageoptions.pageIndex;
    this.filterUpdate();
    
  }
}
