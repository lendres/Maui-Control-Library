using CommunityToolkit.Maui.Views;
using DigitalProduction.Demo.ViewModels;

namespace DigitalProduction.Demo.Pages;

public partial class RecentlyUsedMenuPage : BasePage<RecentlyUsedMenuPageViewModel>
{
	public RecentlyUsedMenuPage(RecentlyUsedMenuPageViewModel viewModel) :
		base(viewModel)
	{
		InitializeComponent();
		viewModel.MenuHostingPage	= this;
	}
}