using Microsoft.Extensions.Logging;

namespace EndUser
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("Zain-ExtraLight.ttf", "ExtraLight");
                    fonts.AddFont("Zain-Light.ttf", "Light");
                    fonts.AddFont("Zain-Regular.ttf", "Regular");
                    fonts.AddFont("Zain-Bold.ttf", "Bold");
                    fonts.AddFont("Zain-ExtraBold.ttf", "ExtraBold");
                    fonts.AddFont("Zain-Black.ttf", "Black");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
