using CommunityToolkit.Maui.Views;
using DigitalProduction.Demo.ViewModels;

namespace DigitalProduction.Demo.Pages;

public partial class StepperPage : BasePage<StepperPageViewModel>
{
	public StepperPage(StepperPageViewModel viewModel) :
		base(viewModel)
	{
		InitializeComponent();
	}
}