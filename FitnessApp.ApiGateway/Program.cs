using System.Net.Http.Headers;
using System.Reflection;
using System.Security.Claims;
using AutoMapper;
using FitnessApp.ApiGateway;
using FitnessApp.ApiGateway.Configuration;
using FitnessApp.ApiGateway.Extensions;
using FitnessApp.ApiGateway.Services.Aggregator;
using FitnessApp.ApiGateway.Services.InternalClient;
using FitnessApp.ApiGateway.Services.TokenClient;
using FitnessApp.ApiGateway.Services.UserIdProvider;
using FitnessApp.Common.Configuration.Swagger;
using FitnessApp.Common.Serializer.JsonSerializer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Identity.Web;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDistributedRedisCache(option =>
{
    option.Configuration = builder.Configuration["Redis:Configuration"];
});

builder.Services.AddTransient<IJsonSerializer, JsonSerializer>();

var mapperConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new MappingProfile());
});
IMapper mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

var authenticationSettings = builder.Configuration.GetSection("AuthenticationSettings");
builder.Services.Configure<AuthenticationSettings>(authenticationSettings);

builder.Services.AddTransient<ITokenClient, TokenClient>();

builder.Services.AddTransient<IInternalClient, InternalClient>();

builder.Services.AddTransient<IUserIdProvider, UserIdProvider>();

builder.Services.AddSettingsService(builder.Configuration);

builder.Services.AddUserProfileService(builder.Configuration);

builder.Services.AddContactsService(builder.Configuration);

builder.Services.AddFoodService(builder.Configuration);

builder.Services.AddExercisesService(builder.Configuration);

builder.Services.AddSignalRService(builder.Configuration);

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
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(
        options =>
    {
        builder.Configuration.Bind("AzureAdB2C", options);
        options.TokenValidationParameters.NameClaimType = "name";
    },
        options =>
        {
            builder.Configuration.Bind("AzureAdB2C", options);
        });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Authenticated", policy => policy.RequireClaim(ClaimTypes.NameIdentifier));
});

builder.Services.AddCors(o => o.AddPolicy("AllowAll", builder =>
{
    builder.AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader();
}));

builder.Services.ConfigureSwaggerConfiguration(Assembly.GetExecutingAssembly().GetName().Name);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseCors("AllowAll");

app.UseSwagger();

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Swagger XML Api Demo v1");
});
