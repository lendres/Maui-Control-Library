namespace DigitalProduction.Maui.Services;

public interface IDialogService
{
	Page? HostingPage { get;  set; }

    void ShowMessage(string title, string message, string closeButtonText);
}