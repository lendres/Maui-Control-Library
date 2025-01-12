using CommunityToolkit.Mvvm.Input;

namespace MenuTest;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
		MainViewModel viewModel = BindingContext as MainViewModel;
		viewModel.MenuHostingPage = this;
    }
}