using CommunityToolkit.Mvvm.Input;

namespace DynamicMenuDemo;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
		MainViewModel viewModel = BindingContext as MainViewModel;
		viewModel.MenuHostingPage = this;
    }
}