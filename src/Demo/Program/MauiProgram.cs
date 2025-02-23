using CommunityToolkit.Maui;
using CommunityToolkit.Maui.ApplicationModel;
using CommunityToolkit.Maui.Markup;
using CommunityToolkit.Maui.Media;
using CommunityToolkit.Maui.Storage;
using DigitalProduction.Demo.Pages;
using DigitalProduction.Demo.ViewModels;
using DigitalProduction.Maui.Services;
using Microsoft.Extensions.Logging;

namespace DigitalProduction.Demo;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseMauiCommunityToolkit()
			.UseMauiCommunityToolkitMarkup()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		RegisterViewsAndViewModels(builder.Services);
		RegisterEssentials(builder.Services);
		CreateServices(builder.Services);
		#if DEBUG
			builder.Logging.AddDebug();
		#endif

		return builder.Build();
	}

	private static void RegisterViewsAndViewModels(in IServiceCollection services)
	{
		services.AddTransient<ControlsGalleryPage, ControlsGalleryViewModel>();
		services.AddTransientWithShellRoute<DynamicMenusPage, DynamicMenusPageViewModel>();
		services.AddTransientWithShellRoute<RecentlyUsedMenuPage, RecentlyUsedMenuPageViewModel>();
	}

	private static IServiceCollection AddTransientWithShellRoute<TPage, TViewModel>(this IServiceCollection services)
		where TPage : BasePage<TViewModel>
		where TViewModel : BaseViewModel
	{
		return services.AddTransientWithShellRoute<TPage, TViewModel>(AppShell.GetPageRoute<TViewModel>());
	}

	private static void CreateServices(IServiceCollection services)
	{
		services.AddSingleton<IDialogService, DialogService>();
		services.AddSingleton<IMenuService, MenuService>();
		services.AddSingleton<IRecentPathsManagerService, RecentPathsManagerService>();
	}

	private static void RegisterEssentials(in IServiceCollection services)
	{
		services.AddSingleton<IDeviceDisplay>(DeviceDisplay.Current);
		services.AddSingleton<IDeviceInfo>(DeviceInfo.Current);
		services.AddSingleton<IFileSaver>(FileSaver.Default);
		services.AddSingleton<IFolderPicker>(FolderPicker.Default);
		services.AddSingleton<IBadge>(Badge.Default);
		services.AddSingleton<ISpeechToText>(SpeechToText.Default);
		services.AddSingleton<ITextToSpeech>(TextToSpeech.Default);
	}
}