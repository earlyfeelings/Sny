using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Sny.Api.Middlewares;
using Sny.Api.Options;
using Sny.Api.Services;
using Sny.Core.AccountsAggregate.Services;
using Sny.Core.GoalsAggregate.Services;
using Sny.Core.Interfaces.Core;
using Sny.Core.Interfaces.Infrastructure;
using Sny.Core.TasksAggregate.Services;
using Sny.DB.Data;
using Sny.Infrastructure.Services.Repos;
using System.Reflection;
using System.Text.Json.Serialization;

namespace Sny.Api
{
    public class Program
    {

        private static string[] prodCors = new string[] 
        {
            "https://snyweb.azurewebsites.net", 
            "http://snyweb.azurewebsites.net",
            "https://snylanding.azurewebsites.net",
            "http://snylanding.azurewebsites.net"
        };

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("dev", builder =>
                builder.WithOrigins("https://localhost:7172")
                     .AllowAnyMethod()
                     .AllowCredentials()
                     .AllowAnyHeader());

                options.AddPolicy("prod", builder =>
                builder.WithOrigins(prodCors)
                     .AllowAnyMethod()
                     .AllowCredentials()
                     .AllowAnyHeader());
            });

            JwtOptions jwtOptions = new JwtOptions();
            builder.Configuration.GetSection("Jwt").Bind(jwtOptions);

            builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("Jwt"));
            builder.Services.Configure<Sny.Core.Options.PasswordOptions>(builder.Configuration.GetSection("Password"));

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = true,
                    ValidAudience = jwtOptions.Audience,
                    ValidateIssuer = true,
                    ValidIssuer = jwtOptions.Issuer,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(jwtOptions.Secret)),
                    ClockSkew = TimeSpan.FromSeconds(10)
                };
            });

            var policy = new AuthorizationPolicyBuilder()
                  .RequireAuthenticatedUser()
                  .Build();

            builder.Services
                .AddControllers(opt => opt.Filters.Add(new AuthorizeFilter(policy)))
                .AddJsonOptions(x =>
            {
                x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(opt =>
            {
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                opt.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    { 
                        new OpenApiSecurityScheme {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[]{}
                    }
               });
            });

            builder.Services.AddScoped<IGoalProvider, GoalProvider>();

            DbContextOptionsBuilder<SnySQLiteContext> dbContextOptionsBuilder = new DbContextOptionsBuilder<SnySQLiteContext>();
            dbContextOptionsBuilder.UseSqlite("Data Source=..//LocalDatabase.db", b => b.MigrationsAssembly("Sny.DB"));
            dbContextOptionsBuilder.UseLazyLoadingProxies();

            SnySQLiteContext snySQLiteContext = new SnySQLiteContext(dbContextOptionsBuilder.Options);
            builder.Services.AddDbContext<SnySQLiteContext>(options => new SnySQLiteContext(dbContextOptionsBuilder.Options));
            SnySQLiteContextSeed.SeedAsync(snySQLiteContext).Wait();

            builder.Services.AddScoped<IAccountManager, AccountManager>();
            builder.Services.AddSingleton<IJwtService, JwtService>();


            builder.Services.AddScoped<ITaskProvider, TaskProvider>();

            builder.Services.AddScoped<ITaskReadOnlyRepo, TaskSQLiteRepo>();
            builder.Services.AddScoped<ITaskProviderRepo, TaskSQLiteRepo>();

            builder.Services.AddScoped<IGoalReadOnlyRepo, GoalSQLiteRepo>();
            builder.Services.AddScoped<IGoalProviderRepo, GoalSQLiteRepo>();

            builder.Services.AddScoped<IAccountReadOnlyRepo, AccountSQLiteRepo>();
            builder.Services.AddScoped<IAccountProviderRepo, AccountSQLiteRepo>();

            builder.Services.AddScoped<ICurrentAccountContext, CurrentAccountContext>();

            builder.Services.AddScoped<ITaskProvider, TaskProvider>();
            builder.Services.AddSingleton<ILoginTokenManager, LoginTokenManager>();

            var app = builder.Build();

            app.UseSwagger();
            app.UseSwaggerUI();

            if (app.Environment.IsDevelopment())
            {
                app.UseCors("dev");
                app.UseDeveloperExceptionPage();
                app.UseMiddleware<FakeLoginMiddleware>();
            }
            else
            {
                app.UseCors("prod");
                app.UseHsts();
            }

           
            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseMiddleware<CurrentLoggedContextMiddleware>();

            app.MapControllers();

            app.Run();
        }
    }
}
