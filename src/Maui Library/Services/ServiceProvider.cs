namespace DigitalProduction.Maui.Services;

public static class ServiceProvider
{
	public static T GetService<T>()
	{
		return (T)(IPlatformApplication.Current?.Services.GetService(typeof(T)) ??
			throw new NullReferenceException("Cannot find a service of type "+nameof(T)+"."));
	}
}