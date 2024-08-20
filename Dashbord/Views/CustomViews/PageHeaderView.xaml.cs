namespace Dashbord.Views.CustomViews;

public partial class PageHeaderView : ContentView
{
    public PageHeaderView()
    {
        InitializeComponent();
    }

    private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
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