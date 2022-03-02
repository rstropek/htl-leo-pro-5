using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CityCongestionCharge.Data;

public enum CarType
{
    PassengerCar,
    Motorcycle,
    Lorry,
    Van,
}

public class Owner
{
    public int Id { get; set; }

    [MaxLength(50)]
    public string FirstName { get; set; } = string.Empty;

    [MaxLength(50)]
    public string LastName { get; set; } = string.Empty;

    [MaxLength(100)]
    public string Address { get; set; } = string.Empty;

    public List<Car> Cars { get; set; } = new();
}

[Index(nameof(LicensePlate), IsUnique = true)]
public class Car
{
    public int Id { get; set; }

    [MaxLength(10)]
    public string LicensePlate { get; set; } = string.Empty;

    [MaxLength(50)]
    public string Make { get; set; } = string.Empty;

    [MaxLength(50)]
    public string? Model { get; set; } = string.Empty;

    [MaxLength(50)]
    public string Color { get; set; } = string.Empty;

    public CarType CarType { get; set; }

    public bool IsElectricOrHybrid { get; set; }

    public Owner? Owner { get; set; }

    public int OwnerId { get; set; }

    public List<Payment> Payments { get; set; } = new();

    public List<Detection> Detections { get; set; } = new();
}

public enum PaymentType
{
    Cash,
    BankTransfer,
    CreditCard,
    DebitCard,
}

public class Payment
{
    public int Id { get; set; }

    public DateTime PaidForDate { get; set; }

    [Precision(8, 2)]
    public decimal PaidAmount { get; set; }

    [MaxLength(100)]
    public string? PayingPerson { get; set; }

    public PaymentType PaymentType { get; set; } = PaymentType.Cash;

    public Car? Car { get; set; }

    public int CarId { get; set; }
}

public enum MovementType
{
    Entering,
    Leaving,
    DrivingInside,
}

public class Detection
{
    public int Id { get; set; }

    public DateTime Taken { get; set; }

    [MinLength(10)]
    [MaxLength(200)]
    public string PhotoUrl { get; set; } = string.Empty;

    public MovementType MovementType { get; set; }

    public List<Car> DetectedCars { get; set; } = new();
}
