import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { environment } from 'src/environments/environment';
import { Question } from '../model';

@Component({
  selector: 'app-quiz',
  templateUrl: './quiz.component.html',
  styleUrls: [ './quiz.component.css' ]
})
export class QuizComponent implements OnInit {
  question?: Question;
  answers: boolean[] = [];
  correct?: boolean;
  answered = false;
  numberOfQuestions = 0;
  numberOfCorrectQuestions = 0;

  constructor(private client: HttpClient, private router: Router) { }

  ngOnInit(): void {
    this.loadRandom();
  }

  loadRandom() {
    this.client.get<Question>(`${environment.apiBase}/api/questions/random`)
    .subscribe(q => {
        this.answered = false;
        this.numberOfQuestions++;
        delete this.correct;
        this.question = q;
        this.answers = [];
        for(let i = 0; i < q.options.length; i++) {
          this.answers.push(false);
        }
      });
  }
  
  check() {
    this.answered = true;
    for(let i = 0; i < this.answers.length; i++) {
      if (this.question?.options[i].isCorrect !== this.answers[i]) {
        this.correct = false;
        return;
      }
    }

    this.correct = true;
    this.numberOfCorrectQuestions++;
  }

  finish() {
    throw new Error("Todo: Implement the necessary code to save quiz result to DB using API");
  }
}
