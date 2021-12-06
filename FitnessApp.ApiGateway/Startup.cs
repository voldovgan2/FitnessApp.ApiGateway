using System;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using FitnessApp.ApiGateway.Configuration;
using FitnessApp.ApiGateway.Extensions;
using FitnessApp.ApiGateway.Models.Exercises.Output;
using FitnessApp.ApiGateway.Models.Food.Output;
using FitnessApp.ApiGateway.Models.Settings.Output;
using FitnessApp.ApiGateway.Models.UserProfile.Output;
using FitnessApp.ApiGateway.Services.Aggregator;
using FitnessApp.ApiGateway.Services.Contacts;
using FitnessApp.ApiGateway.Services.Exercises;
using FitnessApp.ApiGateway.Services.Food;
using FitnessApp.ApiGateway.Services.Settings;
using FitnessApp.ApiGateway.Services.UserIdProvider;
using FitnessApp.ApiGateway.Services.UserProfile;
using FitnessApp.Serializer.JsonMapper;
using FitnessApp.Serializer.JsonSerializer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Polly;
using Polly.Extensions.Http;

namespace FitnessApp.ApiGateway
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(options =>
            {
                options.EnableEndpointRouting = false;
            });

            services.AddDistributedRedisCache(option =>
            {
                option.Configuration = Configuration["Redis:Configuration"];
                option.InstanceName = Configuration["Redis:InstanceName"];
            });

            services.AddTransient<IJsonSerializer, JsonSerializer>();

            services.AddTransient<IJsonMapper, JsonMapper>();

            services.AddTransient<IUserIdProvider, UserIdProvider>();

            services.AddTransient<ISettingsService<SettingsModel>, SettingsService<SettingsModel>>();

            services.AddTransient<IUserProfileService<UserProfileModel>, UserProfileService<UserProfileModel>>();

            services.AddTransient<IContactsService, ContactsService>();

            services.AddTransient<IFoodService<UserFoodsModel, FoodItemModel>, FoodService<UserFoodsModel, FoodItemModel>>();

            services.AddTransient<IExercisesService<UserExercisesModel, ExerciseItemModel>, ExercisesService<UserExercisesModel, ExerciseItemModel>>();

            services.AddTransient<IAggregatorService, AggregatorService>();

            var internalApiSection = Configuration.GetSection("Apis:Internal");
            services.Configure<AuthenticationSettings>(internalApiSection);

            services.AddHttpClient("TokenClient", client =>
            {
                client.BaseAddress = new Uri(internalApiSection.GetSection("Address").Value);
            });

            services.AddHttpClient("InternalClient", client => 
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            })                
                //.AddPolicyHandler(HttpPolicyExtensions.HandleTransientHttpError()
                //.OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
                //.WaitAndRetryAsync(int.Parse(internalApiSection.GetSection("RetryAttempt").Value), retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))))
                ;

            services.AddBaseApiClient("Settings", Configuration);
            services.AddBaseApiClient("UserProfile", Configuration);
            services.AddCollectionApiClient("Contacts", Configuration);
            services.AddCollectionApiClient("Food", Configuration);
            services.AddCollectionApiClient("Exercises", Configuration);

            services.AddAuthentication(opts =>
            {
                opts.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opts.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                opts.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(cfg =>
            {
                cfg.RequireHttpsMetadata = false;
                cfg.Authority = Configuration["JWT:Issuer"];
                cfg.TokenValidationParameters.ValidAudiences = Configuration["JWT:Audience"].Split(" ");
                cfg.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            context.Response.Headers.Add("Token-Expired", "true");
                        }
                        return Task.CompletedTask;
                    }
                };
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Authenticated", policy => policy.RequireClaim(ClaimTypes.NameIdentifier));
            });

            services.AddCors(o => o.AddPolicy("AllowAll", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));    
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            loggerFactory.AddFile("Logs/ApiGateway-{Date}.txt");

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseMvc();

            app.UseCors("AllowAll");
        }
    }
}