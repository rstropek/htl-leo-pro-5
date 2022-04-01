using Microsoft.EntityFrameworkCore;
using WoerdleSolver.Logic;
using WoerdleSolver.WebApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddDbContext<SolverContext>(
    options => options.UseSqlServer(builder.Configuration["ConnectionStrings:DefaultConnection"]))
    .AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<ISolverFactory, SolverFactory>();

builder.Services.AddCors();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(builder => builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
app.UseAuthorization();
app.MapControllers();
app.Run();
