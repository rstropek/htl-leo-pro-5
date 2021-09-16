import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AuthModule } from '@auth0/auth0-angular';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';

import { AuthHttpInterceptor } from '@auth0/auth0-angular';

import { AppComponent } from './app.component';
import { AuthButtonComponent } from './auth-button.component';
import { UserProfileComponent } from './user-profile.component';
import { ApiCallComponent } from './api-call.component';

// Read more about how to add login to an Angular app with
// Auth0 at https://auth0.com/docs/quickstart/spa/angular.

@NgModule({
  declarations: [
    AppComponent,
    AuthButtonComponent,
    UserProfileComponent,
    ApiCallComponent
  ],
  imports: [
    BrowserModule,
    AuthModule.forRoot({
      // The domain and clientId were configured in the previous chapter
      domain: 'rainerdemo-eu.eu.auth0.com',
      clientId: 'JdXG3QuEZitYsTb5eTvkhPLU0nVV2sk2',
    
      // Request this audience and scope at user authentication time
      audience: 'https://api.shareforfuture.net',
      scope: 'impersonate',
    
      // Specify configuration for the interceptor              
      httpInterceptor: {
        allowedList: [
          {
            uri: 'https://localhost:5001/*',
            tokenOptions: {
              audience: 'https://api.shareforfuture.net',
              scope: 'impersonate'
            }
          }
        ]
      }
    }),
    HttpClientModule,
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: AuthHttpInterceptor, multi: true },
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
