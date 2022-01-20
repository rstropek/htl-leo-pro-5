using System.Diagnostics.CodeAnalysis;

namespace CityCongestionCharge.Data;

public class CameraDetectionValidator
{
    private enum CameraType
    {
        Enter,
        Leave,
        Inside,
    }

    /// <summary>
    /// Get camera type from camera ID
    /// </summary>
    /// <param name="cameraId">ID of the camera</param>
    /// <returns>
    /// Camera type (see <see cref="CameraType"/>)
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown in case of an invalid camera ID</exception>
    private static CameraType GetCameraType(int cameraId) => cameraId switch
    {
        < 0 => throw new ArgumentOutOfRangeException("Camera ID must be >= 0"),
        < 200 => CameraType.Enter,
        < 400 => CameraType.Leave,
        _ => CameraType.Inside,
    };

    /// <summary>
    /// Validates a list of camera detections
    /// </summary>
    /// <param name="detections">Detections to validate (see remarks section for details)</param>
    /// <returns>
    /// Empty collection if no errors have beeen found, otherwise collection of
    /// detection errors.
    /// </returns>
    /// <remarks>
    /// <para>
    /// Note that detections are ordered by <see cref="CameraDetection.DetectionTimestamp"/> ascending.
    /// The <see cref="CameraDetection.DetectionID"/> also reflects the timestamp when detections happened.
    /// Therefore, detections with lower IDs happend before detections with higher IDs (e.g. detection with ID
    /// 10 happened <em>before</em> a detection with ID 11).
    /// </para>
    /// <para>
    /// This method validates detections as an isolated unit. It assumes that ALL relevant
    /// detections are contained. Detections from previous calls to the method are not
    /// taken into account. Detections stored in the CCC database are also not taken into
    /// account.
    /// </para>
    /// </remarks>
    [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "For dependency injection")]
    public IEnumerable<DetectionError> ValidateDetections(IEnumerable<CameraDetection> detections)
    {
        throw new NotImplementedException("You have to implement this logic");
    }
}
