using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Data.Sqlite;
using System.Data.Common;

using MudBlazor.Services;

using app.Components;
using app.Infrastructure;

using app.Core.Model;
using app.Services;
using app.Utilities;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

// Add services to the container.

var connectionString = builder.Configuration.GetConnectionString("Default")
    ?? throw new InvalidOperationException("Connection string not found.");

services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
services.AddDatabaseDeveloperPageExceptionFilter();

Console.WriteLine("Using ConnectionString: " + connectionString);

services.AddRazorComponents().AddInteractiveServerComponents();
services.AddMudServices();
services.AddHealthChecks();

// Our services
services.AddScoped<ProductService>();
services.AddScoped<SumService>();
services.AddScoped<HelloService>();

// OTEL : Create a service to expose ActivitySource, and Metric Instruments
// for manual instrumentation
services.AddSingleton<Instrumentation>();

// TODO
// Configure OpenTelemetry tracing & metrics with auto-start using the
// AddOpenTelemetry extension from OpenTelemetry.Extensions.Hosting

var app = builder.Build();

using var scope = app.Services.CreateScope();
InitializeDatabase(scope);

void InitializeDatabase(IServiceScope scope)
{
    var serviceProvider = scope.ServiceProvider;
    var context = serviceProvider.GetRequiredService<ApplicationDbContext>();

    context.Database.EnsureCreated();
    SeedData.Initialize(context);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
//app.UseAuthorization();
app.UseAntiforgery();
app.MapHealthChecks("/healthz");
app.MapRazorComponents<App>().AddInteractiveServerRenderMode();
app.Logger.LogInformation("Starting the app at {time}", DateTime.Now);
app.Run();

public partial class Program { }
