namespace Dashbord.Views.CurrenciesViews;

public partial class CurrenciyInfoView : TabbedPage
{
    public readonly RestClient client = new();
    public Currency localSelectedCurrency; //انشاء كائن من نوع عملة لحفظ البيانات علي صعيد الصفحة


    public CurrenciyInfoView(Currency selectedItem, int ID)
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
            await Navigation.PushModalAsync(new CreateCurrencyView(localSelectedCurrency), false);
        }
        catch (Exception)
        {

        }
    }

    private async void BtnDelete_Clicked(object sender, EventArgs e)
    {
        try
        {
            //var request = new RestRequest($"https://maui.ly/api/Currency/admin/deleteCurrency?currencyID={localSelectedCurrency.Id}", Method.Post);

            var request = new RestRequest($"https://maui.ly/api/Currency/admin/deleteCurrency", Method.Post);
            request.AddQueryParameter("currencyID", localSelectedCurrency.Id, true);


            var responce = await client.ExecuteAsync(request);

            if (responce.IsSuccessStatusCode)
            {
                await DisplayAlert("نجاح العملية", "تمت العملية بنجاح", "موافق");
                await Navigation.PopModalAsync(true);
            }
            else
            {
                await DisplayAlert("فشل العملية", $"لم تتم العملية: {responce.Content}", "موافق");
            }

        }
        catch (Exception)
        {
        }
    }

    private async void BtnSaveNewPrice_Clicked(object sender, EventArgs e)
    {
        try
        {
            if (TxtCurrentPrice.Text.Any(char.IsLetter))
            {
                await DisplayAlert("ادخال خاطي", "الرجاء ادخال سعر صحيح, لايسمح بأدراج الحروف في حقل السعر", "OK");
                return;
            }

            var IsDoublePrice = double.TryParse(TxtCurrentPrice.Text, out double price);
            if (!IsDoublePrice)
            {
                await DisplayAlert("ادخال خاطي", "الرجاء ادخال سعر صحيح, لايسمح بأدراج الحروف في حقل السعر", "OK");
                return;
            }


            CurrencyPrice currencyPrice = new()
            {
                CurrencyId = localSelectedCurrency.Id,
                Price = price,
            };

            var clinte = new RestClient();
            var request = new RestRequest($"https://maui.ly/api/Currency/admin/createCurrencyPrice/{localSelectedCurrency.Id}", Method.Post);

            request.AddHeader("Content-Type", "application/json");

            var json = JsonConvert.SerializeObject(currencyPrice);
            request.AddJsonBody(json);

            //request.AddBody(currencyPrice);

            var responce = await clinte.ExecuteAsync(request);
            if (responce.IsSuccessStatusCode)
            {
                var prisesList = JsonConvert.DeserializeObject<List<CurrencyPrice>>(responce!.Content!.ToString());
                
                if (prisesList is not null && prisesList.Count > 0)
                {

                    foreach (var item in prisesList)
                    {
                        item.Ratio = Math.Round(item.Ratio, 2);
                    }

                    CollectionViewPrices.ItemsSource = prisesList;
                }
            }

        }
        catch (Exception)
        {

            throw;
        }
    }
}