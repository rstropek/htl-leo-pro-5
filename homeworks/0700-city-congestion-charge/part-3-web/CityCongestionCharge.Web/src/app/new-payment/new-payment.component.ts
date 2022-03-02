import { HttpClient } from "@angular/common/http";
import { Component, OnInit } from "@angular/core";
import { Router } from "@angular/router";
import { environment } from "src/environments/environment";
import { PaymentAddDto, PaymentType } from "../model";

// Todo: Complete the component logic

@Component({
  selector: "app-new-payment",
  templateUrl: "./new-payment.component.html",
  styleUrls: ["./new-payment.component.css"],
})
export class NewPaymentComponent {
  constructor(private http: HttpClient, private router: Router) {}
}
