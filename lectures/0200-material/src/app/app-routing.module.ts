import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MainMenuComponent } from './main-menu/main-menu.component';
import { ResponsiveGridLayoutComponent } from './responsive-grid-layout/responsive-grid-layout.component';

const routes: Routes = [
  { path: '', pathMatch: 'full', component: MainMenuComponent },
  { path: 'grid', component: ResponsiveGridLayoutComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
