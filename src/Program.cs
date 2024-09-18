using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor.Services;

using app.Components;
using app.Infrastructure;

using app.Core.Model;
using app.Services;
using app.Utilities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlite(builder.Configuration.GetConnectionString("Default")
        ?? throw new InvalidOperationException("Connection string not found.")));
}
else
{
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("Default")
        ?? throw new InvalidOperationException("Connection string not found.")));
}
Console.WriteLine("Using ConnectionString: " + builder.Configuration.GetConnectionString("Default"));

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddMudServices();
builder.Services.AddHealthChecks();

builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<SumService>();
builder.Services.AddScoped<HelloService>();

// Create a service to expose ActivitySource, and Metric Instruments
// for manual instrumentation
builder.Services.AddSingleton<Instrumentation>();

// TODO
// Configure OpenTelemetry tracing & metrics with auto-start using the
// AddOpenTelemetry extension from OpenTelemetry.Extensions.Hosting

var app = builder.Build();

using var scope = app.Services.CreateScope();
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationDbContext>();
    context.Database.Migrate();
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
app.UseAntiforgery();
app.MapHealthChecks("/healthz");
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();
app.Logger.LogInformation("Starting the app");
app.Run();
