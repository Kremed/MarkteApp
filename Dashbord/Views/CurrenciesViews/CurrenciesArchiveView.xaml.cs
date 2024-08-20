


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

    //OnAppearing

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

    private async void CurrenciesCollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        try
        {
            if (e.CurrentSelection.FirstOrDefault() is Currency selectedItem)
            {
                int selectedId = selectedItem.Id;
                await Navigation.PushModalAsync(new CurrenciyInfoView(selectedItem));


            }
        }
        catch (Exception)
        {
        }
    }

    private async void refreshView_Refreshing(object sender, EventArgs e)
    {
        try
        {
            await GetCurrencies();
        }
        finally
        {
            refreshView.IsRefreshing = false;
        }
    }
}