# Wördle Solver

## Introduction

[https://wordle.at/](https://wordle.at/) is a popular word guessing game. Your job is to implement a solver tool for *wordle.at* similar to the one we created for *mathler* during lectures.

## Requirements

### Business Logic

* Start with the given [StarterSolution](StarterSolution).
* Complete the business logic in [PuzzleSolver.cs](StarterSolution/WoerdleSolver.Logic/WoerdleSolver.cs).
  * Don't try to create a perfect algorithm for suggesting words. Come up with a decent business logic for generating meaningful word lists that fulfills all existing unit tests. The quality of your algorithm influences your grade.
* Complete the unit tests in [WoerdleSolverTests.cs](StarterSolution\WoerdleSolver.Logic.Tests\WoerdleSolverTests.cs). Make sure that all become green.

### WPF UI

* Complete the UI and view logic in [MainWindow.xaml](StarterSolution/WoerdleSolver.WpfUI/MainWindow.xaml) and [MainWindow.xaml.cs](StarterSolution/WoerdleSolver.WpfUI/MainWindow.xaml.cs).
* Note that [MainWindow.xaml.cs](StarterSolution/WoerdleSolver.WpfUI/MainWindow.xaml.cs) already contains properties that you can use in your implementation.

### Web UI

* Complete the web API in [SolverController.cs](StarterSolution\WoerdleSolver.WebApi\Controllers\SolverController.cs)
  * Test the web API with Swagger.
* Complete the Angular UI in [WoerdleSolver.WebUI](StarterSolution\WoerdleSolver.WebUI\src\app).

## Grading

To get a positive grade, you have to implement at least basic solutions in all areas (logic, unit tests, WPF, web API, web UI). Your code **must** compile without errors. You code **most not** crash immediately after starting it.

The quality of your code (e.g. coding style, compiler warnings, code efficiency) and algorithms determine your grade.
