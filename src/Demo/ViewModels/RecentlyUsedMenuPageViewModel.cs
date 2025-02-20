using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DigitalProduction.Maui.Services;

namespace DigitalProduction.Demo.ViewModels;

public partial class RecentlyUsedMenuPageViewModel : BaseViewModel
{
	#region Fields

	private readonly IDialogService     _dialogService;

	#endregion

	#region Construction

	public RecentlyUsedMenuPageViewModel(IRecentPathsManagerService recentPathsManagerService, IDialogService dialogService)
	{
		RecentPathsManagerService	= recentPathsManagerService;
		_dialogService				= dialogService;

		// During the construction of the RecentPathsManagerService, it will restore any saved values.  If
		// No values were saved, then we will add some here for demonstration purposed (so we don't get a blank
		// control).  Normally, you would not do this.
		if (RecentPathsManagerService.GetRecentPaths().Count == 0)
		{
			ResetPaths();
		}
	}

	#endregion

	#region Properties

	[ObservableProperty]
	public partial IRecentPathsManagerService		RecentPathsManagerService { get; set; }

	public Page? MenuHostingPage
	{
		get => _dialogService.HostingPage;
		set => _dialogService.HostingPage = value;
	}

	#endregion

	#region Methods

	[RelayCommand]
	void ShowSelectedMessage(string message)
	{
		_dialogService.ShowMessage("Menu clicked", $"The path \"{message}\" was selected.", "OK");
	}

	[RelayCommand]
	void AddNewPath()
	{
		RecentPathsManagerService.PushTop(@"C:\New\New File.txt");
	}

	[RelayCommand]
	void RemoveNewPath()
	{
		RecentPathsManagerService.RemovePath(@"C:\New\New File.txt");
	}

	[RelayCommand]
	void ResetPaths()
	{
		RecentPathsManagerService.ClearAllPaths();
		RecentPathsManagerService.PushTop(@"C:\Temp\File.txt");
		RecentPathsManagerService.PushTop(@"C:\Users\Lance\Notes.txt");
	}

	#endregion
}