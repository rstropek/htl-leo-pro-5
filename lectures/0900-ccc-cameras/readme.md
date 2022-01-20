# City Congestion Charge (CCC) - Unit Testing Exam

## Introduction

This exercise is part of the larger [*City Congestion Charge* (CCC) sample]([..](https://github.com/rstropek/htl-leo-pro-5/blob/master/homeworks/0700-city-congestion-charge/readme.md)). You should be familiar with this sample from homeworks and lectures. In this exercise, you have to implement some business logic and write unit tests for it.

## The Problem

CCC consists of many traffic cameras distributed all over the city. Some of the cameras are mounted on city borders to recognize cars entering/leaving the city. Some of the cameras are mounted inside the city to detect vehicles driving inside of the city. The type of camera can be derived from a camera'S ID (see also [`GetCameraType` method in *CameraDetectionValidator.cs*](starter-solution/CityCongestionCharge.Data/CameraDetectionValidator.cs)):

* Cameras with IDs 0 to 199 detect cars when they **enter** the city.
* Cameras with IDs 200 to 399 detect cars with they **leave** the city.
* Cameras with IDs >= 400 detect cars when they drive **inside** the city.

Cameras and the license plate recognition is not perfect. Detection data is sometimes inconsistent. Here are error cases that we have to handle:

| Error Code          | Description                                                                                         |
| ------------------- | --------------------------------------------------------------------------------------------------- |
| *LEAVE_NO_ENTER*    | A camera recognizes a car as leaving the city but no camera has ever recorded it entering the city. |
| *INSIDE_NO_ENTER*   | A camera recognizes a car inside the city but we do not have a detection of it entering.            |
| *INSIDE_AFTER_EXIT* | A camera recognizes a car inside the city but we already had a detection of it leaving the city.    |
| *ALREADY_INSIDE*    | A camera recognizes a car entering the city but we already had a detection of it entering.          |

Additionally, license plate recognition is not perfect. Sometimes, the cameras deliver obviously invalid license plate numbers. In our example, we assume that license plate numbers **must always** have the following structure:

* Starts with one or two uppercase letters (e.g. *LL*)
* Followed by a hyphen (minus sign; *-*)
* Followed by two to four digits
* Followed by one or two uppercase letters
* In total, a license plate number must not be longer than 8 characters

Therefore, the regular expression for a single license plate would be [`^[A-Z]{1,2}-\d{2,4}[A-Z]{1,2}$`](regexr.com/6dehg). You can use the regular expression to verify license plate ([`RegEx` class](https://docs.microsoft.com/en-us/dotnet/api/system.text.regularexpressions.regex)), but you do not need to.

Invalid license plates have to lead to the error code *INVALID_LP*.

## Starter Solution

You have a [*starter solution*](starter-solution) for this exercise. It contains data types suitable for the business logic defined above (see [*CameraDetection.cs*](starter-solution/CityCongestionCharge.Data/CameraDetection.cs)).

The starter solution contains the stub of a validation function (see [*CameraDetectionValidator.cs*](starter-solution/CityCongestionCharge.Data/CameraDetectionValidator.cs)). Additionally, it contains sample unit tests (see [*TestCameraDetectionValidator.cs*](starter-solution/CityCongestionCharge.Data.Tests/TestCameraDetectionValidator.cs)).

## Your Job

1. Complete the validation function in [*CameraDetectionValidator.cs*](starter-solution/CityCongestionCharge.Data/CameraDetectionValidator.cs)) according to the business logic defined above. Structure your code so that every non-trivial logic aspect can be automatically tested using unit tests.

2. Implement unit tests for all test cases defined below. Add your unit tests to [*TestCameraDetectionValidator.cs*](starter-solution/CityCongestionCharge.Data.Tests/TestCameraDetectionValidator.cs).

3. Write at least two unit tests for technical edge cases. You have to define the details of those edge cases yourself. Samples for technical edge cases could be:
   * A parameter is `null` but `null` is not a valid input.
   * Method is called with an empty detection collection.
   * Detections contain a negative camera ID.
   * Detections contain an empty string as a license plate number.

## Test Cases for Validation

### Example 1a: Valid detections (single car)

The following combination of detections is fine:

| Detection ID | Camera ID | Detected LP | Detection Timestamp |
| -----------: | --------: | ----------- | ------------------- |
|            0 |         0 | L-123XY     | 2022-01-11T07:30:00 |
|            1 |       400 | L-123XY     | 2022-01-11T07:35:00 |
|            2 |       401 | L-123XY     | 2022-01-11T07:40:00 |
|            3 |       200 | L-123XY     | 2022-01-11T15:10:00 |

### Example 1b: Valid detections (multiple cars)

The following combination of detections is fine:

| Detection ID | Camera ID | Detected LP | Detection Timestamp |
| -----------: | --------: | ----------- | ------------------- |
|            0 |         0 | L-123XY     | 2022-01-11T07:30:00 |
|            1 |       400 | L-123XY     | 2022-01-11T07:35:00 |
|            2 |         0 | W-789AB     | 2022-01-11T08:10:00 |
|            3 |       401 | L-123XY     | 2022-01-11T08:40:00 |
|            4 |       402 | W-789AB     | 2022-01-11T09:15:00 |
|            5 |       200 | L-123XY     | 2022-01-11T15:10:00 |
|            6 |       200 | W-789AB     | 2022-01-11T19:25:00 |

### Example 2: Leaving without entering

The following combination is not ok because the car leaves without ever entering:

| Detection ID | Camera ID | Detected LP | Detection Timestamp |
| -----------: | --------: | ----------- | ------------------- |
|            0 |       200 | L-123XYZ    | 2022-01-11T14:13:00 |

Expected error result:

| Detection ID | Error Code     |
| -----------: | -------------- |
|            0 | LEAVE_NO_ENTER |

### Example 3: Driving inside without entering

The following combination is not ok because the car drives in the city without ever entering:

| Detection ID | Camera ID | Detected LP | Detection Timestamp |
| -----------: | --------: | ----------- | ------------------- |
|            0 |       400 | L-123XY     | 2022-01-11T07:35:00 |
|            1 |       401 | L-123XY     | 2022-01-11T07:40:00 |

Expected error result:

| Detection ID | Error Code      |
| -----------: | --------------- |
|            0 | INSIDE_NO_ENTER |
|            1 | INSIDE_NO_ENTER |

### Example 4: Driving inside after leaving

The following combination is not ok because the car drives in the city after leaving:

| Detection ID | Camera ID | Detected LP | Detection Timestamp |
| -----------: | --------: | ----------- | ------------------- |
|            0 |         0 | L-123XY     | 2022-01-11T07:30:00 |
|            1 |       200 | L-123XY     | 2022-01-11T15:10:00 |
|            2 |       400 | L-123XY     | 2022-01-11T15:35:00 |

Expected error result:

| Detection ID | Error Code        |
| -----------: | ----------------- |
|            2 | INSIDE_AFTER_EXIT |

### Example 5: Multiple entering

The following combination is not ok because the car enters the city twice without leaving in between:

| Detection ID | Camera ID | Detected LP | Detection Timestamp |
| -----------: | --------: | ----------- | ------------------- |
|            0 |         0 | L-123XY     | 2022-01-11T07:30:00 |
|            1 |         0 | L-123XY     | 2022-01-11T07:35:00 |

Expected error result:

| Detection ID | Error Code     |
| -----------: | -------------- |
|            1 | ALREADY_INSIDE |

### Example 6: Multiple leaving

The following combination is not ok because the car leaves the city twice without entering in between:

| Detection ID | Camera ID | Detected LP | Detection Timestamp |
| -----------: | --------: | ----------- | ------------------- |
|            0 |         0 | L-123XY     | 2022-01-11T07:30:00 |
|            1 |       200 | L-123XY     | 2022-01-11T14:13:00 |
|            2 |       200 | L-123XY     | 2022-01-11T15:13:00 |

Expected error result:

| Detection ID | Error Code     |
| -----------: | -------------- |
|            2 | LEAVE_NO_ENTER |

### Example 7: Valid License Plate Numbers

The following detections are fine:

| Detection ID | Camera ID | Detected LP | Detection Timestamp |
| -----------: | --------: | ----------- | ------------------- |
|            0 |         0 | L-123AB     | 2022-01-11T07:30:00 |
|            1 |         0 | W-12FX      | 2022-01-11T07:30:00 |
|            2 |         0 | S-9876X     | 2022-01-11T07:30:00 |
|            3 |         0 | LL-1234Y    | 2022-01-11T07:30:00 |

### Example 8: Invalid License Plate Numbers

The following detections are not ok because of invalid LP numbers:

| Detection ID | Camera ID | Detected LP  | Detection Timestamp |
| -----------: | --------: | ------------ | ------------------- |
|            0 |         0 | LLL-123A     | 2022-01-11T07:30:00 |
|            1 |         0 | -123AB       | 2022-01-11T07:30:00 |
|            2 |         0 | 123AB        | 2022-01-11T07:30:00 |
|            3 |         0 | UU-KNIGHT1   | 2022-01-11T07:30:00 |
|            4 |         0 | WU-123456789 | 2022-01-11T07:30:00 |
|            5 |         0 | X            | 2022-01-11T07:30:00 |
|            6 |         0 | 1            | 2022-01-11T07:30:00 |
|            7 |         0 | -            | 2022-01-11T07:30:00 |

Expected error result:

| Detection ID | Error Code |
| -----------: | ---------- |
|            0 | INVALID_LP |
|            1 | INVALID_LP |
|            2 | INVALID_LP |
|            3 | INVALID_LP |
|            4 | INVALID_LP |
|            5 | INVALID_LP |
|            6 | INVALID_LP |
|            7 | INVALID_LP |
