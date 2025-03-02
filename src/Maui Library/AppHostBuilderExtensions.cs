using CommunityToolkit.Maui;

namespace DigitalProduction.Maui;

/// <summary>
/// Generic MAUI extensions for custom fonts.
/// </summary>
public static class AppHostBuilderExtensions
{
    /// <summary>
    /// Configures fonts for the MAUI application.
    /// </summary>
    /// <param name="builder">The MAUI application builder.</param>
    /// <returns>Configured MAUI app builder with custom settings.</returns>
    public static MauiAppBuilder UseDigitalProductionMaui(this MauiAppBuilder builder)
    {
		builder.UseMauiCommunityToolkit();
        builder.ConfigureFonts(fonts =>
        {
            //fonts.AddFont("fa_solid.ttf", "FontAwesomeSolid");
			fonts.AddFont("FontAwesomeSolid900.oft", "FontAwesomeSolid");
        });

        return builder;
    }
}