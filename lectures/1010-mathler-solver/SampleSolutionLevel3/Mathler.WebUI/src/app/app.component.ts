import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  expected?: number;
  formula: string = '';
  result: string = '';
  guess: string = '';
  gameId?: number;
  error?: string;

  constructor(private httpClient: HttpClient) {
  }

  get expectedIsSet(): boolean {
    return this.gameId !== undefined;
  }

  set() {
    if (this.expected !== undefined) {
      this.httpClient.post<{gameId: number}>(`${environment.baseApiUrl}/api/solver/game`, { expectedResult: this.expected })
        .subscribe(res => {
          this.gameId = res.gameId;
          this.getGuess();
        });
    }
  }

  getGuess() {
    if (this.expectedIsSet) {
      this.httpClient.get<{ formula: string}>(`${environment.baseApiUrl}/api/solver/game/${this.gameId}`)
        .subscribe(res => {
          this.guess = res.formula;
          this.formula = res.formula;
        })
    }
  }

  store() {
    if (this.expectedIsSet) {
      this.httpClient.post(`${environment.baseApiUrl}/api/solver/game/${this.gameId}`, { formula: this.formula, result: this.result })
        .subscribe({
          next: () => {
            this.getGuess();
            this.result = '';
          },
          error: err => { this.error = err.error.error; }
        });
    }
  }

  reset() {
    delete this.error;
    delete this.expected;
    this.formula = '';
    this.result = '';
    this.guess = '';
    delete this.gameId;
  }
}
