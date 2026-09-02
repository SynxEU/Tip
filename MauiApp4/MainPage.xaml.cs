using System.Globalization;
using MauiApp4.Models;

namespace MauiApp4;

public partial class MainPage : ContentPage
{
    public Tip Tip { get; set; }

    public MainPage()
    {
        InitializeComponent();
        Tip = new Tip();
        BindingContext = Tip;
    }

    private void AmountEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        Tip.BillAmount = AmountEntry.Text ?? string.Empty;
    }

    private async void AboutToolbarItem_Clicked(object sender, EventArgs e) =>
        await Shell.Current.Navigation.PushAsync(new AboutPage(), true);

    private async void FeedbackToolbarItem_Clicked(object sender, EventArgs e) =>
        await Shell.Current.Navigation.PushModalAsync(new FeedbackPage(), true);

    private void TipSlider_ValueChanged(object sender, ValueChangedEventArgs e)
    {
        Tip.TipPct = e.NewValue;
    }

    private async void TipButton_Clicked(object sender, EventArgs e)
    {
        if (sender is not Button b || !double.TryParse(b.Text.Replace("%", ""), out double p))
            return;

        if (Math.Abs(p - 20) < 0.001)
        {
            if (await DisplayAlert("Generøs tip", "Vil du give 20% i drikkepenge?", "Yes", "No"))
                Tip.TipPct = 20;
            return;
        }

        await DisplayAlert("Tip", $"Du har valgt {p}% tip.", "OK");
        Tip.TipPct = p;
    }

    private async void CurrencyButton_Clicked(object sender, EventArgs e)
    {
        var old = Tip.Culture;
        string choice = await DisplayActionSheet("Vælg valuta", "Cancel", null, "Kr.", "Euro", "Dollars");

        var culture = choice switch
        {
            "Kr." => CultureInfo.CreateSpecificCulture("da-DK"),
            "Euro" => CultureInfo.CreateSpecificCulture("de-DE"),
            "Dollars" => CultureInfo.CreateSpecificCulture("en-US"),
            _ => old
        };

        if (culture == old) return;

        CurrencySymbolLabel.Text = culture.NumberFormat.CurrencySymbol;
        Tip.SetCulture(culture);
    }
}
