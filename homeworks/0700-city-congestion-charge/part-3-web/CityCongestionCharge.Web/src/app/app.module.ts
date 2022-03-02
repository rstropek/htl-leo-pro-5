import { NgModule } from '@angular/core';
import { FlexLayoutModule } from '@angular/flex-layout';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { WelcomeComponent } from './welcome/welcome.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { PaymentsWithDetectionsComponent } from './payments-with-detections/payments-with-detections.component';
import { PaymentsComponent } from './payments/payments.component';
import { NewPaymentComponent } from './new-payment/new-payment.component';

@NgModule({
  declarations: [
    AppComponent,
    PaymentsWithDetectionsComponent,
    WelcomeComponent,
    PaymentsComponent,
    NewPaymentComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FlexLayoutModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
