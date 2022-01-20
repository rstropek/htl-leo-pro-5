using System.Diagnostics.CodeAnalysis;

namespace CityCongestionCharge.Data;

public class CameraDetectionValidator
{
    /// <summary>
    /// Validates a list of camera detections
    /// </summary>
    /// <param name="detections">Detections to validate</param>
    /// <returns>
    /// Empty collection if no errors have beeen found, otherwise collection of
    /// detection errors.
    /// </returns>
    /// <remarks>
    /// This method validates detections as an isolated unit. It assumes that ALL relevant
    /// detections are contained. Detections from previous calls to the method are not
    /// taken into account. Detections stored in the CCC database are also not taken into
    /// account.
    /// </remarks>
    [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "For dependency injection")]
    public IEnumerable<DetectionError> ValidateDetections(IEnumerable<CameraDetection> detections)
    {
        throw new NotImplementedException("You have to implement this logic");
    }
}
