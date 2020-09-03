import { Injectable } from '@angular/core';
import { HttpErrorResponse, HttpEvent, HttpHandler, HttpRequest } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Injectable()
export class ErrorHandlingInterceptor {

  intercept(req: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    return next.handle(req).pipe(catchError(err => this.handleError(err)));
  }

  private handleError(errorResponse: HttpErrorResponse): Observable<never> {
    // TODO: redirect to error page
    // TODO: log
    return throwError(errorResponse);
  }
}
