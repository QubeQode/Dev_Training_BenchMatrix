using LibraryManagementAPI.Data;
using LibraryManagementAPI.Exceptions;
using LibraryManagementAPI.Middleware;
using LibraryManagementAPI.Services;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var secret = builder.Configuration["Jwt:SecretKey"]
            ?? throw new InvalidOperationException("JWT Secret not configured.");
        
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)),
            ValidateIssuer = false,
            ValidateAudience = false,
            ClockSkew = TimeSpan.Zero
        };

        options.Events = new JwtBearerEvents
        {
            OnChallenge = async context =>
            {
                context.HandleResponse();

                context.Response.StatusCode = 401;
                context.Response.ContentType = "application/json";

                await context.Response.WriteAsJsonAsync(
                    new
                    {
                        Title = "Unathorized",
                        Status = 401,
                        Detail = "A valid JWT access token is required."
                    }
                );
            },

            OnForbidden = async context =>
            {
                context.Response.StatusCode = 403;
                context.Response.ContentType = "application/json";

                await context.Response.WriteAsJsonAsync(
                    new
                    {
                        Title = "Forbidden",
                        Status = 403,
                        Detail = "You do not have permission to access this resource."
                    }
                );
            }
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddControllers();
/*
    Block no longer needed since DTO design removes circular referencing issue:
            .AddJsonOptions(options =>
            {
            options.JsonSerializerOptions.ReferenceHandler =
                ReferenceHandler.IgnoreCycles;  
            });
*/

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Enter JWT Bearer token.",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };

    options.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {securityScheme, new string[] { }}
    });  
});


/*
    Singletons create a single source of truth for authors + books
    Lists registered as services for ASP.NET to draw from when constructing BookService
        builder.Services.AddSingleton(InMemoryStore.Books);
        builder.Services.AddSingleton(InMemoryStore.Authors);
    No longer necessary with DbContext integration
*/

// DI Container for AppDbContext for ASP.NET to construct DbContext when asked for
builder.Services.AddDbContext<AppDbContext>(options =>
{
    // Register MySQL as the Db provider
    options.UseMySql(
        // Direct builder to connection string "DefaultConnection" in appsettings.json
        builder.Configuration.GetConnectionString("DefaultConnection"),
        // Connect to "DefaultConnection" briefly and detect version of MySQL used
        ServerVersion.AutoDetect(
            builder.Configuration.GetConnectionString("DefaultConnection")
        )
    );
});

// Scoped book service creates instances that can alter this single source of truth
// On HTTP request implicitly creates instance of new BookService(AppDbContext);
builder.Services.AddScoped<IBookService, BookService>();

// Exception handling layer for handling erroneous processes being called in as a service in middleware chain
builder.Services.AddExceptionHandler<CustomExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

// Called at the top of the middleware chain to catch any exceptions that bubble up and stop app from crashing
// Also to convert any and all exceptions into client side friendly, clean status + problem details
app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<RequestLoggingMiddleware>();
app.UseAuthentication(); // Who are you?
app.UseAuthorization(); // Are you allowed to do this?
app.MapControllers();  // Okay do this using the controller
app.Run();
