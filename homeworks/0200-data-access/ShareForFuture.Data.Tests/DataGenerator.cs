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
