# The Adventurer (Part 1)

## Background

This homework should help you to prepare for the upcoming PRO exam and also you final exam in Spring 2022. By solving the requirements, you practice all technologies relevant for both exams.

## Introduction

You are working as an intern in a game development company and your job is to implement a simple [text adventure](https://en.wikipedia.org/wiki/Interactive_fiction#Adventure) called *The Adventurer*. Your application should not just allow people to play the game, they must also be able to setup their own "dungeons".

## The Game

The protagonist in the game is a guy named Jason. Jason is an adventurer and a magic spell has teleported him into a dark and dangerous dungeon. Jason can walk through the dungeon by moving from one room to the other. Some rooms contain items (e.g. weapons, armors, magic potions), some contain monsters. Jason has to look for the dungeon's exit by moving around, picking up items, and fighting monsters.

## Dungeon Builder Requirements

You have to **implement a web API** with which game designers can setup a dungeon.

The detailed design of the API has been fixed with the front-end developers already. You can find it in the form of a *Swagger* file (*Open API Specification*) on [SwaggerHub](https://app.swaggerhub.com/apis/rstropek/dnd-light/0.1). Together with the API specification, you worked out sample requests ([*requests.http*](requests.http)) that you can use to test your API implementation.

The database layer has already been implemented for you (see [starter solution](DndLightStarter)). The skeleton for the web API methods do also exist already. You just have to fill in the blanks for the API methods.

## Exam Simulation

If you manage to build a working version of the required web APIs within approx. 3 hours, you are well prepared for the next exam. In order to reach a positive grade, you should at least be able to build a working [`RoomsController`](DndLightStarter/DndLight.WebApi/RoomsController.cs) within 3 hours.

## Done?

Have you completed this exercise? **Compare your solution with the [sample solution](DndLight) and pay special attention to the comments in the sample solution!**. Then you can [continue with part 2...](readme-part-2.md)
