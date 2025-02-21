using DigitalProduction.Demo.ViewModels;
using CommunityToolkit.Maui.Views;

namespace DigitalProduction.Demo.Pages;

public partial class RecentlyUsedSettingPage : Popup
{
	public RecentlyUsedSettingPage(RecentlyUsedSettingViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}

	async protected virtual void OnSaveButtonClicked(object? sender, EventArgs eventArgs)
	{
		RecentlyUsedSettingViewModel? viewModel = BindingContext as RecentlyUsedSettingViewModel;
		System.Diagnostics.Debug.Assert(viewModel != null);

		viewModel.Save();
		CancellationTokenSource cancelationTokenSource = new(TimeSpan.FromSeconds(5));
		await CloseAsync(true, cancelationTokenSource.Token);
	}

	async protected void OnCancelButtonClicked(object? sender, EventArgs eventArgs)
	{
		CancellationTokenSource cancelationTokenSource = new(TimeSpan.FromSeconds(5));
		await CloseAsync(false, cancelationTokenSource.Token);
	}
}