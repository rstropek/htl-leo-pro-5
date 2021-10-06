namespace ShareForFuture.Data.Tests;

internal static class DataGenerator
{
    public static User CreateUser(bool withGroup = false)
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
            ContactEmail = "foo@bar.com",
            UserGroupId = withGroup ? 1 : null,
            Identities = new List<Identity>
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
            },
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

    //public static Offering CreateSharing()
    //{
    //    return new Sharing
    //    {
    //        Offering = CreateOffering(),

    //    };
    //}
}
