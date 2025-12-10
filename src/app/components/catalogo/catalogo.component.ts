import { Component, OnInit, ViewChild, AfterViewInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { Book } from '../../models/book'
import {BooksService} from '../../services/books.service'
import {MatSort} from '@angular/material/sort';
import {MatTableDataSource} from '@angular/material/table';

@Component({
  selector: 'app-catalogo',
  templateUrl: './catalogo.component.html',
  styleUrls: ['./catalogo.component.scss']
})
export class CatalogoComponent implements OnInit, AfterViewInit {
  categorias:string[];
  dataSource:MatTableDataSource<Book>;
  currentPage=0;
  currentPageSize=10;
  currentAuthor="";
  currentTitle="";
  selectedCategory="";
  columnsToDisplay = ['title', 'author', 'publisher'];
  totalBookCount: number;
  pageSizeOptions: number[] = [10,20,50];
  @ViewChild('paginator',{static:true}) paginator:MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  constructor(private booksService:BooksService) { 
    
  }
  ngAfterViewInit(): void {
    
  }

  ngOnInit(): void {
    this.filterUpdate();
    
    this.booksService.getCategories().subscribe(x=>{
      this.categorias = x;
      this.categorias.push("todas");
    });
    this.paginator._intl.itemsPerPageLabel="Filas por página";
    this.paginator._intl.nextPageLabel="Siguiente";
    this.paginator._intl.previousPageLabel="Anterior";
    this.paginator._intl.firstPageLabel="Primera página";
    this.paginator._intl.lastPageLabel="Última página";
    this.paginator._intl.getRangeLabel=(number,pagesize,length)=>`${number*pagesize+1} - ${(number+1)*pagesize} de ${length}`;

  }
  onSubmit(form: NgForm){
    this.currentAuthor=form.value.author;
    this.currentTitle=form.value.title;
    this.selectedCategory = form.value.section;
    this.filterUpdate();
  }
  private filterUpdate(){
    this.booksService.searchByFilters(this.currentAuthor,this.currentTitle, this.selectedCategory, this.currentPage,this.currentPageSize).subscribe(
      
      x=>{
        this.dataSource = new MatTableDataSource(x.results);
        this.dataSource.sort = this.sort;
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
