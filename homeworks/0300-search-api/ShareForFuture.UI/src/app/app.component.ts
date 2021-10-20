import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { FormControl } from '@angular/forms';
import { Observable } from 'rxjs';

import { debounceTime, startWith, switchMap } from 'rxjs/operators';

@Component({
  selector: 'app-root',
  template: `
    <h1>Filter Demo</h1>

    <div>
      <label>Filter:</label>
      <input type="text" [formControl]="filter" />
    </div>
    
    <div *ngIf="result" >
      <ul *ngFor="let r of result | async">
        <li>{{ r | json }}</li>
      </ul>
    </div>
  `,
  styles: []
})
export class AppComponent {
  filter = new FormControl('');
  result?: Observable<any[]>;

  constructor(private httpClient: HttpClient) {
    this.result = this.filter.valueChanges
      .pipe(
        startWith(''),
        debounceTime(200),
        switchMap(f => this.httpClient.get<any[]>(`https://localhost:7070/multi-word?filter=${f}`))
      );
  }
}
