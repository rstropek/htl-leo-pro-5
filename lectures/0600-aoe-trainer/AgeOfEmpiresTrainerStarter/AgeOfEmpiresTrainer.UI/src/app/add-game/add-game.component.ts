import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { environment } from 'src/environments/environment';

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
  // Todo: Adjust the type of the following member if necessary.
  aiPlayers: any[] = [];
}
