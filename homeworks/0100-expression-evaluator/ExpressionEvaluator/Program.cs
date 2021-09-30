using ExpressionEvaluation;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

// Make internal class available to unit tests
[assembly: InternalsVisibleTo("ExpressionEvaluator.Tests")]

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<IExpressionEvaluator, ExpressionEvaluator>();
var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();

var regex = new Regex(@"^((\d+)|([a-zA-Z_]+))([+-]((\d+)|([a-zA-Z_]+)))*$");

// Use concurrent dict because HTTP requests can come in concurrently
var variables = new ConcurrentDictionary<string, int>();

app.MapPost("/api/evaluate", (EvaluateBody body, IExpressionEvaluator eval) =>
{
    if (!regex.IsMatch(body.Expression))
    {
        return Results.BadRequest("Invalid expression");
    }

    var result = eval.Evaluate(body.Expression, variables);
    return Results.Ok(new EvaluateResult(body.Expression, result));
});

app.MapPost("/api/variables", (SetVariableBody body) =>
{
    if (string.IsNullOrEmpty(body.Name))
    {
        return Results.BadRequest("Variable name must not be empty");
    }

    variables[body.Name] = body.Value;
    return Results.Ok(body);
});

app.Run();

record EvaluateBody(string Expression);

record EvaluateResult(string Expression, int Result);

record SetVariableBody(string Name, int Value);
