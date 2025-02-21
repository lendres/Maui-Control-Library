using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DigitalProduction.Maui.Services;

namespace DigitalProduction.Demo.ViewModels;

public partial class RecentlyUsedMenuPageViewModel : BaseViewModel
{
	#region Fields

	private readonly IDialogService     _dialogService;
	private string						_fileDirectory;
	private int                         _fileCounter        = 0;

	#endregion

	#region Construction

	public RecentlyUsedMenuPageViewModel(IRecentPathsManagerService recentPathsManagerService, IDialogService dialogService)
	{
		RecentPathsManagerService	= recentPathsManagerService;
		_dialogService				= dialogService;

		_fileDirectory				= DigitalProduction.Reflection.Assembly.Path()!;

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

	private string CreateTempFile()
	{
		string tempFile = System.IO.Path.Combine(_fileDirectory, "File That Exists " + (++_fileCounter).ToString() + ".txt");
		System.IO.File.WriteAllText(tempFile, "This file is for testing only."+Environment.NewLine);
		return tempFile;
	}

	private void RemoveTempFiles()
	{
		 string[] files = Directory.GetFiles(_fileDirectory, "File That Exists*");
		
		foreach (string file in files)
		{
			System.IO.File.Delete(file);
		}
	}

	[RelayCommand]
	void ShowSelectedMessage(string message)
	{
		_dialogService.ShowMessage("Menu clicked", $"The path \"{message}\" was selected.", "OK");
	}

	[RelayCommand]
	void ShowRemovedMessage(string message)
	{
		_dialogService.ShowMessage("Menu clicked", $"The path \"{message}\" was was not found.", "OK");
	}

	[RelayCommand]
	void AddNewFile()
	{
		RecentPathsManagerService.PushTop(CreateTempFile());
	}

	[RelayCommand]
	void RemoveFiles()
	{
		RemoveTempFiles();
	}

	[RelayCommand]
	void ResetPaths()
	{
		RemoveTempFiles();
		RecentPathsManagerService.ClearAllPaths();
		RecentPathsManagerService.PushTop(CreateTempFile());
		RecentPathsManagerService.PushTop(@"C:\Temp\Does Not Exist File 2.txt");
		RecentPathsManagerService.PushTop(@"C:\Temp\Does Not Exist File 1.txt");
	}

	#endregion
}