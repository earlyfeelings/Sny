using Sny.Core.GoalsAggregate.Services;
using Sny.Core.Interfaces.Core;
using Sny.Core.Interfaces.Infrastructure;
using Sny.Infrastructure.Services.Repos;
using System.Reflection;

namespace Sny.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(opt =>
            {
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                opt.IncludeXmlComments(xmlPath);
            });

            builder.Services.AddScoped<IGoalsProvider, GoalsProvider>();
            builder.Services.AddScoped<IGoalsReadOnlyRepo, GoalsInmemoryRepo>();

            var app = builder.Build();
            
            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}