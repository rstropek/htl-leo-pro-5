using Xunit;

namespace CityCongestionCharge.Data.Tests;

public class TestCameraDetectionValidator
{
    [Fact]
    public void Example1_Valid_Detections()
    {
        var detections = new CameraDetection[]
        {
            new(0, 0, "L-123XY", DateTime.Parse("2022-01-11T07:30:00")),
            new(1, 400, "L-123XY", DateTime.Parse("2022-01-11T07:35:00")),
            new(2, 401, "L-123XY", DateTime.Parse("2022-01-11T07:40:00")),
            new(3, 200, "L-123XY", DateTime.Parse("2022-01-11T15:10:00")),
        };

        var validator = new CameraDetectionValidator();
        var errors = validator.ValidateDetections(detections);
        Assert.NotNull(errors);
        Assert.Empty(errors);
    }


    [Fact]
    public void Todo()
    {
        // Todo: Replace this test with test for test cases 1b to 8.

        throw new NotImplementedException("Implement unit tests for test cases 1b to 8");
    }
}
