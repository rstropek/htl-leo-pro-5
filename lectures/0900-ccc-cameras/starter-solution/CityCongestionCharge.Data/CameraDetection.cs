namespace CityCongestionCharge.Data;

/// <summary>
/// Represents a detection of a car by a traffic camera
/// </summary>
/// <param name="DetectionID">Unique key of the detection</param>
/// <param name="CameraID">ID of the camera</param>
/// <param name="DetectedLP">Detected license plate</param>
/// <param name="DetectionTimestamp">Date and time when detection happened</param>
public record CameraDetection(
    int DetectionID,
    int CameraID,
    string DetectedLP,
    DateTime DetectionTimestamp);

public enum ErrorType
{
    /// <summary>
    /// A camera recognizes a car as leaving the city but no camera has ever recorded it entering the city.
    /// </summary>
    LeaveNoEnter,

    /// <summary>
    /// A camera recognizes a car inside the city but we do not have a detection of it entering.
    /// </summary>
    InsideNoEnter,

    /// <summary>
    /// A camera recognizes a car inside the city but we already had a detection of it leaving the city.
    /// </summary>
    InsideAfterExit,

    /// <summary>
    /// A camera recognizes a car entering the city but we already had a detection of it entering.
    /// </summary>
    AlreadyInside,

    /// <summary>
    /// A camera delivered an invalid license plate number
    /// </summary>
    InvalidLP
}

/// <summary>
/// Represents a detection error
/// </summary>
/// <param name="DetectionID">Unique key of the detection that has an error</param>
/// <param name="ErrorType">Type of error</param>
public record DetectionError(
    int DetectionID,
    ErrorType ErrorType);
