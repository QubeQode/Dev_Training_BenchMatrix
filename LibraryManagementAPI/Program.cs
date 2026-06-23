using LibraryManagementAPI.Data;
using LibraryManagementAPI.Exceptions;
using LibraryManagementAPI.Middleware;
using LibraryManagementAPI.Services;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
      options.JsonSerializerOptions.ReferenceHandler =
        ReferenceHandler.IgnoreCycles;  
    });

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

// Singletons create a single source of truth for authors + books
// Lists registered as services for ASP.NET to draw from when constructing BookService
builder.Services.AddSingleton(InMemoryStore.Books);
builder.Services.AddSingleton(InMemoryStore.Authors);

// Scoped book service creates instances that can alter this single source of truth
// On HTTP request implicitly creates instance of new BookService(InMemoryStore.Books, InMemoryStore.Authors);
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
app.MapControllers();
app.Run();
