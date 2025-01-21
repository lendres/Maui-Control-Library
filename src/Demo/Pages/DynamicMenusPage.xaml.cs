using CommunityToolkit.Maui.Views;
using DPMauiDemo.ViewModels;

namespace DPMauiDemo.Pages;

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