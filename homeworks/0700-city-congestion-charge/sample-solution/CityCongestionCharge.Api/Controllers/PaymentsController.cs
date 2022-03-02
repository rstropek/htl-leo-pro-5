namespace BikeTogether.Api.Controllers;

/// <summary>
/// Web API for accessing payments
/// </summary>
[Route("api/[controller]")]
[ApiController]
[Produces("application/json")]
public class PaymentsController : ControllerBase
{
    private readonly CccDataContext context;

    /// <summary>
    /// Initializes a new instance of the <see cref="PaymentsController"/> class.
    /// </summary>
    /// <param name="context">Entity Framework data context</param>
    public PaymentsController(CccDataContext context)
    {
        this.context = context;
    }

    #region Data transfer objects
    /// <summary>
    /// DTO describing a payment
    /// </summary>
    public class PaymentResultDto
    {
        public int Id { get; set; }
        public DateTime PaidForDate { get; set; }
        public decimal PaidAmount { get; set; }
        public string? PayingPerson { get; set; }
        public PaymentType PaymentType { get; set; }
        public string LicensePlate { get; set; } = string.Empty;
    }

    /// <summary>
    /// Converts a given <see cref="Payment"/> into a <see cref="PaymentResultDto"/>
    /// </summary>
    private static PaymentResultDto PaymentToResultDto(Payment p)
        => new()
        {
            Id = p.Id,
            PaidAmount = p.PaidAmount,
            PayingPerson = p.PayingPerson,
            PaidForDate = p.PaidForDate,
            PaymentType = p.PaymentType,
            LicensePlate = p.Car!.LicensePlate,
        };

    public class DetectionDetailDto
    {
        public DateTime Taken { get; set; }

        public bool MultipleCarsOnOneDetection { get; set; }
    }

    public class PaymentWithDetectionDto
    {
        public int PaymentId { get; set; }

        public decimal PaidAmount { get; set; }

        public string LicensePlate { get; set; } = string.Empty;

        public IEnumerable<DetectionDetailDto>? DetectionDetails { get; set; }
    }

    public class PaymentAddDto
    {
        public decimal PaidAmount { get; set; }
        public string? PayingPerson { get; set; }
        public PaymentType PaymentType { get; set; }
        public string LicensePlate { get; set; } = string.Empty;
    }
    #endregion

    /// <summary>
    /// Gets a filtered list of payments
    /// </summary>
    /// <param name="paymentTypeFilter">Optional filter for <see cref="Payment.PaymentType"/></param>
    /// <param name="onlyFuturePayments">Optional filter for getting only payments regarding days in the future</param>
    /// <param name="onlyAnonymous">Optional filter for getting only anonymous payments (i.e. payments without <see cref="Payment.PayingPerson"/>)</param>
    /// <returns>
    /// List of payments fulfilling the given filter parameters.
    /// </returns>
    [HttpGet(Name = nameof(GetPayments))]
    public ActionResult<IEnumerable<PaymentResultDto>> GetPayments(
        [FromQuery(Name = "type")] PaymentType? paymentTypeFilter,
        [FromQuery(Name = "future")] bool? onlyFuturePayments,
        [FromQuery(Name = "anonym")] bool? onlyAnonymous)
    {
        return Ok(context.FilteredPayments(paymentTypeFilter, onlyFuturePayments, onlyAnonymous)
            .Select(p => PaymentToResultDto(p)));
    }

    /// <summary>
    /// Gets a list of payments for days with detections
    /// </summary>
    /// <returns>
    /// List of payments for days with car detections
    /// </returns>
    [HttpGet("with-detections", Name = nameof(GetPaymentsWithDetections))]
    public ActionResult<IEnumerable<PaymentWithDetectionDto>> GetPaymentsWithDetections()
    {
        return Ok(context.Payments
            .Where(p => p.Car!.Detections.Any(d => d.Taken.Year == p.PaidForDate.Year
                && d.Taken.Month == p.PaidForDate.Month && d.Taken.Day == p.PaidForDate.Day))
            .OrderBy(p => p.Car!.LicensePlate)
            .Select(p => new PaymentWithDetectionDto
            {
                PaymentId = p.Id,
                LicensePlate = p.Car!.LicensePlate,
                PaidAmount = p.PaidAmount,
                DetectionDetails = p.Car.Detections
                    .OrderBy(d => d.Taken)
                    .Select(d => new DetectionDetailDto
                    {
                        Taken = d.Taken,
                        MultipleCarsOnOneDetection = d.DetectedCars.Count > 1
                    })
            }));
    }

    /// <summary>
    /// Adds a payment
    /// </summary>
    /// <param name="p">Payment to add</param>
    /// <returns>Data about the created payment (including primary key)</returns>
    /// <response code="201">Payment created</response>
    /// <response code="404">No car with given license plate exists</response>
    [HttpPost(Name = nameof(AddPayment))]
    [ProducesResponseType(typeof(PaymentResultDto), StatusCodes.Status201Created)]
    public async Task<ActionResult<PaymentResultDto>> AddPayment(PaymentAddDto p)
    {
        var car = await context.Cars.FirstOrDefaultAsync(c => c.LicensePlate == p.LicensePlate);
        if (car == null) { return NotFound(); }

        var payment = new Payment
        {
            PaidAmount = p.PaidAmount,
            PaidForDate = DateTime.Today,
            PayingPerson = p.PayingPerson,
            PaymentType = p.PaymentType,
            Car = car
        };
        context.Payments.Add(payment);
        await context.SaveChangesAsync();
        return CreatedAtRoute(nameof(GetPaymentById), new { id = payment.Id }, PaymentToResultDto(payment));
    }

    /// <summary>
    /// Get a single payment by ID
    /// </summary>
    /// <param name="id">Id of the payment to get</param>
    /// <response code="200">Payment found</response>
    /// <response code="404">Payment not found</response>
    [HttpGet("{id}", Name = nameof(GetPaymentById))]
    [ProducesResponseType(typeof(PaymentResultDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<PaymentResultDto>> GetPaymentById(int id)
    {
        var payment = await context.Payments.AsNoTracking()
            .Include(p => p.Car)
            .FirstOrDefaultAsync(p => p.Id == id);
        if (payment == null) { return NotFound(); }
        return Ok(PaymentToResultDto(payment));
    }
}
