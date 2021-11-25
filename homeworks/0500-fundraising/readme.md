# Fundraising

## Introduction

Traditionally, non-profit organizations like *freiwillige Feuerwehr* raise money by going from door to door asking for donations. Obviously, the fundraising teams try to avoid visiting a single household multiple times.

You do volunteer work in such a non-profit organization and you want to build a simple web app with which the fundraising teams can coordinate their visits.

## Functional Requirements

### Campaigns

*As a fundraising team manager, I want to maintain a list of fundraising campaigns (e.g. _Weihnachtssammlung 2021_).*

* Create a campaign
* Delete a campaign (only possible if no related data is in the DB for the given campaign)
* Update the name of a campaign

### Check Household

*As a fundraiser, I want to check whether a team already visited a certain household and met someone there.*

Input:

* (Mandatory) Campaign (Dropdown, not just a text field)
* (Optional) Part of town name
  * Example: *Leo* when looking for *Leonding*
* (Optional) Part of street name
  * Example: *Limes* when looking for *Limesstraße*
* Flag whether all visits should be shown or only unsuccessful ones (i.e. visits where nobody was at home).

Output:

* List of all visits fitting the input filter criteria (see above).
* Each result record has to contain the following properties:
  * Town name
  * Street name
  * House number
  * Family name
  * Flag whether someone was met or nobody was at home

### Enter Visits

*As a fundraiser, I want to enter a visit.*

Input:

* (Mandatory) Campaign (Dropdown, not just a text field)
* (Mandatory) Town name (max. length 150 chars)
* (Mandatory Street name (max. length 150 chars)
* (Mandatory) House number
  * Starts with a number >= 1
  * Can optionally contain a single letter after the number
* (Optional) Family name (max. length 150 chars)
* Flag whether someone was met or nobody was at home

### Enter Successful Re-visit

*As a fundraiser, I want to be able to mark a household that was successfully re-visited.*

This function must be available from within the list generated by the *Check Household* feature. For unsuccessful visits, there has to be a button with which the visit can be marked as successful. A fundraiser will click the button if she re-visited the household and met somebody there.

## Non-Functional Requirements

* Use .NET 6 to implement the backend web API.
* Use Entity Framework Core 6 to implement the database access.
* Use SQL Server (LocalDB or Docker Container) as your database.
* Use Angular 12 for UI development.

## Out-of-Scope

* Authentication and authorization

## Grading

### Minimum Requirements

In order to reach a positive grade, you must fulfill the following requirements:

* Code compiles without any error.
* App does not immediately crash when started.
* Code is checked-in to GitHub (**with** proper *.gitignore* file(s)).
* *Campaigns* feature works as specified (backend and frontend).
* *Check Household* feature works *without* filtering (backend and frontend).
* User can navigate between different UI functions.

### Grades

You grade will depend on the following aspects:

* Completeness of the implementation of the requirements
* Quality of your code
* Proper handling of edge cases (e.g. meaningful message if the user tries to delete a campaign with data relating to it)
* UI/UX design (focus on clean and simple design, no extra points for visual creativity)
* Web API design (e.g. correct use of HTTP status codes, HTTP methods, etc.)