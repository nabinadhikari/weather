using Weather.Api.RateLimit;
using Weather.Contracts.CONFIG;
using Weather.Data;
using Weather.RequestRecordData;

var builder = WebApplication.CreateBuilder(args);
var allowSpecificOrigins = "allowSpecificOrigins";

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var webConfig = builder.Configuration.GetSection("WebConfig").Get<WebConfig>();
var weatherConfig = builder.Configuration.GetSection("OpenWeatherMapConfig").Get<WeatherServiceConfig>();
builder.Services.AddSingleton(m => webConfig);
builder.Services.AddSingleton(m => weatherConfig);
builder.Services.AddSingleton<IWeatherService, WeatherService>();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSingleton<IRequestRecordService, RequestRecordService>();


builder.Services.AddCors(options =>
{
    options.AddPolicy(name: allowSpecificOrigins, policy =>
    {
        policy
        .AllowAnyMethod()
        .AllowAnyHeader()
        .WithOrigins(webConfig.CorsAllowedOrigins);
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(allowSpecificOrigins);

app.UseAuthorization();
app.UseRateLimiterMiddleware();
app.MapControllers();

app.Run();
