namespace Dashbord.Views.MarktingViews
{
    public partial class MarktingArchiveView : ContentPage
    {
        public MarktingArchiveView()
        {
            InitializeComponent();
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
    }
}