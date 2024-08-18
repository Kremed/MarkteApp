


namespace Dashbord.Views.CurrenciesViews;


public partial class CreateCurrencyView : ContentPage
{
    public CreateCurrencyView()
    {
        InitializeComponent();
    }

    private async void TxtImageUrl_Unfocused(object sender, FocusEventArgs e)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(TxtImageUrl.Text) ||
               !TxtImageUrl.Text.ToLower().StartsWith("https") ||
               !(TxtImageUrl.Text.ToLower().EndsWith(".png") || TxtImageUrl.Text.ToLower().EndsWith(".jpg")))
            {
                await DisplayAlert("صورة العملة", "الرجاء ادخال رابط صورة صالح, من المفضل ان تكون الصورة بحجم 512 بكسل طولي و 512 بكسل عرضي", "موافق");
                TxtImageUrl.Text = "";
                ImgCurrencyUrl.Source = null;
                return;
            }

            ImgCurrencyUrl.Source = TxtImageUrl.Text;
        }
        catch
        {
        }
    }

    private void SwActiveState_Toggled(object sender, ToggledEventArgs e)
    {
        try
        {
            if (SwActiveState.IsToggled == true)
            {
                LblActiveState.Text = "انشاء العملة بحالة فعـالة";

                LblActiveState.TextColor = Colors.Green;
            }
            else if (SwActiveState.IsToggled == false)
            {
                LblActiveState.Text = "انشاء العملة بحالة غير فعـالة";

                LblActiveState.TextColor = Colors.Red;
            }
        }
        catch
        {
        }
    }

    private async void BtnCreateCurrency_Clicked(object sender, EventArgs e)
    {
        try
        {
            string? aaa = TxtCode.Text?.ToUpper().Trim();

            Currency currency = new Currency()
            {
                Name = TxtName.Text.Trim(),
                Description = TxtDiscrption.Text.Trim(),
                Code = TxtCode.Text.ToUpper().Trim(),
                ImageUrl = TxtImageUrl.Text.ToLower().Trim(),
                IsActive = SwActiveState.IsToggled,
            };

            var result = Currency.Validate(currency);

            if (result != "OK")
            {
                await DisplayAlert("الرجاء تأكيد البيانات", result, "OK");
                return;
            }


            //(1) Call API Endpoint via Rest Sharp NuGet Packege + Newtonsoft.Json =>

            var client = new RestClient();

            var request = new RestRequest("https://maui.ly/api/Currency/admin/createCurrency", Method.Post);

            var json = JsonConvert.SerializeObject(currency);

            request.AddJsonBody(json);

            request.AddHeader("Content-Type", "application/json");

            RestResponse response = await client.ExecuteAsync(request);

            if (response.IsSuccessful)
            {
                TxtName.Text = "";
                TxtDiscrption.Text = "";
                TxtCode.Text = "";
                TxtCode.Text = "";
                SwActiveState.IsToggled = true;
                await DisplayAlert("عملية ناجحة", $"تمت أضافة العملة بنجاح : {currency.Name}", "موافق");
            }
            else
            {
                await DisplayAlert("عملية فاشلة", response.Content, "موافق");
            }

            //(2) Call API Endpoint via HttpClient By Microsoft + Newtonsoft.Json =>

            //var client = new HttpClient();

            //var request = new HttpRequestMessage(HttpMethod.Post,
            //    "https://maui.ly/api/Currency/admin/createCurrency");

            //var json = JsonConvert.SerializeObject(currency);

            //request.Content = new StringContent(json, Encoding.UTF8, "application/json");

            //var response = await client.SendAsync(request);
        }
        catch (Exception ex)
        {
        }
    }
}