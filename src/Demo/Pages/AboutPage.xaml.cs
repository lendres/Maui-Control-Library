using CommunityToolkit.Maui.Views;
using DPMauiDemo.ViewModels;

namespace DPMauiDemo.Pages;

public partial class AboutPage : BasePage<AboutPageViewModel>
{
	public AboutPage(AboutPageViewModel viewModel) :
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