import { AfterViewInit, Component, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { animate, state, style, transition, trigger } from '@angular/animations';
import { MatSort } from '@angular/material/sort';
import { MatDialog } from '@angular/material/dialog';
import { merge, of, Subject } from 'rxjs';
import { catchError, map, startWith, switchMap } from 'rxjs/operators';
import { BookSearch, Book, BookHistory } from '../shared/models';
import { BookDialogComponent } from '../book-dialog/book-dialog.component';
import { BookService } from '../shared/services/book.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'],
  animations: [
    trigger('detailExpand', [
      state('collapsed', style({ height: '0px', minHeight: '0' })),
      state('expanded', style({ height: '*' })),
      transition('expanded <=> collapsed', animate('225ms cubic-bezier(0.4, 0.0, 0.2, 1)')),
    ]),
  ],
})
export class HomeComponent implements AfterViewInit {

  private readonly filterSubject: Subject<void> = new Subject<void>();

  displayedColumns: string[] = ['id', 'title', 'description', 'publishDate', 'authors', 'edit'];

  totalItems: number;
  data: Book[];
  expandedRow: Book = null;
  history: BookHistory[];
  searchModel: BookSearch = new BookSearch();

  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;

  constructor(
    private bookService: BookService,
    public dialog: MatDialog) {
  }

  ngAfterViewInit(): void {

    this.sort.sortChange.subscribe(() => this.paginator.pageIndex = 0);
    this.filterSubject.subscribe(() => this.paginator.pageIndex = 0);

    merge(this.sort.sortChange, this.paginator.page, this.filterSubject.asObservable())
      .pipe(
        startWith({}),
        switchMap(() => {

          const take = this.paginator.pageSize;
          const skip = take * this.paginator.pageIndex;

          return this.bookService.getList(skip, take, this.sort.active, this.sort.direction, this.searchModel);
        }),
        map(data => {
          this.totalItems = data.itemCount;
          return data.items;
        }),
        catchError(() => {
          return of([]);
        })
      ).subscribe(data => this.data = data);
  }

  applyFilter() {
    this.filterSubject.next();
  }

  clearFilter() {
    this.searchModel = new BookSearch();
  }

  getHistory(book: Book) {

    if (this.expandedRow === book) {
      this.expandedRow = null;
    } else {
      this.bookService.getHistory(book.id).subscribe(hist => {
        this.expandedRow = book;
        this.history = hist;
      });
    }
  }

  openDialog(book: Book) {

    if (this.expandedRow === book) {
      this.expandedRow = null;
    }

    const dialogRef = this.dialog.open(BookDialogComponent, {
      width: '600px',
      data: {
        id: book.id,
        title: book.title,
        description: book.description
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.bookService.update(result).subscribe(data => {
          const entity = this.data.find(b => b.id === data.id);
          entity.title = data.title;
          entity.description = data.description;
        });
      }
    });
  }
}
