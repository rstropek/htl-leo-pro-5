import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { environment } from 'src/environments/environment';

interface GameAI
{
  civilization: string;
  level: string;
  defeated: boolean;
}

interface AIPlayer {
  civilization: number;
  difficultyLevel: number;
  defeated: boolean;
}

interface GameResult {
  numberOfAIPlayers: number;
  gameStatus: number;
  civilizationLevel: number;
  notes?: string;
  aiPlayers: AIPlayer[];
}

@Component({
  selector: 'app-add-game',
  templateUrl: './add-game.component.html',
  styleUrls: [ 'add-game.component.css' ]
})
export class AddGameComponent {
  civilizationLevel = '1';
  gameStatus: string = '1';
  notes: string = '';
  aiPlayers: GameAI[] = [
    { civilization: '0', level: '0', defeated: true }
  ];

  constructor(private client: HttpClient, private router: Router) { }

  addPlayer() {
    this.aiPlayers.push({ civilization: '0', level: '0', defeated: true });
  }

  delete(i: number) {
    this.aiPlayers.splice(i, 1);
  }

  save() {
    const game: GameResult = {
      numberOfAIPlayers: this.aiPlayers.length,
      civilizationLevel: parseInt(this.civilizationLevel),
      gameStatus: parseInt(this.gameStatus),
      notes: this.notes,
      aiPlayers: this.aiPlayers.map(p => {
        return {
          civilization: parseInt(p.civilization),
          difficultyLevel: parseInt(p.level),
          defeated: p.defeated,
        };
      })
    };
    this.client.post(`${environment.apiBase}/api/gameResults`, game)
      .subscribe(() => this.router.navigate(['game-results']));
  }
}
