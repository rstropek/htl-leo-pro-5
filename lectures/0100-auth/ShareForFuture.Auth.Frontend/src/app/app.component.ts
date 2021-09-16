import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  template: `
    <h1>Hello World!</h1>
    <app-auth-button></app-auth-button>
    <h2>User Profile</h2>
    <app-user-profile></app-user-profile>
    <h2>API Call Result</h2>
    <app-api-call></app-api-call>
  `,
  styles: []
})
export class AppComponent {
  title = 'share-for-future';
}
