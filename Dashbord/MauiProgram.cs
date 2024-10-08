﻿global using RestSharp;
global using Dashbord.Views;
global using Newtonsoft.Json;
global using MarketApp.Shered;
global using Dashbord.Views.CurrenciesViews;

namespace Dashbord
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

            return builder.Build();
        }
    }
}
