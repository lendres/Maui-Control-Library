using CommunityToolkit.Mvvm.Input;

namespace MenuDemo;

public partial class MainPage : ContentPage
{
    public MainPage(MainViewModel viewModel)
    {
        InitializeComponent();
		BindingContext				= viewModel;
		viewModel.MenuHostingPage	= this;
    }
}