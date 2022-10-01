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

            builder.Services.AddControllers().AddJsonOptions(x =>
            {
                x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(opt =>
            {
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                opt.IncludeXmlComments(xmlPath);
            });

            builder.Services.AddScoped<IGoalProvider, GoalProvider>();
            builder.Services.AddSingleton<IGoalReadOnlyRepo, GoalInmemoryRepo>();

            builder.Services.AddScoped<IAccountManager, AccountManager>();

            AccountInmemoryRepo inMemoryAccountRepo = new AccountInmemoryRepo();

            builder.Services.AddSingleton<IAccountReadOnlyRepo>(inMemoryAccountRepo);
            builder.Services.AddSingleton<IAccountProviderRepo>(inMemoryAccountRepo);

            var app = builder.Build();
            
            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseCors();

            app.UseHttpsRedirection();
            

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
