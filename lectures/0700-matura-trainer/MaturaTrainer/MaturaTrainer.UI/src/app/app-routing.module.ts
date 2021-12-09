import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AddQuestionComponent } from './add-question/add-question.component';
import { QuestionsComponent } from './questions/questions.component';
import { QuizComponent } from './quiz/quiz.component';

const routes: Routes = [
  { path: '', pathMatch: 'full', redirectTo: 'game-results' },
  { path: 'questions', component: QuestionsComponent  },
  { path: 'add-question', component: AddQuestionComponent  },
  { path: 'quiz', component: QuizComponent  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
