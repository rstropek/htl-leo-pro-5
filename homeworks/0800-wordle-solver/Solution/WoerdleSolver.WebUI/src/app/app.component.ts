import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  word: string = '';
  result: string = '';
  possibleWords: string[] = [];
  gameId?: number;
  error?: string;

  constructor(private httpClient: HttpClient) {
  }

  ngOnInit(): void {
    this.newGame();
  }

  newGame() {
    this.httpClient.post<{gameId: number}>(`${environment.baseApiUrl}/api/solver/game`, { })
      .subscribe(res => {
        this.gameId = res.gameId;
        this.fillPossibleWords();
      });
  }

  fillPossibleWords() {
    this.httpClient.get<string[]>(`${environment.baseApiUrl}/api/solver/game/${this.gameId}`)
      .subscribe(res => {
        this.possibleWords = res;
        this.word = this.possibleWords[0];
      })
  }

  store() {
    this.httpClient.post(`${environment.baseApiUrl}/api/solver/game/${this.gameId}`, { word: this.word, result: this.result })
      .subscribe({
        next: () => {
          this.fillPossibleWords();
          this.result = this.word = '';
          delete this.error;
        },
        error: err => { this.error = err.error.error; }
      });
  }

  reset() {
    delete this.error;
    delete this.gameId;
    this.word = this.result = '';
    this.possibleWords = [];
    this.newGame();
  }
}
