
using Dashbord.Views.MarktingViews;

namespace Dashbord.Views;

public partial class MainView : ContentPage
{
    public MainView()
    {
        InitializeComponent();
    }

    private async void BrdCreateCurency_Tapped(object sender, TappedEventArgs e)
    {

        try
        {
            var ddd = Convert.ToInt32("12");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Exception !!", $":تعرض التطبيق لخطاء ما, الرجاء اعادة المحاولة {Environment.NewLine} {ex.Message}", "موافق");
        }
        finally
        {
            await Navigation.PushModalAsync(new CreateCurrencyView());
        }
    }

    private async void BrdCurenciesArchive_Tapped(object sender, TappedEventArgs e)
    {
        try
        {
            await Navigation.PushModalAsync(new CurrenciesArchiveView());
        }
        catch (Exception ex)
        {
            await DisplayAlert("Exception !!", $":تعرض التطبيق لخطاء ما, الرجاء اعادة المحاولة {Environment.NewLine} {ex.Message}", "موافق");
        }
    }

    private async void BrdAdsArchive_Tapped(object sender, TappedEventArgs e)
    {
        try
        {
            await Navigation.PushModalAsync(new MarktingArchiveView());
        }
        catch (Exception ex)
        {
            await DisplayAlert("Exception !!", $":تعرض التطبيق لخطاء ما, الرجاء اعادة المحاولة {Environment.NewLine} {ex.Message}", "موافق");
        }
    }
}