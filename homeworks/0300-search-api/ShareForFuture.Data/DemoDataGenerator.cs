namespace ShareForFuture.Data;

using Bogus;
using Bogus.Extensions;
using Microsoft.EntityFrameworkCore;

public class DemoDataGenerator
{
    private readonly S4fDbContext context;

    public DemoDataGenerator(S4fDbContext context)
    {
        this.context = context;
    }

    public async Task ClearAll()
    {
        await context.Database.ExecuteSqlRawAsync(@$"
            DELETE FROM [{nameof(S4fDbContext.UnavailabilityPeriods)}];
            DELETE FROM [{nameof(S4fDbContext.Sharings)}];
            DELETE FROM [{nameof(S4fDbContext.ComplaintNotes)}];
            DELETE FROM [{nameof(S4fDbContext.Complaints)}];
            DELETE FROM [{nameof(S4fDbContext.DeviceImages)}];
            DELETE FROM [{nameof(S4fDbContext.Offerings)}];
            DELETE FROM [OfferingsTags];
            DELETE FROM [{nameof(S4fDbContext.OfferingTags)}];
            DELETE FROM [{nameof(S4fDbContext.DeviceSubCategories)}];
            DELETE FROM [{nameof(S4fDbContext.DeviceCategories)}];
            DELETE FROM [{nameof(S4fDbContext.Identities)}];
            DELETE FROM [{nameof(S4fDbContext.Users)}];
        ");
        context.ChangeTracker.Clear();
    }

    public async Task Generate()
    {
        // User groups
        var empGroups = await context.UserGroups.ToListAsync();

        // Users
        var usersFaker = new Faker<User>()
            .RuleFor(u => u.FirstName, f => f.Name.FirstName())
            .RuleFor(u => u.LastName, f => f.Name.LastName())
            .RuleFor(u => u.Street, f => f.Address.StreetName())
            .RuleFor(u => u.City, f => f.Address.City())
            .RuleFor(u => u.ZipCode, f => f.Address.ZipCode())
            .RuleFor(u => u.Country, f => f.Address.CountryCode())
            .RuleFor(u => u.ContactPhone, f => f.Phone.PhoneNumber())
            .RuleFor(u => u.ContactEmail, (f, u) => f.Internet.Email(u.FirstName, u.LastName))
            .RuleFor(u => u.LastSuccessfullEmailVerification, f => f.Date.Past(2))
            .RuleFor(u => u.LastSuccessfullLogin, f => f.Date.Past(1))
            .RuleFor(u => u.UserGroup, f => f.Random.Bool(0.1f) ? f.PickRandom(empGroups) : null)
            .RuleFor(u => u.Identities, f =>
                new List<Identity>
                {
                    new()
                    { 
                        Provider = f.PickRandom<IdentityProvider>(),
                        SubjectId = f.Random.Uuid().ToString()
                    }
                }
            );
        var users = Enumerable.Range(1, 100)
            .Select(_ => usersFaker.Generate())
            .ToList();
        context.Users.AddRange(users);

        // Device Categories
        var allCategory = new DeviceCategory() { Title = "All" };
        context.DeviceCategories.Add(allCategory);

        // Device Subcategories
        var cat = new[] { "Tools", "Devices", "Helper", "Various", "Anything else" };
        var categoryFaker = new Faker<DeviceSubCategory>()
            .RuleFor(u => u.Category, allCategory)
            .RuleFor(u => u.Title, f => f.PickRandom(cat));
        var categories = Enumerable.Range(1, 10)
            .Select(_ => categoryFaker.Generate())
            .ToList();
        context.DeviceSubCategories.AddRange(categories);

        // Tags
        var tags = new[] { "Useful", "Like new", "Beautiful", "Nice", "Green", "Chair", "Plastic" }
            .Select(t => new OfferingTag { Tag = t })
            .ToArray();
        context.OfferingTags.AddRange(tags);

        // Offerings
        var offeringFaker = new Faker<Offering>()
            .RuleFor(u => u.User, f => users[f.Random.Int(0, users.Count - 1)])
            .RuleFor(u => u.Title, f => f.Commerce.ProductName())
            .RuleFor(u => u.Description, f => f.Commerce.ProductDescription())
            .RuleFor(u => u.Condition, f => f.PickRandom<DeviceCondition>())
            .RuleFor(u => u.LastSuccessfullAvailabilityVerification, f => f.Date.Past(1).OrNull(f, 0.5f))
            .RuleFor(u => u.SubCategory, f => f.PickRandom(categories))
            .RuleFor(u => u.Tags, f => tags[..f.Random.Int(1, 4)].ToList());
        var offerings = Enumerable.Range(1, 1000)
            .Select(_ => offeringFaker.Generate())
            .ToList();
        context.Offerings.AddRange(offerings);

        await context.SaveChangesAsync();
    }
}
