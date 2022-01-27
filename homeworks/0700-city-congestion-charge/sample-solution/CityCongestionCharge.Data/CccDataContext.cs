using Microsoft.EntityFrameworkCore;

namespace CityCongestionCharge.Data;

public partial class CccDataContext
{
    public IQueryable<Detection> FilteredDetections(CarType? carType, string? licensePlateFilter, bool? onlyInside)
    {
        IQueryable<Detection> detections = Detections;

        if (carType.HasValue || !string.IsNullOrEmpty(licensePlateFilter))
        {
            detections = detections.Where(d => d.DetectedCars.Any(c => 
                (!carType.HasValue || c.CarType == carType)
                && (string.IsNullOrEmpty(licensePlateFilter) || c.LicensePlate.Contains(licensePlateFilter))));
        }

        if (onlyInside is true)
        {
            detections = detections.Where(d => d.MovementType == MovementType.DrivingInside);
        }

        return detections.Include(d => d.DetectedCars);
    }
}
