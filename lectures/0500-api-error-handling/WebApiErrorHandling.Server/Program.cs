using Microsoft.AspNetCore.Mvc;
using Serilog;
using Serilog.Events;
using System.ComponentModel.DataAnnotations;
using WebApiErrorHandling.Server.Controllers;

// Setup serilog in a two-step process. First, we configure basic logging
// to be able to log errors during ASP.NET Core startup. Later, we read
// log settings from appsettings.json. Read more at
// https://github.com/serilog/serilog-aspnetcore#two-stage-initialization.
// General information about serilog can be found at
// https://serilog.net/
Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .CreateBootstrapLogger();
try
{
    Log.Information("Starting web host");

    var builder = WebApplication.CreateBuilder(args);

    // Full setup of serilog. We read log settings from appsettings.json
    builder.Host.UseSerilog((context, services, configuration) => configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext());

    // Add services to the container.
    builder.Services.AddControllers();
    builder.Services.AddCors();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    app.UseSerilogRequestLogging(configure =>
    {
        configure.MessageTemplate = "HTTP {RequestMethod} {RequestPath} ({UserId}) responded {StatusCode} in {Elapsed:0.0000}ms";
    }); // We want to log all HTTP requests
    app.UseHttpsRedirection();
    app.UseCors(builder => builder.AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin());
    app.UseAuthorization();
    if (!app.Environment.IsDevelopment())
    {
        // Do not add exception handler for dev environment. In dev,
        // we get the developer exception page with detailed error info.
        app.UseExceptionHandler(errorApp =>
        {
            // Logs unhandled exceptions. For more information about all the
            // different possibilities for how to handle errors see
            // https://docs.microsoft.com/en-us/aspnet/core/fundamentals/error-handling?view=aspnetcore-5.0
            errorApp.Run(async context =>
            {
                // Return machine-readable problem details. See RFC 7807 for details.
                // https://datatracker.ietf.org/doc/html/rfc7807#page-6
                var pd = new ProblemDetails
                {
                    Type = "https://demo.api.com/errors/internal-server-error",
                    Title = "An unrecoverable error occurred",
                    Status = StatusCodes.Status500InternalServerError,
                    Detail = "This is a demo error used to demonstrate problem details",
                };
                pd.Extensions.Add("RequestId", context.TraceIdentifier);
                await context.Response.WriteAsJsonAsync(pd, pd.GetType(), null, contentType: "application/problem+json");
            });
        });
    }

    app.MapControllers();

    app.MapGet("/ping", () => "pong");

    app.MapGet("/request-context", (IDiagnosticContext diagnosticContext) =>
    {
        // You can enrich the diagnostic context with custom properties.
        // They will be logged with the HTTP request.
        diagnosticContext.Set("UserId", "someone");
    });

    app.MapPost("/minimal-customers", (Serilog.ILogger logger, CustomerDtoClass customer) =>
    {
        var context = new ValidationContext(customer, null, null);
        var results = new List<ValidationResult>();
        if (!Validator.TryValidateObject(customer, context, results, true))
        {
            var pd = new ProblemDetails
            {
                Type = "https://demo.api.com/errors/validation-error",
                Title = "Validation Error",
                Status = StatusCodes.Status400BadRequest,
            };
            pd.Extensions.Add("RequestId", results);
            return Results.Json(pd, contentType: "application/problem+json", statusCode: StatusCodes.Status400BadRequest);
        }

        logger.Information("Writing customer {CustomerName} to DB", customer.Name);

        return Results.StatusCode(StatusCodes.Status201Created);
    });

    app.MapGet("/", () => Results.Text(@"<html lang=""en""><body>
        <ul>
            <li><a href=""ping"">Ping</a></li>
            <li><a href=""request-context"">Add context to request</a></li>
            <li><a href=""logDemo/simple"">Some simple log messages</a></li>
            <li><a href=""logDemo/exception"">Handled exception</a></li>
            <li><a href=""logDemo/timing"">Log timing</a></li>
            <li><a href=""logDemo/unhandled-exception"">Unhandled exception</a></li>
        </ul></body></html>", "text/html"));

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
    return 1;
}
finally
{
    Log.CloseAndFlush();
}

return 0;
