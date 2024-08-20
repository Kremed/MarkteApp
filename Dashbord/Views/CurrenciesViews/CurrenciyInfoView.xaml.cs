namespace Dashbord.Views.CurrenciesViews;

public partial class CurrenciyInfoView : TabbedPage
{
    public readonly RestClient client = new();
    public Currency localSelectedCurrency; //انشاء كائن من نوع عملة لحفظ البيانات علي صعيد الصفحة


    public CurrenciyInfoView(Currency selectedItem)
    {
        InitializeComponent();

        localSelectedCurrency = selectedItem;

        SetData();
    }

    private void SetData()
    {
        TxtName.Text = localSelectedCurrency.Name;
        TxtDiscrption.Text = localSelectedCurrency.Description;
        TxtCode.Text = localSelectedCurrency.Code;
        ImgCurrencyUrl.Source = localSelectedCurrency.ImageUrl;
        if (localSelectedCurrency.IsActive)
        {
            LblActiveState.Text = "فعـالة";
            LblActiveState.TextColor = Colors.Green;
        }
        else
        {
            LblActiveState.Text = "غير فعـالة";
            LblActiveState.TextColor = Colors.Red;
        }
    }

    private async void BtnEditCurrency_Clicked(object sender, EventArgs e)
    {
        try
        {
            await Navigation.PushModalAsync(new CreateCurrencyView(localSelectedCurrency));
        }
        catch (Exception)
        {

        }
    }

    private void BtnDelete_Clicked(object sender, EventArgs e)
    {
        try
        {
            var request = new RestRequest(
                          $"https://maui.ly/api/Currency/admin/deleteCurrency?currencyID={localSelectedCurrency.Id}",
                          Method.Post);


        }
        catch (Exception)
        {
        }
    }
}