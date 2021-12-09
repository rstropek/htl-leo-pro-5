# Maturatrainer

## Einleitung

Sie bereiten sich gerade für die HTL-Matura vor. In einem Lernteam haben Sie Fragen ausgearbeitet, die möglicherweise zur Matura kommen. Jetzt möchten Sie ein Programm schreiben, mit dessen Hilfe man die Fragen erfassen und anschließend üben kann.

Sie haben beschlossen, eine Web App zu bauen, in der Sie für die Matura üben können.

## Funktionale Anforderungen

### Erfassung von Fragen

* Erfassung von Fragen.
* Jede Frage besteht aus einem Fragetext und 1..9 Antwortmöglichkeiten.
* Je Antwortmöglichkeit wird hinterlegt, ob die Antwort korrekt ist.

### Liste von Fragen

* Anzeige einer Liste von Fragen als Tabelle.
* Optionale Filtermöglichkeiten:
  * Fragetext (filtern mit *contains*)

### Löschen von Fragen

* In der Liste der Fragen muss es je Frage einen *Löschen*-Button geben, mit der die Frage gelöscht werden kann.

### Quiz durchführen

* Man kann die Fragen verwenden, um zu üben.

## Nicht-Funktionale Anforderungen

* Backend API mit .NET 6 (RC 2 oder finale Version)
* Entity Framework Core 6 für DB-Zugriff (RC 2 oder finale Version)
* SQL Server (LocalDB oder Docker Container) als Datenbankserver
* Angular 12 oder 13 für Benutzerschnittstelle

## Starter-Solution

Sie bauen bei diesem Test auf einer bereitgestellten [Starter-Solution](MaturaTrainerStarter) auf. Die Starter-Solution enthält folgende Teile, die Ihnen den Start erleichtern sollen:

* Grundgerüst für [.NET 6](MaturaTrainerStarter/MaturaTrainer.Api)- und [Angular 13](MaturaTrainerStarter/MaturaTrainer.UI)-Anwendung mit allen Abhängigkeiten (NuGet und NPM), die Sie zur Lösung der Aufgabe brauchen.
* Fertiger [EFCore Data Context](MaturaTrainerStarter/MaturaTrainer.Api/Database.cs) mit fertigem Datenmodell.
* Grundgerüst für einen [ASP.NET Core Controller](MaturaTrainerStarter/MaturaTrainer.Api/QuestionsController.cs).
* [Beispiel-Requests](requests.http) mit Demo-Daten
* Grundgerüst des [HTML- und CSS-Codes für das UI](MaturaTrainerStarter/MaturaTrainer.UI/src/app). Die UI für *Quiz durchführen* ist komplett ([HTML- und CSS-Codes für das UI](MaturaTrainerStarter/MaturaTrainer.UI/src/app/quiz/quiz.component.ts). Sie müssen nur den API-Zugriff hinzufügen.

## Bewertung

### Minimale Anforderungen

Folgende Anforderungen muss ihr Programm mindestens erfolgen, um positiv bewertet zu werden:

* Code kompiliert ohne Fehler.
* Anwendung stürzt nicht sofort ab wenn sie gestartet wird.
* Code ist in GitHub eingecheckt (**mit** entsprechender *.gitignore*-Datei).
* *Erfassung von Fragen* funktioniert
* *Liste von Fragen* funktioniert **ungefiltert**
* *Löschen von Fragen* funktioniert

### Noten

Die jeweilige Note zwischen 1 und 4 ergibt sich aus:

* Vollständigkeit der implementierten Anforderungen
* Codequalität
* Fehlerbehandlung
* UI/UX Design (klares und einfaches Design, keine Extrapunkte für visuell besonders kreative Lösungen)
* Web API Design (z.B. richtige Nutzung von HTTP Statuscodes, HTTP Methoden etc.)
