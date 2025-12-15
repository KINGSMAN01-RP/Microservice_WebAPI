using EnquiryMaster_WebAPI.Data;
using EnquiryMaster_WebAPI.Repositories.EnquiryMaster;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//Add DbContext 
builder.Services.AddDbContext<EnquiryDbContext>( options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//Add Custom Repositories Services
builder.Services.AddScoped<IEnquiryRepository, EnquiryRepository>();


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Add Health Checks
builder.Services.AddHealthChecks()
    .AddSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
    .AddCheck("CustomCheck", () =>
    {
        bool condition = true;
        return condition ? HealthCheckResult.Healthy() : HealthCheckResult.Unhealthy();
    });


// Register HealthCheck UI
builder.Services.AddHealthChecksUI(options =>
{
    options.SetEvaluationTimeInSeconds(10); // check every 10 sec
    options.MaximumHistoryEntriesPerEndpoint(60);
    options.AddHealthCheckEndpoint("EnquiryService Health", "/health");
})
.AddInMemoryStorage();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();


// Map Health Check endpoint
app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.MapHealthChecksUI(options =>
{
    options.UIPath = "/health-ui";          // Dashboard route
    options.ApiPath = "/health-json";       // JSON Endpoint
});


app.Run();
