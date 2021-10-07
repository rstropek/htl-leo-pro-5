# Search API

## Introduction

Last week, we worked on the [data layer for the _Share for Future_ (S4F) project](https://github.com/rstropek/htl-leo-pro-5/tree/master/homeworks/0200-data-access). In this exercise, you have to implement a UI aspect of the project: The _offering search_.

## Functional Requirements

### Google-like Search

Users can search for S4F offerings in a Google-like way. That means that a user can enter a single search string and S4F uses it to find relevant offerings.

### Single-word Search

If the user enters a single word, S4F has to return all offerings for which...

- ...the search word is contained in the offering's title **or**
- ...the search word is contained in the offering's description **or**
- ...the search word is contained in a tag associated with the offering.

Here is an example. Let's assume that we have the following offerings in the DB:

| Title            | Description                                                           | Tags                              |
| ---------------- | --------------------------------------------------------------------- | --------------------------------- |
| **Drill**ing machine | Makita drilling machine, great for **concrete** walls                     | **Tool**, **Drill**ing, Makita            |
| Bosch **drill**er    | **Drill**ing machine from Bosch, good for light home improvement projects | Machine, Bosch, Home Improvement |
| Beamer           | Full-HD beamer for watching TV                                        | TV, Beamer, Home **Cinema**           |

If the user searches for *drill*, both drilling machines must be returned. If the user searches for *Cinema*, the beamer must be returned. If the user searches for *concrete*, the *Makita* drilling machine must be returned. If the user searches for *Laptop*, the system has to display a message that no matching data was found.

### Multi-word Search

If the user enters multiple words (separated by 0..n whitespaces), S4F has to apply the search logic mentioned above for each word. An offering has to be included in the result if it contains **at least one** of the search words. The results must be sorted based on the number of matching search words per offering.

Here is an example. Let's assume the same DB content as shown above. If the user searches for *drill tool*, both drilling machines must be returned because both records contain *drill*. The *Makita* drilling machine must be shown **first**, because it's data contains both *drill* **and** *tool*. The *Bosch* machine has to be second, because it contains *drill*, but not *tool*.

## Non-functional Requirements

* Use .NET 6 for implementing the backend web API.
* Use the existing [data layer for the _Share for Future_ (S4F) project](https://github.com/rstropek/htl-leo-pro-5/tree/master/homeworks/0200-data-access) to access a SQL Server database.
* Use Angular to implement the frontend.

## Extra Points

* One extra point: Avoid a dedicated *Search* button. If the user types text in the search field and stops typing for at least 200ms, the search should be triggered automatically.
* One extra point: Implement an addditional web API endpoint that can be used to fill the database with auto-generated demo data for offerings. Use [*Bogus*](https://github.com/bchavez/Bogus) to generate the data.
