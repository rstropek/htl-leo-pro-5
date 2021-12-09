import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { environment } from 'src/environments/environment';
import { AddAnswerOptionDto, AddQuestionDto } from '../model';

@Component({
  selector: 'app-add-question',
  templateUrl: './add-question.component.html',
  styleUrls: [ 'add-question.component.css' ]
})
export class AddQuestionComponent {
  text = '';
  options: AddAnswerOptionDto[] = [
    { text: '', isCorrect: true },
  ];

  constructor(private client: HttpClient, private router: Router) { }

  addOption() {
    this.options.push({ text: '', isCorrect: true });
  }

  delete(i: number) {
    this.options.splice(i, 1);
  }

  save() {
    const question: AddQuestionDto = {
      text: this.text,
      options: this.options.map(o => {
        return {
          text: o.text,
          isCorrect: o.isCorrect,
        };
      })
    };
    this.client.post(`${environment.apiBase}/api/questions`, question)
      .subscribe(() => this.router.navigate(['questions']));
  }
}
