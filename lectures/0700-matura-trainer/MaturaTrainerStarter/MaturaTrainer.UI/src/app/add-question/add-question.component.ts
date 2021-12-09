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
}
