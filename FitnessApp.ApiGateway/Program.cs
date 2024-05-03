using System.Net.Http.Headers;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;
using FitnessApp.ApiGateway;
using FitnessApp.ApiGateway.Configuration;
using FitnessApp.ApiGateway.Extensions;
using FitnessApp.ApiGateway.Middleware;
using FitnessApp.ApiGateway.Services.Aggregator;
using FitnessApp.ApiGateway.Services.InternalClient;
using FitnessApp.ApiGateway.Services.TokenClient;
using FitnessApp.ApiGateway.Services.UserIdProvider;
using FitnessApp.Common.Configuration;
using FitnessApp.Common.Middleware;
using FitnessApp.Common.Serializer.JsonSerializer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDistributedRedisCache(option =>
{
    option.Configuration = builder.Configuration["Redis:Configuration"];
});

builder.Services.AddTransient<IJsonSerializer, JsonSerializer>();
builder.Services.ConfigureMapper(new MappingProfile());

var apiAuthenticationSettings = builder.Configuration.GetSection("ApiAuthenticationSettings");
builder.Services.Configure<ApiAuthenticationSettings>(apiAuthenticationSettings);

builder.Services.ConfigureNats(builder.Configuration);

builder.Services.AddTransient<ITokenClient, TokenClient>();

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

#pragma warning disable SA1515 // Single-line comment should be preceded by blank line
#pragma warning disable SA1005 // Single line comments should begin with single space
builder.Services.AddHttpClient("InternalClient", client =>
{
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
})
    //.AddPolicyHandler(HttpPolicyExtensions.HandleTransientHttpError()
    //.OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
    //.WaitAndRetryAsync(int.Parse(internalApiSection.GetSection("RetryAttempt").Value), retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))))
    ;
#pragma warning restore SA1005 // Single line comments should begin with single space
#pragma warning restore SA1515 // Single-line comment should be preceded by blank line

builder.Services
    .AddAuthentication(opts =>
    {
        opts.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        opts.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        opts.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(cfg =>
     {
         cfg.RequireHttpsMetadata = false;
         cfg.Authority = builder.Configuration["ClientAuthenticationSettings:Issuer"];
         cfg.TokenValidationParameters.ValidAudiences = builder.Configuration["ClientAuthenticationSettings:Audience"].Split(" ");
         cfg.Events = new JwtBearerEvents
         {
             OnAuthenticationFailed = context =>
             {
                 if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                 {
                     context.Response.Headers.Append("Token-Expired", "true");
                 }

                 return Task.CompletedTask;
             }
         };
     });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(builder.Configuration["ClientAuthenticationSettings:Policy"], policy => policy.RequireClaim(ClaimTypes.NameIdentifier));
});

builder.Services.AddCors(o => o.AddPolicy("AllowAll", builder =>
{
    builder.AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader();
}));

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
app.UseAuthentication();
app.UseMiddleware<ErrorHandlerMiddleware>();
app.UseMiddleware<CorrelationIdHeaderMiddleware>();
app.MapControllers();
app.UseCors("AllowAll");

app.Run();

public partial class Program { }