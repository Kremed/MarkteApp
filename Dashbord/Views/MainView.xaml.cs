﻿using Dashbord.Views;
namespace Dashbord.Views;

public partial class MainView : ContentPage
{
    public MainView()
    {
        InitializeComponent();
    }

    private async void BrdCreateCurency_Tapped(object sender, TappedEventArgs e)
    {
        
        try
        {
            var ddd = Convert.ToInt32("12");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Exception !!", $":تعرض التطبيق لخطاء ما, الرجاء اعادة المحاولة {Environment.NewLine} {ex.Message}", "موافق");
        }
        finally
        {
            await Navigation.PushModalAsync(new CurrenciesViews.CreateCurrencyView());
        }
    }
}