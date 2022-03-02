import { HttpClient, HttpParams } from "@angular/common/http";
import { Component, OnInit } from "@angular/core";
import { Observable } from "rxjs";
import { environment } from "src/environments/environment";
import { PaymentResultDto, PaymentType } from "../model";

@Component({
  selector: "app-payments",
  templateUrl: "./payments.component.html",
  styleUrls: ["./payments.component.css"],
})
export class PaymentsComponent implements OnInit {
  paymentType?: PaymentType = undefined;
  onlyFuturePayments: boolean = false;
  onlyAnonymous: boolean = false;

  payments?: Observable<PaymentResultDto[]>;

  constructor(private http: HttpClient) {}

  ngOnInit(): void {
    this.refresh();
  }

  refresh() {
    let params = new HttpParams();
    if (this.paymentType !== undefined) {
      params = params.append("type", this.paymentType);
    }

    if (this.onlyFuturePayments) {
      params = params.append("future", this.onlyFuturePayments);
    }

    if (this.onlyAnonymous) {
      params = params.append("anonym", this.onlyAnonymous);
    }

    const paramsString = params.toString();

    this.payments = this.http.get<PaymentResultDto[]>(`${environment.apiBaseUrl}/api/payments${paramsString ? '?' : ''}${paramsString}`);
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
