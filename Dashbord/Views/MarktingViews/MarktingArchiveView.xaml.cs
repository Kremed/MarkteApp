global using MarketApp.Shered;
using MarketApp.Shered.Dtos;
namespace Dashbord.Views.MarktingViews
{
    public partial class MarktingArchiveView : ContentPage
    {
        public MarktingArchiveView()
        {
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            await GetAds();
            base.OnAppearing();
        }
        private async void BtnGoToCreateView_Clicked(object sender, EventArgs e)
        {
            try
            {
                await Navigation.PushModalAsync(new CreateAdView());
            }
            catch (Exception ex)
            {
                await DisplayAlert("Exception !!", $":تعرض التطبيق لخطاء ما, الرجاء اعادة المحاولة {Environment.NewLine} {ex.Message}", "موافق");
            }
        }

        public async Task GetAds()
        {
            try
            {

                var client = new RestClient();

                var request = new RestRequest("https://maui.ly/api/Markting/admin/getAds", Method.Get);

                RestResponse response = await client.ExecuteAsync(request);

                if (response.IsSuccessStatusCode &&
                    !string.IsNullOrEmpty(response.Content))
                {
                    var marketingAdsList = JsonConvert.DeserializeObject<List<MarketingAd>>(response.Content);

                    foreach (var item in marketingAdsList)
                    {
                        if (item.IsActive is true)
                        {
                            item.StringActiveSatus = "حالة مفعلة";
                            item.colorCode = "#DB0808";
                        }
                        else
                        {
                            item.StringActiveSatus = "حالة غير مفعلة";
                            item.colorCode = "#1ADB08";
                        }
                    }

                    AdsCollectionView.ItemsSource = marketingAdsList;
                }
                else
                {
                    await DisplayAlert("لم يتم استجلاب البيانات", response.Content, "موافق");
                }
            }
            catch (Exception ex)
            {

                await DisplayAlert("Exception On GetAds() :", ex.Message, "OK");
            }
        }

        private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
        {
            try
            {
                var SelectedId = e.Parameter;
                if (SelectedId is null)
                {
                    await DisplayAlert("معرف غير موجود", "الرجاء اختيار عنصر من القائمة", "OK");
                    return;
                }

                var clinet = new RestClient();

                var request = new RestRequest($"https://maui.ly/api/Markting/admin/deactivateAd", Method.Post);

                request.AddQueryParameter("id", SelectedId.ToString());

                var responce = await clinet.ExecuteAsync(request);

            }
            catch (Exception ex)
            {
                await DisplayAlert("Exception:", ex.Message, "OK");
            }
        }
    }
}