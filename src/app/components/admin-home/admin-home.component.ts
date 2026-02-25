import { Component, OnInit } from '@angular/core';
import { Book } from '../../models/book';

const blankBook: Book = {
  titulo: '',
  autor: '',
  isbn: '',
  categoria: '',
  editorial: '',
  edicion: '',
  paginas: 0,
  ejemplares:1,
  observaciones: '',
  inventario: ""
};

@Component({
    selector: 'app-admin-home',
    templateUrl: './admin-home.component.html',
    styleUrls: ['./admin-home.component.scss'],
    standalone: false
})
export class AdminHomeComponent implements OnInit {

  constructor() { }

  ngOnInit(): void {
  }

  newBook: Book = {...blankBook};

  addBook() {
    console.log('Libro agregado:', this.newBook);
    // Aquí puedes agregar la lógica para enviar el libro al servidor o actualizar la lista de libros
    this.newBook = {...blankBook}; // Reinicia el formulario
  }
  scanBarCode() {
    console.log('Escaneando código de barras...');
    const isbn = "978-1-50672-171-2"; // Aquí deberías obtener el ISBN del escáner de código de barras
    this.fetchBookInfo(isbn);
    // Aquí puedes agregar la lógica para escanear el código de barras y obtener la información del libro
  }
  fetchBookInfo(isbn: string) {
    const url = `https://www.googleapis.com/books/v1/volumes?q=isbn:${isbn}`;
    fetch(url)
      .then(response => response.json())
      .then(data => {
        if (data.totalItems > 0) {
          const bookInfo = data.items[0].volumeInfo;
          this.newBook.titulo = bookInfo.title || '';
          this.newBook.autor = bookInfo.authors ? bookInfo.authors.join(', ') : '';
          this.newBook.isbn = isbn;
          this.newBook.categoria = bookInfo.categories ? bookInfo.categories[0] : '';
          this.newBook.editorial = bookInfo.publisher || '';
          this.newBook.edicion = bookInfo.publishedDate || '';
          this.newBook.paginas = bookInfo.pageCount || 0;
        } else {
          alert('No se encontró información para el ISBN proporcionado.');
        }
      })
      .catch(error => {
        alert('Error al obtener información del libro. Por favor, inténtelo de nuevo más tarde.');
        console.error('Error al obtener información del libro:', error);
      });

  }

}
