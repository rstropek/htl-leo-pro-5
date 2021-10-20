# Web API Exercise E2E

## Introduction

The purpose of this homework is to practice design, implementation, and testing of an end-to-end application using ASP.NET Core 6, Entity Framework 6, and Angular 12.

This homework is based on the [Share4Future sample](../0300-search-api) that we have been working on over the last weeks. You can use the previously created data access layer in this homework.

## Functional Requirements

Your job is to implement a prototype for a web application with which users can create offerings. The web application has to implement the following functions:

* Login has not been implemented yet. Therefore, the app should allow the user to enter the ID of her user record and assume that the given user is currently signed in.

* Get a list of all offerings from the signed-in user.
* Toggle the *currently available* state.
* Create a new offering. The user must be able to enter at least the following data items for the offering:
  * Title
  * Description
  * Device condition (select, not just plain text)
  * Subcategory (select, not just plain text)
  * Tags (text box with comma-separated tags is sufficient)
* Alter an existing offering. The user must be able to alter at least the following data for the offering:
  * Title
  * Description
  * Device condition (select, not just plain text)

## Non-Functional Requirements

* Use ASP.NET Core 6 and Entity Framework 6 in the backend
* Design a proper web API (e.g. correct use of HTTP status codes, self-describing paths, etc.)
* Include error handling in the API
* Create a SPA client with Angular 12
