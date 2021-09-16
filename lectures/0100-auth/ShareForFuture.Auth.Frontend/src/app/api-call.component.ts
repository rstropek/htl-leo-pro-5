import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, concatMap, tap } from 'rxjs/operators';
import { Observable, of, zip } from 'rxjs';

import { AuthService } from '@auth0/auth0-angular';

@Component({
  selector: 'app-api-call',
  template: `
    <ng-container *ngIf="!(auth.isAuthenticated$ | async)">
      Not authenticated
    </ng-container>
    <div *ngIf="loading">
      Loading...
    </div>
    <div *ngIf="result">
      <pre>{{ result | async | json }}</pre>
    </div>
  `,
  styles: [
  ]
})
export class ApiCallComponent implements OnInit {
  result?: Observable<any>;
  loading = false;

  constructor(public auth: AuthService, private http: HttpClient) {}

  ngOnInit(): void {
    this.result = this.auth.isAuthenticated$
    .pipe(
      tap(() => this.loading = true),
      concatMap(() => 
        zip(
          this.http.get(encodeURI('https://localhost:5001/public')).pipe(catchError(err => of(err))),
          this.http.get(encodeURI('https://localhost:5001/private')).pipe(catchError(err => of(err))),
          this.http.get(encodeURI('https://localhost:5001/admin')).pipe(catchError(err => of(err))),
        )
      ),
      tap(() => this.loading = false)
    );
  }
}
