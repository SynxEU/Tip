using System.Globalization;

namespace MauiApp4;

public partial class MainPage : ContentPage
{
    private static readonly CultureInfo Da = CultureInfo.CreateSpecificCulture("da-DK");
    private CultureInfo _culture = Da;
    private const double Max = 99999.0;

    public MainPage() => InitializeComponent();

    private void AmountEntry_TextChanged(object sender, TextChangedEventArgs e) => _ = CalculateTipAsync();

    private async void AboutToolbarItem_Clicked(object sender, EventArgs e) =>
        await Shell.Current.Navigation.PushAsync(new AboutPage(), true);

    private async void FeedbackToolbarItem_Clicked(object sender, EventArgs e) =>
        await Shell.Current.Navigation.PushModalAsync(new FeedbackPage(), true);

    private void TipSlider_ValueChanged(object sender, ValueChangedEventArgs e)
    {
        TipPercentageLabel.Text = $"{e.NewValue:0}%";
        _ = CalculateTipAsync();
    }

    private void UpdateLabels(double tip, double total)
    {
        TipLabel.Text = tip.ToString("C", _culture);
        TotalLabel.Text = total.ToString("C", _culture);
        RoundedLabel.Text = Math.Round(total, MidpointRounding.AwayFromZero).ToString("C", _culture);
    }

    private async void TipButton_Clicked(object sender, EventArgs e)
    {
        if (sender is not Button b || !double.TryParse(b.Text.Replace("%", ""), out double p))
            return;

        if (Math.Abs(p - 20) < 0.001)
        {
            if (await DisplayAlert("Generøs tip", "Vil du give 20% i drikkepenge?", "Yes", "No"))
                TipSlider.Value = 20;
            return;
        }

        await DisplayAlert("Tip", $"Du har valgt {p}% tip.", "OK");
        TipSlider.Value = p;
    }

    private async Task CalculateTipAsync()
    {
        if (!double.TryParse(AmountEntry.Text, NumberStyles.Any, _culture, out double amount))
        {
            UpdateLabels(0, 0);
            return;
        }

        if (amount > Max)
        {
            await DisplayAlert("Limit", $"Maximum beløb er {Max:N0}.", "OK");
            amount = Max;
            AmountEntry.Text = amount.ToString("G", _culture);
        }

        double tip = amount * (TipSlider.Value / 100);
        UpdateLabels(tip, amount + tip);
    }

    private async void CurrencyButton_Clicked(object sender, EventArgs e)
    {
        CultureInfo old = _culture;
        string choice = await DisplayActionSheet("Vælg valuta", "Cancel", null, "Kr.", "Euro", "Dollars");

        _culture = choice switch
        {
            "Kr." => Da,
            "Euro" => CultureInfo.CreateSpecificCulture("de-DE"),
            "Dollars" => CultureInfo.CreateSpecificCulture("en-US"),
            _ => old
        };

        if (choice != "Kr." && choice != "Euro" && choice != "Dollars")
            return;

        CurrencySymbolLabel.Text = _culture.NumberFormat.CurrencySymbol;

        if (double.TryParse(AmountEntry.Text, NumberStyles.Any, old, out var amount))
            AmountEntry.Text = amount.ToString("G", _culture);

        await CalculateTipAsync();
    }
}
