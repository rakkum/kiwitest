import { Inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Book, BookHistory, BookSearch, PagedResult } from '../models';
import { Observable } from 'rxjs';

@Injectable()
export class BookService {

  constructor(
    private http: HttpClient,
    @Inject('BASE_URL') private baseUrl: string) {
  }

  getList(skip: number, take: number, sortColumn: string, sortDir: string, filter: BookSearch): Observable<PagedResult<Book>> {

    const params = new URLSearchParams();
    for (const field of Object.keys(filter)) {
      params.set(field, filter[field]);
    }

    const url = `${this.baseUrl}books?skip=${skip}&take=${take}&sortColumn=${sortColumn}&sortDir=${sortDir}&${params.toString()}`;
    return this.http.get<PagedResult<Book>>(url);
  }

  update(book: Book): Observable<Book> {
    const url = `${this.baseUrl}books?id=${book.id}`;
    return this.http.put<Book>(url, book);
  }

  getHistory(id: number): Observable<BookHistory[]> {
    const url = `${this.baseUrl}books/history?id=${id}`;
    return this.http.get<BookHistory[]>(url);
  }
}
