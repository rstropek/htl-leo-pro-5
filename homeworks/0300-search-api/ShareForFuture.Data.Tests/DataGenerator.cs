namespace ShareForFuture.Data.Tests;

internal static class DataGenerator
{
    public static User CreateUser(bool withGroup = false, string? email = null, bool identities = true)
    {
        return new User
        {
            FirstName = "Foo",
            LastName = "Bar",
            Street = "Anywhere",
            City = "Somehwere",
            ZipCode = "9999",
            Country = "AT",
            ContactPhone = "+4312345678",
            ContactEmail = email ?? "foo@bar.com",
            UserGroupId = withGroup ? 1 : null,
            Identities = identities ? new List<Identity>
            {
                new()
                {
                    Provider = IdentityProvider.Google,
                    SubjectId = "foobar@google"
                },
                new()
                {
                    Provider = IdentityProvider.Microsoft,
                    SubjectId = "foobar@google"
                },
            } : new List<Identity>(),
        };
    }

    public static Offering CreateOffering()
    {
        return new Offering
        {
            User = CreateUser(),
            Title = "Foo Bar",
            Description = "Foo Bar Description",
            Condition = DeviceCondition.Used,
            LastSuccessfullAvailabilityVerification = DateTime.Now,
            SubCategory = new()
            {
                Title = "Foo",
                Category = new()
                {
                    Title = "Bar"
                }
            },
            Tags = new List<OfferingTag>
            {
                new() { Tag = "Foo" },
                new() { Tag = "Bar" },
            },
            Images = new List<DeviceImage>
            {
                new() { Title = "FooBar", ImageData = new byte[] { 1, 2, 3} },
            }
        };
    }

    public static IEnumerable<Offering> CreateSearchOffering()
    {
        var u = CreateUser();
        return new Offering[]
        {
            new()
            {
                User = u,
                Title = "Drilling machine",
                Description = "Makita drilling machine, great for concrete walls",
                Condition = DeviceCondition.Used,
                LastSuccessfullAvailabilityVerification = DateTime.Now,
                SubCategory = new()
                {
                    Title = "Foo",
                    Category = new()
                    {
                        Title = "Bar"
                    }
                },
                Tags = new List<OfferingTag>
                {
                    new() { Tag = "Tool" },
                    new() { Tag = "Drilling" },
                    new() { Tag = "Makita" },
                },
            },
            new()
            {
                User = u,
                Title = "Bosch driller",
                Description = "Drilling machine from Bosch, good for light home improvement projects",
                Condition = DeviceCondition.Used,
                LastSuccessfullAvailabilityVerification = DateTime.Now,
                SubCategory = new()
                {
                    Title = "Foo",
                    Category = new()
                    {
                        Title = "Bar"
                    }
                },
                Tags = new List<OfferingTag>
                {
                    new() { Tag = "Machine" },
                    new() { Tag = "Bosch" },
                    new() { Tag = "Home Improvement" },
                },
            },
            new()
            {
                User = u,
                Title = "Beamer",
                Description = "Full-HD beamer for watching TV",
                Condition = DeviceCondition.Used,
                LastSuccessfullAvailabilityVerification = DateTime.Now,
                SubCategory = new()
                {
                    Title = "Foo",
                    Category = new()
                    {
                        Title = "Bar"
                    }
                },
                Tags = new List<OfferingTag>
                {
                    new() { Tag = "TV" },
                    new() { Tag = "Beamer" },
                    new() { Tag = "Home Cinema" },
                },
            },
        };
    }

    public static Sharing CreateSharing()
    {
        return new Sharing
        {
            Offering = CreateOffering(),
            Borrower = CreateUser(email: "borrower@foobar.com", identities: false),
            From = DateTime.Now.AddDays(1),
            Until = DateTime.Now.AddDays(3)
        };
    }

    public static Complaint CreateComplaint()
    {
        return new Complaint
        {
            Complainer = CreateUser(),
            Complainee = CreateUser(email: "complainee@foobar.com", identities: false),
            AssignedTo = CreateUser(email: "employee@foobar.com", identities: false),
        };
    }
}
