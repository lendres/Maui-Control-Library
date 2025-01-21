using CommunityToolkit.Maui.Views;
using DigitalProduction.Demo.ViewModels;

namespace DigitalProduction.Demo.Pages;

public partial class DynamicMenusPage : BasePage<DynamicMenusPageViewModel>
{
	public DynamicMenusPage(DynamicMenusPageViewModel viewModel) :
		base(viewModel)
	{
		InitializeComponent();
	}

	async void OnButtonAbout1Clicked(object? sender, EventArgs args)
	{
	}

	async void OnButtonAbout2Clicked(object? sender, EventArgs args)
	{
	}
}