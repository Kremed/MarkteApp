using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;

namespace Dashbord
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new MainView();

            //[Entry Return Type] github issues: https://github.com/dotnet/maui/issues/23086#issuecomment-2211830145
            EntryHandler.Mapper.AppendToMapping("EntryReturnTypeFix", (handler, view) =>
            {
#if ANDROID
                handler.PlatformView.UpdateReturnType(view);
#endif
            });
        }
    }
}
