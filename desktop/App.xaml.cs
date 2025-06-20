using System;
using System.Windows;
using desktop.data.db;
using desktop.data.Models;
using desktop.Extensions;
using desktop.Services;
using desktop.Services.DataProviders;
using desktop.Stores;
using desktop.ViewModels;
using LoRAPI.Controllers;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualStudio.Threading;

namespace desktop
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly IHost _host;

        public App()
        {
            _host = Host.CreateDefaultBuilder()
                .AddViewModels()
                .ConfigureServices(
                    (hostContext, services) =>
                    {
                        string? connectionString = hostContext.Configuration.GetConnectionString(
                            "Default"
                        );
                        if (string.IsNullOrEmpty(connectionString))
                        {
                            throw new ArgumentException(
                                "Nie można było wyczytać danych do połączenia się z bazą danych"
                            );
                        }
                        services.AddSingleton<ILoRDbContextFactory>(
                            new LoRDbContextFactory(connectionString)
                        );
                        services.AddSingleton<NavigationStore>();
                        services.AddSingleton((s) => new Profile("Test name"));
                        services.AddTransient((s) => new JoinableTaskContext());
                        services.AddSingleton<JoinableTaskFactory>();
                        services.AddSingleton<ProfileStore>();
                        services.AddSingleton<IDataProvider, DataProvider>();
                        services.AddSingleton(s => new MainWindow
                        {
                            DataContext = s.GetRequiredService<AppViewModel>()
                        });
                    }
                )
                .Build();
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            _host.Start();

            ILoRDbContextFactory loRDbContextFactory =
                _host.Services.GetRequiredService<ILoRDbContextFactory>();
            using (LoRDbContext dbContext = loRDbContextFactory.CreateDbContext())
            {
                dbContext.Database.Migrate();
            }
            NavigationService<ProfileViewModel> profileNavigation =
                _host.Services.GetRequiredService<NavigationService<ProfileViewModel>>();
            await profileNavigation.NavigateAsync();

            MainWindow = _host.Services.GetRequiredService<MainWindow>();
            MainWindow.Show();
            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _host.Dispose();
            base.OnExit(e);
        }
    }
}
