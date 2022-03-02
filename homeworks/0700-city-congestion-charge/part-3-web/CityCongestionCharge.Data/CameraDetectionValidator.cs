using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

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

    private enum CarState
    {
        IsInside,
        IsOutside,
    }

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
        var result = new List<DetectionError>();

        static CarState GetCarState(IEnumerable<CameraDetection> detections) => detections
                .Select(d => d.CameraID)
                .Sum(cameraId => GetCameraType(cameraId) switch
                {
                    CameraType.Enter => +1,
                    CameraType.Leave => -1,
                    _ => 0
                }) == 0 ? CarState.IsOutside : CarState.IsInside;

        var regex = new Regex(@"^[A-Z]{1,2}-\d{2,4}[A-Z]{1,2}$");

        var ix = 0;
        foreach (var detection in detections)
        {
            if (string.IsNullOrEmpty(detection.DetectedLP)
                || detection.DetectedLP.Length > 8
                || !regex.IsMatch(detection.DetectedLP))
            {
                result.Add(new(detection.DetectionID, ErrorType.InvalidLP));
                continue;
            }

            var previousDetections = detections
                .Take(ix)
                .Where(d => d.DetectedLP == detection.DetectedLP);

            switch (GetCameraType(detection.CameraID))
            {
                case CameraType.Enter:
                    if (GetCarState(previousDetections) == CarState.IsInside)
                    {
                        result.Add(new(detection.DetectionID, ErrorType.AlreadyInside));
                    }

                    break;
                case CameraType.Leave:
                    if (GetCarState(previousDetections) == CarState.IsOutside)
                    {
                        result.Add(new(detection.DetectionID, ErrorType.LeaveNoEnter));
                    }

                    break;
                case CameraType.Inside:
                    if (GetCarState(previousDetections) == CarState.IsOutside)
                    {
                        if (previousDetections.Any(d => GetCameraType(d.CameraID) == CameraType.Leave))
                        {
                            result.Add(new(detection.DetectionID, ErrorType.InsideAfterExit));
                        }
                        else
                        {
                            result.Add(new(detection.DetectionID, ErrorType.InsideNoEnter));
                        }
                    }

                    break;
                default:
                    throw new InvalidOperationException("Invalid camera type, should never happen");
            }

            ix++;
        }

        return result;
    }
}
