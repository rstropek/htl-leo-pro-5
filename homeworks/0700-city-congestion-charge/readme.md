# Exercise: City Congestion Charge (CCC)

## Introduction

The city of Linz wants to reduce car traffic in the inner city. For that reason, it has been decided to introduce a [*Congestion Charge*](https://de.wikipedia.org/wiki/Innenstadtmaut) system similar to the one in London. Traffic cameras with license plate recognition software have been installed at all major streets in Linz. Your job in this exercise is to build a software system for Congestion Charge payment.

## Rules

Here is an overview of the most important business rules:

* For every car running on fossile fuels (FF), the owner has to pay **1€ per hour** (wall-clock time) that the car is in Linz (driving or parking). Hybrid electric vehicles (HEV) and electric vehicles (EV) do not need to pay this fee.

* If the car is **driven** (not just parked) during rush hours, an additional **Congestion Charge** (CC) of **3€ per calendar day** has to be paid. This fee has to be paid for **all** cars (FF, HEV, and EV).
  * Rush hours are Monday to Friday between 7:30am and 10:00am and between 3:30pm and 6:00pm

* Lorries and vans have to pay a markup of 50% on all charges. Motorcycles pay 50% less. All other means of transports (e.g. bikes, e-bikes) are always 100% free.

* The maximum amount to pay for a calendar day is 20€.
  * For lorries and vans: 30€
  * For motorcycles: 10€

* People can pay up to 90 days in advance, on the day of travel or by midnight of the third day after travel.

* Everybody can pay for a car using its license plate. The payer does not need to be the car owner.
  * Example: A business owner might pay a fee for two hours for a customer. In this case, the customer only has to pay for additional time that her car was in Linz.
  * Over-payments are lost (e.g. business owner pay for two hours, car was in Linz only for one hour).

## Simplifications

In real world, data delivered from traffic cameras would be full of inconsistencies and errors. We do not deal with that in this exercise. You can assume that data in the DB is consistent and meaningful.

In real world, inhabitants of Linz as well as business owners, Taxi companies, doctors etc. could apply for *Congestion Charge Exemptions*. We do not cover that case in this example. We assume that everybody has to pay.

In real world, rates would change over time. Therefore, the software would need to have configurable rates (e.g. stored in DB with valid-from- and valid-until-dates). This requirement is not relevant for this example.

## Data Model

The following data model has been designed based on the business rules described above:

```txt
                +-------------+ 
                |             | 
       +----- 1 +   Owner     | 
       |        |             | 
       |        +-------------+   
       |                                 
       n       
+------+------+                   +-------------+
|             |                   |             |
|    Car      | n ------------- m +  Detection  |
|             |                   |             |
+------+------+                   +-------------+
       1             
       |             
       |        +-------------+
       |        |             |         
       +----- n +   Payment   |
                |             |
                +-------------+

```

* *Car* represents a physical car uniquely identified by its license plate. Properties of a car:
  * ID (system-provided identification number)
  * License plate (unique; mandatory string, max. 10 characters long)
  * Make (mandatory string, max. 50 characters long)
  * Model (optional string, max. 50 characters long)
  * Color (mandatory string, max. 50 characters long)
  * Car type (mandatory)
    * Passanger car
    * Motorcycle
    * Lorry
    * Van
    * SUV
    * Other

* *Owner* represents the person currently owning a car. Properties of an owner:
  * ID (system-provided identification number)
  * First name (mandatory string, max. 50 characters long)
  * Last name (mandatory string, max. 50 characters long)
  * Address (mandatory, string, max. 100 characters long)

* *Payment* represents a CC payment. Properties of a payment:
  * ID (system-provided identification number)
  * Paid-for date (mandatory, date)
  * PaidAmount (mandatory, decimal)
  * PayingPerson (optional string, max. 100 characters long)

* *Detection* represents a detection of a car by a traffic camera. Note that a single detection (=photo taken by traffic camera) can contain multiple cars. The photos themselves are not stored in the DB; only URLs to the photos are stored. Properties of a detection:
  * ID (system-provided identification number)
  * Date and time when photo has been taken (mandatory)
  * Movement type (mandatory)
    * Entering Linz (detected by a camera at the city boarder)
    * Leaving Linz (detected by a camera at the city boarder)
    * Driving inside Linz (detected by a camera inside of Linz)
  * Photo URL (mandatory, string, max. 200 characters long, min 10 characters long)

## Exercises

1. [Unit testing](part-1-unit-testing)
