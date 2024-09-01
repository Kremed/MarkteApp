
using MarketApp.Shered;
using Newtonsoft.Json;
using RestSharp;

namespace EndUser
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        [Obsolete]
        public MainPage()
        {
            InitializeComponent();

            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await GetAds();
            });

            MainThread.BeginInvokeOnMainThread(() =>
            {
                
                Device.StartTimer(TimeSpan.FromSeconds(2), (Func<bool>)(() =>
                {
                    if (AdsCarouselView.Position == (count - 1))
                        AdsCarouselView.Position = 0;
                    else
                        AdsCarouselView.Position += 1;
                    return true;
                }));
            });
        }

        public async Task GetAds()
        {
            try
            {
                var client = new RestClient();

                var request = new RestRequest("https://maui.ly/api/Markting/getAds", Method.Get);

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

                    AdsCarouselView.ItemsSource = marketingAdsList;

                    count = marketingAdsList.Count();
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


    }

}
