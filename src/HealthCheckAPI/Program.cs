using HealthCheckAPI.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHealthChecks()
    .AddCheck("Health_01", new HealthCheck("https://bing.com", 500L))
    .AddCheck("Health_02", new HealthCheck("https://google.com", 500L))
    .AddCheck("Health_03", new HealthCheck("https://10.0.0.0", 500L));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHealthChecks(new PathString("/health"), new CustomHealthCheckOptions());

app.UseAuthorization();

app.MapControllers();

app.Run();
