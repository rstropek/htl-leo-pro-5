using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

// Note that this sample uses the new ASP.NET Core Minimal Web API
// programming model. Read more at https://www.hanselman.com/blog/exploring-a-minimal-web-api-with-aspnet-core-6.

var builder = WebApplication.CreateBuilder(args);

#region Setup dependency injection
var services = builder.Services;

// Add and configure bearer auth.
// Read more at https://auth0.com/docs/quickstart/backend/aspnet-core-webapi#validate-access-tokens.
services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = $"https://{builder.Configuration["Auth0:Domain"]}/";
        options.Audience = builder.Configuration["Auth0:Audience"];
        // If the access token does not have a `sub` claim, `User.Identity.Name` will be `null`.
        // Map it to a different claim by setting the NameClaimType below.
        options.TokenValidationParameters = new TokenValidationParameters
        {
            NameClaimType = ClaimTypes.NameIdentifier
        };
    });

// Add authorization.
// We use Auth0's RBAC to distinguish between regular users and admins.
// For more information about Auth0 RBAC see https://auth0.com/docs/authorization/rbac.
services.AddAuthorization(options =>
    options.AddPolicy("AdminsOnly", policy => policy.RequireClaim("permissions", "administrate"))
);

// Add CORS. We need that because API is consumed by SPA (Angular).
services.AddCors();
#endregion

#region Setup middlewares for request processing
var app = builder.Build();

// Show detailed exception information as this is a sample
// for learning certain technologies
app.UseDeveloperExceptionPage();

// Enable CORS and HTTPS redirection
app.UseCors(options => options.AllowAnyMethod().AllowAnyOrigin().AllowAnyHeader());
app.UseHttpsRedirection();

// Enable auth
app.UseAuthentication();
app.UseAuthorization();

// Add thre demo endpoints. One is available without a token.
// On accepts any token. The final one requires a token with a
// specific permission.
app.Map("/public", context => context.Response.WriteAsync("\"Hello World!\""));
app.Map("/private", [Authorize] (context) => context.Response.WriteAsync("\"Hello Private World!\""));
app.Map("/admin", [Authorize(Policy = "AdminsOnly")] (context) => context.Response.WriteAsync("\"Hello Admin World!\""));
#endregion

app.Run();
