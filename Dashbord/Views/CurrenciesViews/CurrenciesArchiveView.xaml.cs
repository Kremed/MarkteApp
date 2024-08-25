namespace Dashbord.Views.CurrenciesViews;

public partial class CurrenciesArchiveView : ContentPage
{
    //Constractor => من هنا نقطة بداية الانشاء لاي كلاس
    // يمكن أنشاء أكثر من Constractor لنفس الكلاس
    public CurrenciesArchiveView()
    {
        InitializeComponent();

        //MainThread.BeginInvokeOnMainThread(async () =>
        //{
        //    await GetCurrencies();
        //});
    }
    protected async override void OnAppearing()
    {
        await GetCurrencies();
        base.OnAppearing();
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

    private async void CurrenciesCollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        try
        {
            //var selectedItem = e.CurrentSelection.FirstOrDefault() as Currency;
            if (e.CurrentSelection.FirstOrDefault() is Currency selectedItem)
            {
                await Navigation.PushModalAsync(new CurrenciyInfoView(selectedItem, selectedItem.Id));
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

    private async void SwipEdit_Clicked(object sender, EventArgs e)
    {
        try
        {
            //SwipeView Static Code
            var swipeItem = sender as SwipeItem;

            var selectedItem = swipeItem?.BindingContext as Currency;

            if (selectedItem is not null)
            {
                await Navigation.PushModalAsync(new CurrenciyInfoView(selectedItem, selectedItem.Id));
            }
        }
        catch (Exception)
        {
        }
    }

    private async void SwipeDelete_Clicked(object sender, EventArgs e)
    {
        try
        {
            //SwipeView Static Code
            var swipeItem = sender as SwipeItem;
            var item = swipeItem?.BindingContext as Currency;

            if (item != null)
            {
                await DisplayAlert("Deleted", $"{item.Name} has been deleted.", "OK");
            }
        }
        catch
        {
        }
    }

    private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        try
        {
            if (e.Parameter is Currency selectedItem)
            {
                await Navigation.PushModalAsync(new CurrenciyInfoView(selectedItem, selectedItem.Id));
            }
        }
        catch (Exception)
        {
        }
    }

    private void TapGestureRecognizer_Tapped_1(object sender, TappedEventArgs e)
    {

    }
}