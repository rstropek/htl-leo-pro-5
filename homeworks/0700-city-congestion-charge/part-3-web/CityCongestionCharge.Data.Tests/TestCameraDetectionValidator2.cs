using Xunit;

namespace CityCongestionCharge.Data.Tests;

public class TestCameraDetectionValidator2
{
    [Fact]
    public void Too_Long_LP()
    {
        var detections = new CameraDetection[]
        {
            new(0, 0, "LL-1234AB",     DateTime.Parse("2022-01-11T07:30:00")),
        };

        var validator = new CameraDetectionValidator();
        var errors = validator.ValidateDetections(detections);
        Assert.Single(errors);

        Assert.Equal(new[]
        {
            new DetectionError(0, ErrorType.InvalidLP),
        }, errors);
    }

    [Fact]
    public void Additional_EntryAfterLeave()
    {
        var detections = new CameraDetection[]
        {
            new(0,   0, "L-123XY", DateTime.Parse("2022-01-11T07:30:00")),
            new(1, 200, "L-123XY", DateTime.Parse("2022-01-11T08:10:00")),
            new(2,   0, "L-123XY", DateTime.Parse("2022-01-11T09:30:00")),
            new(3, 200, "L-123XY", DateTime.Parse("2022-01-11T10:10:00")),
        };

        var validator = new CameraDetectionValidator();
        var errors = validator.ValidateDetections(detections);
        Assert.Empty(errors);
    }
}
