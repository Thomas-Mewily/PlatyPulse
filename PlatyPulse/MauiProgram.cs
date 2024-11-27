using Microsoft.Extensions.Logging;
using PlatyPulseAPI;
using System.Collections.ObjectModel;

namespace PlatyPulse;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        PlatyApp.InitJsonSerializerOptions();

        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
		builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
