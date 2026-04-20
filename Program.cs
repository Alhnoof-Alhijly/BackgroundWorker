using AutoMapper;
using BackgroundWorker;
using Microsoft.EntityFrameworkCore;
using Template.AutoMapper;
using Template.Database;
using Template.Extensions.Registers;
using Template.Services.Calculator;


public class Program
{
    public static void Main(string[] arg)
    {
        var host = CalculatorHostBuilder(arg).Build();

        host.Run();
    }

    public static IHostBuilder CalculatorHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .ConfigureServices((hostContext, services) =>
        {
            services.RegisterAppSettings(hostContext.Configuration);
            services.AddDbContext<DatabaseContext>(x => x.UseNpgsql(services.GetAppSettings().ConnectionString.ConnectionStringDb));

            var mapper = new MapperConfiguration(x =>
            {
                x.AddProfile(new AutoMapperConfiguration());
            });

            var map = mapper.CreateMapper();
            services.AddSingleton(map);

            services.AddScoped<ICalculatorService, CalculatorService>();

            services.AddHostedService<CalculatorWorker>();
        });

}