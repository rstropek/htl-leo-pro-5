import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule, ReactiveFormsModule } from "@angular/forms";

import { AppRoutingModule } from './app-routing.module';
import { MaterialModules } from "./mat-imports";
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ResponsiveGridLayoutComponent } from './responsive-grid-layout/responsive-grid-layout.component';
import { FlexLayoutModule } from '@angular/flex-layout';
import { MainMenuComponent } from './main-menu/main-menu.component';

@NgModule({
  declarations: [
    AppComponent,
    ResponsiveGridLayoutComponent,
    MainMenuComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    FormsModule,
    ReactiveFormsModule,
    FlexLayoutModule,
    [...MaterialModules],
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
