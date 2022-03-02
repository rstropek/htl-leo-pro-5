import { HttpClient } from "@angular/common/http";
import { Component, OnInit } from "@angular/core";
import { Router } from "@angular/router";
import { environment } from "src/environments/environment";
import { PaymentAddDto, PaymentType } from "../model";

@Component({
  selector: "app-new-payment",
  templateUrl: "./new-payment.component.html",
  styleUrls: ["./new-payment.component.css"],
})
export class NewPaymentComponent {
  payment: PaymentAddDto = {
    licensePlate: '',
    paidAmount: 0,
    payingPerson: '',
    paymentType: PaymentType.Cash
  };

  constructor(private http: HttpClient, private router: Router) {}

  save() {
    if (this.payment.licensePlate) {
      // Note: Students need to show that they know how to post data to RESTful api.
      this.http
        .post(`${environment.apiBaseUrl}/api/payments`, this.payment)
        .subscribe(() => this.router.navigateByUrl("/payments"));
    }
  }
}
