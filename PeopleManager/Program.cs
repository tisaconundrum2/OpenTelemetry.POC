using OpenTelemetry.Exporter;
using OpenTelemetry.Logs;
using OpenTelemetry.Resources;
using PeopleManager.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Register the in-memory people repository as a singleton so data persists across requests.
builder.Services.AddSingleton<PersonRepository>();

// Read OpenTelemetry settings from configuration.
var otlpEndpoint = builder.Configuration["OpenTelemetry:OtlpEndpoint"];
var otlpHeaders = builder.Configuration["OpenTelemetry:OtlpHeaders"];
var serviceName = builder.Configuration["OpenTelemetry:ServiceName"]
    ?? builder.Environment.ApplicationName;

// Add OpenTelemetry logging provider with OTLP exporter (SigNoz).
builder.Services.AddOpenTelemetry()
    .ConfigureResource(r => r.AddService(serviceName))
    .WithLogging(logging => logging
        .AddOtlpExporter(otlp =>
        {
            if (!string.IsNullOrEmpty(otlpEndpoint))
                otlp.Endpoint = new Uri(otlpEndpoint);
            if (!string.IsNullOrEmpty(otlpHeaders))
                otlp.Headers = otlpHeaders;
            otlp.Protocol = OtlpExportProtocol.Grpc;
        }),
        options =>
        {
            // Required so that log message bodies are populated in SigNoz.
            options.IncludeFormattedMessage = true;
            options.IncludeScopes = true;
        });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
