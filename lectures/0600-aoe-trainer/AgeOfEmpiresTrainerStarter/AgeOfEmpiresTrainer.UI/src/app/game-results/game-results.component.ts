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
  ngOnInit(): void {
  }
}
