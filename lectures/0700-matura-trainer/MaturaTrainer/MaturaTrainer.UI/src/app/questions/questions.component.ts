import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { QuestionSummaryDto } from '../model';

interface GameResultSummary {
  id: number,
  numberOfAIPlayers: number,
  civilizations: string,
  gameStatus: string,
}

@Component({
  selector: 'app-questions',
  templateUrl: './questions.component.html',
  styleUrls: [ 'questions.component.css' ]
})
export class QuestionsComponent implements OnInit {
  textFilter = '';

  questions!: Observable<QuestionSummaryDto[]>;

  constructor(private client: HttpClient) { }

  ngOnInit(): void {
    this.refresh();
  }

  refresh() {
    this.questions = this.client.get<QuestionSummaryDto[]>(`${environment.apiBase}/api/questions?q=${this.textFilter}`);
  }
  
  delete(id: number) {
    this.client.delete(`${environment.apiBase}/api/questions/${id}`)
      .subscribe(() => this.refresh());
  }
}
