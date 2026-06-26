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

// Using DI to register auth services with auth scheme "Bearer" - Teaches ASP.NET to handle [Authorize]
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)

    // Register JWT Bearer Handler and configure how auth works
    .AddJwtBearer(options =>
    {
        // Read secret key from config in appsettings.json
        var secret = builder.Configuration["Jwt:SecretKey"]
            ?? throw new InvalidOperationException("JWT Secret not configured.");
        
        // Configure token validation so ASP.NET knows exactly how to verify incoming JWTs
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true, // Verify token signed with expected key
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)), // Used to verify JWT Signature
            ValidateIssuer = false,
            ValidateAudience = false,
            ClockSkew = TimeSpan.Zero // No tolerance after token expires
        };

        // Configure Authentication Events
        options.Events = new JwtBearerEvents
        {
            // Runs when authentication fails - customize output
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

            // Runs when authorization fails but authentication succeeds - customize output
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
    }
);

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

// Configure swagger generation
builder.Services.AddSwaggerGen(options =>
{
    // Define a security scheme - how should clients authenticate when calling this API?
    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization", // Header name that holds authentication value
        Description = "Enter JWT Bearer token.",
        In = ParameterLocation.Header, // Should be sent in a HTTP header
        Type = SecuritySchemeType.Http, // Auth mechanism is part of HTTP protocol
        Scheme = "bearer",
        BearerFormat = "JWT",
        Reference = new OpenApiReference // Give security scheme a uniquely named resuable component
        {
            Id = JwtBearerDefaults.AuthenticationScheme, // Resolves to "bearer"
            Type = ReferenceType.SecurityScheme // Tells swagger reference points to a security scheme
        }
    };

    // Register security scheme to the OpenAPI document
    options.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
    // make registered security scheme required - endpoints might use this security scheme
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {securityScheme, new string[] { }} // Use defined security scheme
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
