using Bogus;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Json;

namespace CityCongestionCharge.Data;

/// <summary>
/// Helper class for gathering generated demo data
/// </summary>
public class DemoData
{
    /// <summary>
    /// List of demo owners
    /// </summary>
    public List<Owner> Owners { get; set; } = new();

    /// <summary>
    /// List of demo cars
    /// </summary>
    public List<Car> Cars { get; set; } = new();

    /// <summary>
    /// List of demo payments
    /// </summary>
    public List<Payment> Payments { get; set; } = new();

    /// <summary>
    /// List of demo detections
    /// </summary>
    public List<Detection> Detections { get; set; } = new();
}

/// <summary>
/// Generator for random demo data
/// </summary>
public class DemoDataGenerator
{
    /// <summary>
    /// Specifies how many owners should be generated
    /// </summary>
    private const int NUMBER_OF_OWNERS = 100;

    /// <summary>
    /// Specifies how many cars should be generated
    /// </summary>
    private const int NUMBER_OF_CARS = 50;

    private record MakeModel(string Make, string Model);

    /// <summary>
    /// Generate demo data
    /// </summary>
    /// <param name="withIds">Indicates whether ID fields should be filled with sequential numbers</param>
    /// <remarks>
    /// If <paramref name="withIds"/> is set to false, all ID fields are filled with zeroes.
    /// </remarks>
    public DemoData Generate(bool withIds = false)
    {
        var result = new DemoData();

        var ownerIds = 0;
        var testOwners = new Faker<Owner>()
            .RuleFor(u => u.Id, _ => withIds ? ++ownerIds : 0)
            .RuleFor(u => u.FirstName, f => f.Name.FirstName())
            .RuleFor(u => u.LastName, f => f.Name.LastName())
            .RuleFor(u => u.Address, f => $"{f.Address.StreetAddress()}, {f.Address.City()}");
        result.Owners = testOwners.Generate(NUMBER_OF_OWNERS);

        var makeModels = new[]
        {
            new MakeModel("VW", "Golf"),
            new MakeModel("VW", "Polo"),
            new MakeModel("Toyota", "Prius"),
            new MakeModel("BMW", "i3"),
            new MakeModel("Tesla", "Model S"),
            new MakeModel("Tesla", "Model X"),
        };
        var makes = makeModels!.Select(mm => mm.Make).Distinct().ToArray();

        var rand = new Random();

        var carIds = 0;
        var testCars = new Faker<Car>()
            .RuleFor(c => c.Id, _ => withIds ? ++carIds : 0)
            .RuleFor(c => c.LicensePlate, f => $"{f.Random.AlphaNumeric(2).ToUpperInvariant()}-{f.Random.Number(10, 99)}{f.Random.AlphaNumeric(2).ToUpperInvariant()}")
            .RuleFor(c => c.Make, _ => makes.Skip(rand.Next(makes.Length - 1)).First())
            .RuleFor(c => c.Owner, f => f.PickRandom(testOwners))
            .RuleFor(c => c.Model, (_, dc) =>
            {
                var models = makeModels!.Where(mm => mm.Make == dc.Make).ToArray();
                return models.Skip(rand.Next(models.Length - 1)).First().Model;
            })
            .RuleFor(c => c.Color, f => f.Commerce.Color())
            .RuleFor(c => c.CarType, f => f.PickRandom<CarType>());
        result.Cars = testCars.Generate(NUMBER_OF_CARS);

        var testDetections = new List<Detection>();
        foreach(var car in result.Cars)
        {
            var enter = new Detection()
            {
                MovementType = MovementType.Entering,
                PhotoUrl = "https://somecar.com/photo.jpg",
                Taken = DateTime.Now.AddDays(rand.Next(10))
            };
            enter.DetectedCars.Add(car);
            car.Detections.Add(enter);
            testDetections.Add(enter);

            for (var i = 0; i < 5; i++)
            {
                var inside = new Detection()
                {
                    MovementType = MovementType.DrivingInside,
                    PhotoUrl = "https://somecar.com/photo.jpg",
                    Taken = enter.Taken.AddMinutes(rand.Next(120)),
                };
                inside.DetectedCars.Add(car);
                for (var j = rand.Next(5); j > 0; j--)
                {
                    inside.DetectedCars.Add(result.Cars[rand.Next(result.Cars.Count)]);
                    car.Detections.Add(inside);
                }

                car.Detections.Add(inside);
                testDetections.Add(inside);
            }

            var leave = new Detection()
            {
                MovementType = MovementType.Leaving,
                PhotoUrl = "https://somecar.com/photo.jpg",
                Taken = enter.Taken.AddHours(2),
            };
            leave.DetectedCars.Add(car);
            car.Detections.Add(leave);
            testDetections.Add(leave);
        }

        result.Detections = testDetections;

        return result;
    }
}

/// <summary>
/// Writer writing generated demo data to the DB
/// </summary>
public class DemoDataWriter
{
    private readonly CccDataContext context;
    private readonly DemoDataGenerator generator;

    /// <summary>
    /// Initializes a new instance of the <see cref="DemoDataWriter"/> class
    /// </summary>
    /// <param name="context">Target database context</param>
    /// <param name="generator">Generator used to generate demo data</param>
    public DemoDataWriter(CccDataContext context, DemoDataGenerator generator)
    {
        this.context = context;
        this.generator = generator;
    }

    /// <summary>
    /// Deletes all data from the database
    /// </summary>
    public async Task ClearAll()
    {
        await context.Database.ExecuteSqlRawAsync(@$"
            DELETE FROM [{nameof(CccDataContext.Detections)}];
            DELETE FROM [{nameof(CccDataContext.Payments)}];
            DELETE FROM [{nameof(Car)}{nameof(Detection)}];
            DELETE FROM [{nameof(CccDataContext.Cars)}];
            DELETE FROM [{nameof(CccDataContext.Owners)}];
        ");
        context.ChangeTracker.Clear();
    }

    /// <summary>
    /// Fills the database with generated demo data
    /// </summary>
    public async Task Fill()
    {
        var data = generator.Generate();

        // Take generated demo data and write it to DB.
        context.Owners.AddRange(data.Owners);
        context.Cars.AddRange(data.Cars);
        context.Payments.AddRange(data.Payments);
        context.Detections.AddRange(data.Detections);
        await context.SaveChangesAsync();
    }
}
