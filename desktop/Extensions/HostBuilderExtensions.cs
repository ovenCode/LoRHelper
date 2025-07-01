using System;
using System.Configuration;
using desktop.data.db;
using desktop.data.Models;
using desktop.Services;
using desktop.Stores;
using desktop.utils;
using desktop.ViewModels;
using LoRAPI.Controllers;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualStudio.Threading;

namespace desktop.Extensions
{
    public static class HostBuilderExtensions
    {
        /// <summary>
        /// Extension function that registers all the application views in one place
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IHostBuilder AddViewModels(this IHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.AddTransient((s) => CreateProfilePageViewModel(s));
                services.AddSingleton<Func<ProfileViewModel>>(
                    (s) => () => s.GetRequiredService<ProfileViewModel>()
                );
                services.AddSingleton<NavigationService<ProfileViewModel>>();

                services.AddTransient<InGameViewModel>();
                services.AddSingleton<ILoRApiHandler, LoRApiController>();
                services.AddSingleton<ErrorLogger>();
                services.AddTransient<InGameService>();
                services.AddTransient((s) => CreateInGameViewModel(s));
                services.AddSingleton<Func<InGameViewModel>>(
                    (s) => () => s.GetRequiredService<InGameViewModel>()
                );
                services.AddSingleton<NavigationService<InGameViewModel>>();

                services.AddTransient<InfoViewModel>();
                services.AddSingleton<Func<InfoViewModel>>(
                    (s) => () => s.GetRequiredService<InfoViewModel>()
                );
                services.AddSingleton<NavigationService<InfoViewModel>>();

                services.AddTransient<SettingsViewModel>();
                services.AddSingleton<Func<SettingsViewModel>>(
                    (s) => () => s.GetRequiredService<SettingsViewModel>()
                );
                services.AddSingleton<NavigationService<SettingsViewModel>>();
                services.AddTransient<AppViewModel>();

                services.AddTransient<CustomMessageBoxViewModel>();
                services.AddSingleton<Func<CustomMessageBoxViewModel>>(
                    (s) => () => s.GetRequiredService<CustomMessageBoxViewModel>()
                );
            });

            return builder;
        }

        private static ProfileViewModel CreateProfilePageViewModel(IServiceProvider serviceProvider)
        {
            return ProfileViewModel.LoadViewModel(
                navigationStore: serviceProvider.GetRequiredService<NavigationStore>(),
                profileStore: serviceProvider.GetRequiredService<ProfileStore>(),
                loadingStore: serviceProvider.GetRequiredService<ILoadingStore>(),
                loadingService: serviceProvider.GetRequiredService<ILoadingService>(),
                inGameService: serviceProvider.GetRequiredService<InGameService>(),
                taskFactory: serviceProvider.GetRequiredService<JoinableTaskFactory>(),
                inGameNavigationService: serviceProvider.GetRequiredService<
                    NavigationService<InGameViewModel>
                >(),
                infoNavigationService: serviceProvider.GetRequiredService<
                    NavigationService<InfoViewModel>
                >(),
                settingNavigationService: serviceProvider.GetRequiredService<
                    NavigationService<SettingsViewModel>
                >()
            );
        }

        private static InGameViewModel CreateInGameViewModel(IServiceProvider serviceProvider)
        {
            return InGameViewModel.LoadViewModel(
                serviceProvider.GetRequiredService<NavigationService<ProfileViewModel>>(),
                serviceProvider.GetRequiredService<InGameService>(),
                serviceProvider.GetRequiredService<ILoadingService>(),
                serviceProvider.GetRequiredService<JoinableTaskFactory>(),
                serviceProvider.GetRequiredService<GlobalMessagingStore>()
            );
        }
    }
}
