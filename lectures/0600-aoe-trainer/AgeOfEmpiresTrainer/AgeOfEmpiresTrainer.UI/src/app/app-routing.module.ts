import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AddGameComponent } from './add-game/add-game.component';
import { GameResultsComponent } from './game-results/game-results.component';

const routes: Routes = [
  { path: '', pathMatch: 'full', redirectTo: 'game-results' },
  { path: 'game-results', component: GameResultsComponent  },
  { path: 'add-game', component: AddGameComponent  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
