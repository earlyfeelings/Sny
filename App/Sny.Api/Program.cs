using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Sny.Api.Middlewares;
using Sny.Api.Options;
using Sny.Api.Services;
using Sny.Core.AccountsAggregate.Services;
using Sny.Core.GoalsAggregate.Services;
using Sny.Core.Interfaces.Core;
using Sny.Core.Interfaces.Infrastructure;
using Sny.Infrastructure.Services.Repos;
using System.Reflection;
using System.Text.Json.Serialization;

namespace Sny.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //TODO: restrict for production use!
            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
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
                    IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(jwtOptions.Secret))
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

            GoalInmemoryRepo inMemoryGoalRepo = new GoalInmemoryRepo();
            builder.Services.AddSingleton<IGoalReadOnlyRepo>(inMemoryGoalRepo);
            builder.Services.AddSingleton<IGoalProviderRepo>(inMemoryGoalRepo);

            builder.Services.AddScoped<IAccountManager, AccountManager>();
            builder.Services.AddSingleton<IJwtService, JwtService>();

            AccountInmemoryRepo inMemoryAccountRepo = new AccountInmemoryRepo();

            builder.Services.AddSingleton<IAccountReadOnlyRepo>(inMemoryAccountRepo);
            builder.Services.AddSingleton<IAccountProviderRepo>(inMemoryAccountRepo);

            builder.Services.AddScoped<ICurrentAccountContext, CurrentAccountContext>();

            var app = builder.Build();

            app.UseSwagger();
            app.UseSwaggerUI();

            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMiddleware<FakeLoginMiddleware>();
            }
            else
            {
                app.UseHsts();
            }

            app.UseCors();
            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseMiddleware<CurrentLoggedContextMiddleware>();

            app.MapControllers();

            app.Run();
        }
    }
}
