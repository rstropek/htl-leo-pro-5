using Xunit;

namespace CityCongestionCharge.Data.Tests;

public class TestCameraDetectionValidator
{
    [Fact]
    public void Example1_Valid_Detections()
    {
        var detections = new CameraDetection[]
        {
            new(0,   0, "L-123XY", DateTime.Parse("2022-01-11T07:30:00")),
            new(1, 400, "L-123XY", DateTime.Parse("2022-01-11T07:35:00")),
            new(2, 401, "L-123XY", DateTime.Parse("2022-01-11T07:40:00")),
            new(3, 200, "L-123XY", DateTime.Parse("2022-01-11T15:10:00")),
        };

        var validator = new CameraDetectionValidator();
        var errors = validator.ValidateDetections(detections);
        Assert.Empty(errors);
    }

    [Fact]
    public void Example1_Valid_Detections_Multi_Cars()
    {
        var detections = new CameraDetection[]
        {
            new(0,     0, "L-123XY", DateTime.Parse("2022-01-11T07:30:00")),
            new(1,   400, "L-123XY", DateTime.Parse("2022-01-11T07:35:00")),
            new(2,     0, "W-789AB", DateTime.Parse("2022-01-11T08:10:00")),
            new(3,   401, "L-123XY", DateTime.Parse("2022-01-11T08:40:00")),
            new(4,   402, "W-789AB", DateTime.Parse("2022-01-11T09:15:00")),
            new(5,   200, "L-123XY", DateTime.Parse("2022-01-11T15:10:00")),
            new(6,   200, "W-789AB", DateTime.Parse("2022-01-11T19:25:00")),
        };

        var validator = new CameraDetectionValidator();
        var errors = validator.ValidateDetections(detections);
        Assert.Empty(errors);
    }

    [Fact]
    public void Example2_Leaving_Without_Entering()
    {
        var detections = new CameraDetection[]
        {
            new(0, 200, "L-123XY", DateTime.Parse("2022-01-11T14:13:00")),
        };

        var validator = new CameraDetectionValidator();
        var errors = validator.ValidateDetections(detections);
        Assert.Single(errors);
        Assert.Equal(new[] { new DetectionError(0, ErrorType.LeaveNoEnter) }, errors);
    }

    [Fact]
    public void Example3_Driving_Inside_Without_Entering()
    {
        var detections = new CameraDetection[]
        {
            new(0, 400, "L-123XY", DateTime.Parse("2022-01-11T07:35:00")),
            new(1, 401, "L-123XY", DateTime.Parse("2022-01-11T07:40:00")),
        };

        var validator = new CameraDetectionValidator();
        var errors = validator.ValidateDetections(detections);
        Assert.Equal(2, errors.Count());
        Assert.Equal(new[]
        {
            new DetectionError(0, ErrorType.InsideNoEnter),
            new DetectionError(1, ErrorType.InsideNoEnter),
        }, errors);
    }

    [Fact]
    public void Example4_Driving_Inside_After_Leaving()
    {
        var detections = new CameraDetection[]
        {
            new(0,   0, "L-123XY", DateTime.Parse("2022-01-11T07:30:00")),
            new(1, 200, "L-123XY", DateTime.Parse("2022-01-11T15:10:00")),
            new(2, 400, "L-123XY", DateTime.Parse("2022-01-11T15:35:00")),
        };

        var validator = new CameraDetectionValidator();
        var errors = validator.ValidateDetections(detections);
        Assert.Single(errors);
        Assert.Equal(new[]
        {
            new DetectionError(2, ErrorType.InsideAfterExit),
        }, errors);
    }

    [Fact]
    public void Example5_Multiple_Entering()
    {
        var detections = new CameraDetection[]
        {
            new(0, 0, "L-123XY", DateTime.Parse("2022-01-11T07:30:00")),
            new(1, 0, "L-123XY", DateTime.Parse("2022-01-11T07:35:00")),
        };

        var validator = new CameraDetectionValidator();
        var errors = validator.ValidateDetections(detections);
        Assert.Single(errors);
        Assert.Equal(new[]
        {
            new DetectionError(1, ErrorType.AlreadyInside),
        }, errors);
    }

    [Fact]
    public void Example6_Multiple_Leaving()
    {
        var detections = new CameraDetection[]
        {
            new(0,   0, "L-123XY", DateTime.Parse("2022-01-11T07:30:00")),
            new(1, 200, "L-123XY", DateTime.Parse("2022-01-11T14:13:00")),
            new(2, 200, "L-123XY", DateTime.Parse("2022-01-11T15:13:00")),
        };

        var validator = new CameraDetectionValidator();
        var errors = validator.ValidateDetections(detections);
        Assert.Single(errors);
        Assert.Equal(new[]
        {
            new DetectionError(2, ErrorType.LeaveNoEnter),
        }, errors);
    }

    [Fact]
    public void Example7_Valid_License_Plate_Numbers()
    {
        var detections = new CameraDetection[]
        {
            new(0, 0, "L-123AB",  DateTime.Parse("2022-01-11T07:30:00")),
            new(1, 0, "W-12FX",   DateTime.Parse("2022-01-11T07:30:00")),
            new(2, 0, "S-9876X",  DateTime.Parse("2022-01-11T07:30:00")),
            new(3, 0, "LL-1234Y", DateTime.Parse("2022-01-11T07:30:00")),
        };

        var validator = new CameraDetectionValidator();
        var errors = validator.ValidateDetections(detections);
        Assert.Empty(errors);
    }

    [Fact]
    public void Example8_Invalid_License_Plate_Numbers()
    {
        var detections = new CameraDetection[]
        {
            new(0, 0, "LLL-123A",     DateTime.Parse("2022-01-11T07:30:00")),
            new(1, 0, "-123AB",       DateTime.Parse("2022-01-11T07:30:00")),
            new(2, 0, "123AB",        DateTime.Parse("2022-01-11T07:30:00")),
            new(3, 0, "UU-KNIGHT1",   DateTime.Parse("2022-01-11T07:30:00")),
            new(4, 0, "WU-123456789", DateTime.Parse("2022-01-11T07:30:00")),
            new(5, 0, "X",            DateTime.Parse("2022-01-11T07:30:00")),
            new(6, 0, "1",            DateTime.Parse("2022-01-11T07:30:00")),
            new(7, 0, "-",            DateTime.Parse("2022-01-11T07:30:00")),
        };

        var validator = new CameraDetectionValidator();
        var errors = validator.ValidateDetections(detections);
        Assert.Equal(8, errors.Count());
        Assert.All(errors, e => Assert.Equal(ErrorType.InvalidLP, e.ErrorType));
    }

    [Fact]
    public void Example9_Invalid_Camera_ID()
    {
        var detections = new CameraDetection[]
        {
            new(0, -1, "LL-123XY", DateTime.Parse("2022-01-11T07:30:00")),
        };

        var validator = new CameraDetectionValidator();
        Assert.Throws<ArgumentOutOfRangeException>(() => validator.ValidateDetections(detections));
    }

    [Fact]
    public void Example10_Empty_Detection_Collection()
    {
        var validator = new CameraDetectionValidator();
        var errors = validator.ValidateDetections(Array.Empty<CameraDetection>());
        Assert.Empty(errors);
    }
}
