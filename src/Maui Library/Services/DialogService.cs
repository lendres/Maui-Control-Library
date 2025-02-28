namespace DigitalProduction.Maui.Services;

public class DialogService : IDialogService
{
	public Page? HostingPage { get; set; }

	public void ShowMessage(string title, string message, string closeButtonText)
	{
		HostingPage?.DisplayAlert(title, message, closeButtonText);
	}
}