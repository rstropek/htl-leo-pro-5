import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { QuestionSummaryDto } from '../model';

@Component({
  selector: 'app-questions',
  templateUrl: './questions.component.html',
  styleUrls: [ 'questions.component.css' ]
})
export class QuestionsComponent implements OnInit {

  constructor(private client: HttpClient) { }

  ngOnInit(): void {
  }
}
