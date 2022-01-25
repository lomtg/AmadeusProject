using EducationalProject;
using EducationalProject.BackgroundServices;
using EducationalProject.MiddlewareRegistration;
using EducationalProject.Options;
using EducationalProject.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddHttpClient<AccessTokenProviderService>(client =>
            { 
            client.BaseAddress = new Uri("https://test.api.amadeus.com");
            client.Timeout = new TimeSpan(0, 0, 5);
            });

builder.Services.Configure<ServiceAvailableOptions>(builder.Configuration.GetSection("AppServices"));
builder.Services.Configure<AccessTokenOptions>(builder.Configuration.GetSection("Tokens"));
builder.Services.Configure<AmadeusAPIOptions>(builder.Configuration.GetSection("AmadeusAPI"));

builder.Services.AddHostedService<AccessTokenService>();

builder.Services.AddScoped<FlightService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.CheckServiceAvailablity();

app.MapControllers();

app.Run();
