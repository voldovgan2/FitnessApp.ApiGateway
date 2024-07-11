using System.Reflection;
using FitnessApp.ApiGateway;
using FitnessApp.ApiGateway.Configuration;
using FitnessApp.ApiGateway.Extensions;
using FitnessApp.ApiGateway.Middleware;
using FitnessApp.ApiGateway.Services.Aggregator;
using FitnessApp.ApiGateway.Services.Authorization;
using FitnessApp.ApiGateway.Services.InternalClient;
using FitnessApp.ApiGateway.Services.UserIdProvider;
using FitnessApp.Common.Configuration;
using FitnessApp.Common.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDistributedRedisCache(option =>
{
    option.Configuration = builder.Configuration["Redis:Configuration"];
});

builder.Services.ConfigureMapper(new MappingProfile());

var apiAuthenticationSettings = builder.Configuration.GetSection("ApiAuthenticationSettings");
builder.Services.Configure<ApiAuthenticationSettings>(apiAuthenticationSettings);

builder.Services.ConfigureNats(builder.Configuration);

builder.Services.AddTransient<ITokenClient, TokenClient>();

builder.Services.AddTransient<ITokenProvider, TokenProvider>();

builder.Services.AddTransient<IInternalClient, InternalClient>();

builder.Services.AddTransient<IUserIdProvider, UserIdProvider>();

builder.Services.AddSettingsService(builder.Configuration);

builder.Services.AddUserProfileService(builder.Configuration);

builder.Services.AddContactsService(builder.Configuration);

builder.Services.AddFoodService(builder.Configuration);

builder.Services.AddExercisesService(builder.Configuration);

builder.Services.AddNotificationService();

builder.Services.ConfigureVault(builder.Configuration);

builder.Services.AddTransient<IAggregatorService, AggregatorService>();

builder.Services.AddBaseApiClient("Settings", builder.Configuration);
builder.Services.AddBaseApiClient("UserProfile", builder.Configuration);
builder.Services.AddBaseApiClient("Contacts", builder.Configuration);
builder.Services.AddBaseApiClient("Food", builder.Configuration);
builder.Services.AddBaseApiClient("Exercises", builder.Configuration);

builder.Services.AddHttpClient("TokenClient");

builder.Services.ConfigureInternalHttpClient();

builder.Services.ConfigureAuthentication2(builder.Configuration);

builder.Services.ConfigureAuthorization();

builder.Services.ConfigureCors();

builder.Services.ConfigureSwagger(Assembly.GetExecutingAssembly().GetName().Name);

builder.ConfigureLogging();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerAndUi();
}
else
{
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseMiddleware<ErrorHandlerMiddleware>();
app.UseMiddleware<CorrelationIdHeaderMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseCors("AllowAll");

app.Run();

public partial class Program { }