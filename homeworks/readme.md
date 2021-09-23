# Expression Evaluator

## Introduction

Your job is to implement a web API that can evaluate simple mathematical expressions. An example for such an expression is *1+2+3*; result would be *6*). You can earn extra points if your web API supports setting variable values (details see below).

You can find the technical specification of the web API in [*api-spec.yaml*](api-spec.yaml). Note: You can view a human-readable version the specification on [https://editor.swagger.io/](https://editor.swagger.io/). Just copy [*api-spec.yaml*](api-spec.yaml) into the *Swagger* website.

**Pay close attention to the specification!**. Everything (including response codes) has to be implemented as specified.

## Requirements

### Minimum requirements

* Use ASP.NET Core 6 with *minimal API*
* Implement the *evaluate* endpoint (`POST /api/evaluate...`) **without** variables.
  * Expressions can consist of **positive integer** numbers and the operators *+* and *-*. Therefore, expressions have to match the *regular expression* `\d+([+-]\d+)*` to be valid (try it in [https://regexr.com/](https://regexr.com/) if you are not familiar with regular expressions).
* Correct calculation of the formula result.

### Extra points

* You get one extra point if you add at least three **unit tests** (*xUnit*) that verify the correctness of the expression evaluator
* You get one extra point if you implement the API endpoints *getVariables* (`GET /api/variables`) and *setVariable* (`POST /api/variables`) and you allow variables in expressions (e.g. *x*, *1+x*, *x+1+y*). The extended *regular expression* for expressions is `((\d+)|([a-zA-Z_]+))([+-]((\d+)|([a-zA-Z_]+)))*`.

## Test Cases

### *evaluate* without variables

Request:

```http
POST /api/evaluate
Content-Type: application/json

{ "expression": "1+2+3" }
```

Response:

```json
{ "expression": "1+2+3", "result": 6 }
```

### Set a variable

Request:

```http
POST /api/variables
Content-Type: application/json

{ "name": "x", "value": 4 }
```

Response:

```json
{ "name": "x", "value": 4 }
```

### *evaluate* with variable

Assumption: Variable *x* has value *4*.

Request:

```http
POST /api/evaluate
Content-Type: application/json

{ "expression": "1+2+x+3" }
```

Response:

```json
{ "expression": "1+2+x+3", "result": 10 }
```
