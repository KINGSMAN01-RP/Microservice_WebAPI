var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



// Add HealthChecks UI
builder.Services.AddHealthChecksUI()
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


// Map HealthChecks UI dashboard
app.MapHealthChecksUI(options =>
{
    options.UIPath = "/health-ui"; // Dashboard URL
});


app.Run();
