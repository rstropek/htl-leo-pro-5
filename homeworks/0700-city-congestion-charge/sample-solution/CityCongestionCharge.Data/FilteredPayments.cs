using Microsoft.EntityFrameworkCore;

namespace CityCongestionCharge.Data;

public partial class CccDataContext
{
    public IQueryable<Payment> FilteredPayments(PaymentType? paymentTypeFilter,
        bool? onlyFuturePayments, bool? onlyAnonymous)
    {
        IQueryable<Payment> payments = Payments.Include(p => p.Car);

        if (paymentTypeFilter.HasValue)
        {
            payments = payments.Where(p => p.PaymentType == paymentTypeFilter);
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
