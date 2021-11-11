# Spielstatistik

## Einleitung

Sie sind begeisterter Spieler des Echtzeitstrategiespiels *Age of Empires* (AoE) und möchten ihre Spielstärke in Vorbereitung auf die nächste Game-Party strukturiert verbessern. Bei AoE gibt es verschiedene Zivilisationen (z.B. Engländer, Chinesen, Mongolen, Franzosen, etc.). Jede Zivilisation hat ihre Stärken und Schwächen. Ihre Lieblingszivilisation sind die Mongolen und sie möchten durch Übungsspiele trainieren, gegen andere Zivilisationen zu gewinnen.

Sie haben beschlossen, eine Web App zu bauen, in der Sie Ihren Trainingsfortschritt dokumentieren können. Die gesammelten Daten sind die Basis für strukturiertes Spieltraining.

## Funktionale Anforderungen

### Erfassung von Spielergebnissen

* Erfassung von Spielergebnissen.
* Jedes Spielergebnis muss mindestens folgende Informationen enthalten:
  * Anzahl KI-Spieler (**Wert zwischen 1 und 3**)
  * Je KI-Spieler...
    * ...die [Zivilisation](https://www.ageofempires.com/games/age-of-empires-iv#game-civilizations),
    * ...die Spielstärke (*easy*, *intermediate*, *hard*) und
    * ...ein Kennzeichen, ob dieser KI-Spieler besiegt wurde.
  * Spielergebnis (verloren oder gewonnen)
  * Erreichtest Zivilisationslevel (**Zahl zwischen 1 und 4**)
  * Optionale Notizen (langes Textfeld)
* Die Spielergebnisse müssen eine aufsteigende ID haben, um erkennen zu können, in welcher Reihenfolge die Spielergebnisse zustande kamen.

### Liste von Spielergebnissen

* Anzeige einer Liste von Spielergebnissen als Tabelle.
* Optionale Filtermöglichkeiten:
  * Anzahl KI-Spieler
  * Gegnerische Zivilisation (mindestens ein KI-Gegner muss diese Zivilisation gespielt haben)
  * Spielergebnis
* Die Tabelle muss mindestens folgende Spalten enthalten:
  * Anzahl KI-Spieler
  * Gegnerische Zivilisationen (Komma-getrennte Liste)
  * Spielergebnis
* Die Tabelle muss *absteigend* nach der Spielergebnis-ID sortiert sein.

### Löschen von Spielergebnissen

* In der Liste der Spielergebnisse muss es je Ergebnis einen *Löschen*-Button geben, mit der das Spielergebnis gelöscht werden kann.

## Nicht-Funktionale Anforderungen

* Backend API mit .NET 6 (RC 2 oder finale Version)
* Entity Framework Core 6 für DB-Zugriff (RC 2 oder finale Version)
* SQL Server (LocalDB oder Docker Container) als Datenbankserver
* Angular 12 oder 13 für Benutzerschnittstelle

## Starter-Solution

Sie können bei diesem Test wählen, ob sie den Code von Grund auf selbst schreiben oder auf einer bereitgestellten [Starter-Solution](AgeOfEmpiresTrainerStarter) aufbauen wollen. Die Starter-Solution enthält folgende Teile, die Ihnen den Start erleichtern sollen:

* Grundgerüst für [.NET 6](AgeOfEmpiresTrainerStarter/AgeOfEmpiresTrainer.Api)- und [Angular 13](AgeOfEmpiresTrainerStarter/AgeOfEmpiresTrainer.UI)-Anwendung mit allen Abhängigkeiten (NuGet und NPM), die Sie zur Lösung der Aufgabe brauchen.
* Grundgerüst für einen [EFCore Data Context](AgeOfEmpiresTrainerStarter/AgeOfEmpiresTrainer.Api/Database.cs).
* Grundgerüst für eine [Klasse mit DB-Zugriffsmethoden](AgeOfEmpiresTrainerStarter/AgeOfEmpiresTrainer.Api/GameResultManager.cs).
* Grundgerüst für einen [ASP.NET Core Controller](AgeOfEmpiresTrainerStarter/AgeOfEmpiresTrainer.Api/GameResultsController.cs).
* [Beispiel-Requests](AgeOfEmpiresTrainerStarter/AgeOfEmpiresTrainer.Api/requests.http) mit Demo-Daten
* Grundgerüst des [HTML- und CSS-Codes für das UI](AgeOfEmpiresTrainerStarter/AgeOfEmpiresTrainer.UI/src/app)

**Sie müssen sich NICHT an das in der Starter-Solution umgesetzte UI-Design oder an die bereitgestellten Muster-Requests halten**. Sie können ohne weiteres einen anderen Ansatz im Bereich UI-Design und/oder API-Design wählen!

## Bewertung

### Minimale Anforderungen

Folgende Anforderungen muss ihr Programm mindestens erfolgen, um positiv bewertet zu werden:

* Code kompiliert ohne Fehler.
* Anwendung stürzt nicht sofort ab wenn sie gestartet wird.
* Code ist in GitHub eingecheckt (**mit** entsprechender *.gitignore*-Datei).
* *Erfassung von Spielergebnissen* funktioniert **ohne** einzelne KI-Spieler
* *Liste von Spielergebnissen* funktioniert **ungefiltert** und **ohne** Details über KI-Spieler (gegnerische Zivilisation)
* *Löschen von Spielergebnissen* funktioniert

### Noten

Die jeweilige Note zwischen 1 und 4 ergibt sich aus:

* Vollständigkeit der implementierten Anforderungen
* Codequalität
* Fehlerbehandlung
* UI/UX Design (klares und einfaches Design, keine Extrapunkte für visuell besonders kreative Lösungen)
* Web API Design (z.B. richtige Nutzung von HTTP Statuscodes, HTTP Methoden etc.)
