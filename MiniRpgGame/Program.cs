using System.IO;
using MiniRpgGame.Domain;
using MiniRpgGame.Domain.Commands;
using MiniRpgGame.Domain.Platform;
using MiniRpgGame.Infrastructure;
using MiniRpgGame.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MiniRpgGame
{
    static class Program
    {
        static void Main(string[] args)
        {
            var serviceProvider = BuildServiceProvider();
            using (var scope = serviceProvider.CreateScope())
            {
                var services = scope.ServiceProvider;
                var game = services.GetRequiredService<IGame>();
                game.StartGame();
            }
        }

        private static ServiceProvider BuildServiceProvider()
        {
            var services = new ServiceCollection();
            ConfigureServices(services);
            return services.BuildServiceProvider();
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            var configuration = BuildConfiguration();
            
            services.AddSingleton<IConfiguration>(configuration);
            
            var settings = new SettingsProvider(configuration);
            services.AddSingleton<IBattleSettings>(settings);
            services.AddSingleton<IGameplaySettings>(settings);
            services.AddSingleton<IHealerSettings>(settings);
            services.AddSingleton<IStockroomSetting>(settings);

            services.AddSingleton<IEventBus, EventBus>();
            
            services.AddScoped<IStockroom, Stockroom>();
            services.AddScoped<Doctor>();
            services.AddScoped<Seller>();            
            services.AddScoped<IBattleLogic, DefaultBattleLogic>();
            services.AddScoped<Battle>();
            services.AddScoped<Gameplay>();
            services.AddScoped<IConsoleGameIO, ConsoleGameIO>();
            services.AddScoped<IBot, DumbBot>();
            services.AddScoped<IGame, ConsoleGame>();
        }

        private static IConfigurationRoot BuildConfiguration() =>
            new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json").Build();
    }
}