import { HttpClient, HttpParams } from "@angular/common/http";
import { Component, OnInit } from "@angular/core";
import { Observable } from "rxjs";
import { environment } from "src/environments/environment";
import { PaymentResultDto, PaymentType } from "../model";

// Todo: Complete the component logic

@Component({
  selector: "app-payments",
  templateUrl: "./payments.component.html",
  styleUrls: ["./payments.component.css"],
})
export class PaymentsComponent implements OnInit {

  constructor(private http: HttpClient) {}

  ngOnInit(): void {
  }

  getPaymentTypeDescription(type: PaymentType): string {
    switch (type) {
      case PaymentType.Cash:
        return "Cash";
      case PaymentType.BankTransfer:
        return "Bank Transfer";
      case PaymentType.CreditCard:
        return "Creditcard";
      case PaymentType.DebitCard:
        return "Debitcard";
      default:
        return "unknown";
    }
  }
}
