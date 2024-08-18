
namespace Dashbord.Views.CurrenciesViews;

public partial class CurrenciesArchiveView : ContentPage
{
    public CurrenciesArchiveView()
    {
        InitializeComponent();

        MainThread.BeginInvokeOnMainThread(async () =>
        {
            await GetCurrencies();
        });
    }

    public async Task GetCurrencies()
    {
        try
        {

            var client = new RestClient();

            var request = new RestRequest("https://maui.ly/api/Currency/admin/getCurrencies", Method.Get);

            RestResponse response = await client.ExecuteAsync(request);

            if (response.IsSuccessStatusCode &&
                !string.IsNullOrEmpty(response.Content))
            {
                var currenciesList = JsonConvert.DeserializeObject<List<Currency>>(response.Content);

                CurrenciesCollectionView.ItemsSource = currenciesList;
            }
            else
            {
                await DisplayAlert("لم يتم استجلاب البيانات", response.Content, "موافق");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Exception On GetCurrencies() :", ex.Message, "OK");
        }
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        try
        {
            await Navigation.PopModalAsync();
        }
        catch (Exception)
        {
        }
    }
}