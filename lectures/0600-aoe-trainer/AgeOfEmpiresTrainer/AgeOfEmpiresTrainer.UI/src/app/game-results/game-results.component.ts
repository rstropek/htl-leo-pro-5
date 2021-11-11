import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

interface GameResultSummary {
  id: number,
  numberOfAIPlayers: number,
  civilizations: string,
  gameStatus: string,
}

@Component({
  selector: 'app-game-results',
  templateUrl: './game-results.component.html',
  styleUrls: [ 'game-results.component.css' ]
})
export class GameResultsComponent implements OnInit {
  numberOfAIPlayers: string = '';
  civilization: string = '';
  gameStatus: string = '';

  gameResult!: Observable<GameResultSummary[]>;

  constructor(private client: HttpClient) { }

  ngOnInit(): void {
    this.refresh();
  }

  refresh() {
    this.gameResult = this.client.get<GameResultSummary[]>(`${environment.apiBase}/api/gameResults?${this.buildQuery()}`);
  }

  buildQuery(): string {
    let filters: string[] = [];

    if (this.numberOfAIPlayers) {
      filters.push(`numberOfAIPlayers=${this.numberOfAIPlayers}`);
    }

    if (this.civilization) {
      filters.push(`civilization=${this.civilization}`);
    }

    if (this.gameStatus) {
      filters.push(`gameStatus=${this.gameStatus}`);
    }

    return filters.join('&');
  }
  
  delete(id: number) {
    this.client.delete(`${environment.apiBase}/api/gameResults/${id}`)
      .subscribe(() => this.refresh());
  }
}
