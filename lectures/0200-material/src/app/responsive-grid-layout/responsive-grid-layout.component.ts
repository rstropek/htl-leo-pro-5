import { Component } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-responsive-grid-layout',
  templateUrl: './responsive-grid-layout.component.html',
  styleUrls: ['./responsive-grid-layout.component.scss']
})
export class ResponsiveGridLayoutComponent {
  public elements: string[] = [];

  constructor(private snackBar: MatSnackBar) {
    this.fillWith(10, true);
  }

  public fillWith(numberOfElements: number, initial?: boolean): void {
    this.elements = [];
    for (let i = 1; i <= numberOfElements; i++) {
      this.elements.push(`Element ${i}`);
    }

    if (!initial) {
      this.snackBar.open('Elements regenerated', undefined, { duration: 1000 });
    }
  }
}
