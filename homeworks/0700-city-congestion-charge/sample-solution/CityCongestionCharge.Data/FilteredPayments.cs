using Microsoft.EntityFrameworkCore;

namespace CityCongestionCharge.Data;

public partial class CccDataContext
{
    public IQueryable<Payment> FilteredPayments(string? licensePlateFilter,
        bool? onlyFuturePayments, bool? onlyAnonymous)
    {
        IQueryable<Payment> payments = Payments.Include(p => p.Car);

        if (licensePlateFilter != null)
        {
            payments = payments.Where(p => p.Car!.LicensePlate.Contains(licensePlateFilter));
        }

        if (onlyFuturePayments is true)
        {
            payments = payments.Where(p => p.PaidForDate > DateTime.Today);
        }

        if (onlyAnonymous is true)
        {
            payments = payments.Where(p => p.PayingPerson == null);
        }

        return payments;
    }
}
