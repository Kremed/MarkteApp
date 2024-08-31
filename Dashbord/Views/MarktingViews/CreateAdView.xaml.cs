namespace Dashbord.Views.MarktingViews;

public partial class CreateAdView : ContentPage
{
    public CreateAdView()
    {
        InitializeComponent();
    }
    public FileResult imageFile { get; set; } = null!;
    private async void BtnUplodImageFile_Clicked(object sender, EventArgs e)
    {
        try
        {
            var result = await FilePicker.PickAsync(new PickOptions
            {
                FileTypes = FilePickerFileType.Images,
                PickerTitle = "Select an image"
            });

            if (result != null)
            {
                imageFile = result;
                ImgAdCover.Source = result.FullPath;
                ImgAdCover.IsVisible = true;
            }
            else
            {
                imageFile = null;
                ImgAdCover.Source = "";
                ImgAdCover.IsVisible = false;
            }
        }
        catch (Exception ex)
        {

        }
    }

    private async void BtnCallApi_Clicked(object sender, EventArgs e)
    {
        try
        {
            if (DpFromDate.Date < DateTime.Now.Date)
            {
                await DisplayAlert("خطاء مدخلات", "لايمكن أضافة اعلان بتـاريخ رجعي, الرجاء التأكد من تاريخ بداية الاعلان.", "موافق");
                return;
            }

            if (DpToDate.Date < DpFromDate.Date)
            {
                await DisplayAlert("خطاء مدخلات", "تاريخ نهاية الاعلان يجب ان يكون اكبر من تاريخ البداية.", "موافق");
                return;
            }

            if (string.IsNullOrWhiteSpace(TxtTitle.Text) ||
                string.IsNullOrWhiteSpace(TxtDescription.Text) ||
                string.IsNullOrWhiteSpace(TxtUrl.Text))
            {
                await DisplayAlert("خطاء مدخلات", "الرجاء التـــأكد من عنوان الاعلان والوصف ورابط الاعلان.", "موافق");
                return;
            }

            if (imageFile is null)
            {
                await DisplayAlert("خطاء مدخلات", "الرجاء تحميل ملف صورة الاعلان.", "موافق");
                return;
            }


            var client = new RestClient();

            var request = new RestRequest("https://maui.ly/api/Markting/admin/createAd", Method.Post);

            request.AddParameter("StringActiveSatus", "AnyThing");
            request.AddParameter("Title", TxtTitle.Text);
            request.AddParameter("Description", TxtDescription.Text);
            request.AddParameter("FromDate", Convert.ToDateTime(DpFromDate.Date).ToString("yyyy-MM-ddTHH:mm:ss"));
            request.AddParameter("ToDate", Convert.ToDateTime(DpToDate.Date).ToString("yyyy-MM-ddTHH:mm:ss"));
            request.AddParameter("IsActive", true);
            request.AddParameter("CoverImage", "CoverImage");
            request.AddParameter("Url", TxtUrl.Text);

            request.AddFile("file", imageFile.FullPath, "image/jpeg");

            var responce = await client.ExecuteAsync(request);

            if (responce.IsSuccessStatusCode)
            {
                await Navigation.PopModalAsync();
            }
            else
            {
                await DisplayAlert("Error API Call ",responce.Content!.ToString(),"OK");
            }

        }
        catch (Exception)
        {
        }
    }
}