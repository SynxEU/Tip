using System.Globalization;
using System.ComponentModel;
using MauiApp4.Models;
using MauiApp4.Converters;
using Microsoft.Maui.Graphics;

namespace MauiApp4;

public partial class MainPage : ContentPage
{
    public Tip Tip { get; set; }

    public MainPage()
    {
        InitializeComponent();
        Tip = new Tip();
        BindingContext = Tip;
        Tip.PropertyChanged += Tip_PropertyChanged;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        Tip.TipPct = Settings.GetDefaultTipPct();
    }

    private void Tip_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(Tip.TipPct))
        {
            AnimateTipColor(Tip.TipPct);
        }
    }

    private void AnimateTipColor(double newPct)
    {
        try
        {
            var conv = new TipPctToColorConverter();
            var toColor = (Color)conv.Convert(newPct, typeof(Color), null, CultureInfo.CurrentCulture);

            var fromColor = TipPercentageLabel.TextColor;

            var animation = new Microsoft.Maui.Controls.Animation(v =>
            {
                var blended = BlendColors(fromColor, toColor, v);
                TipPercentageLabel.TextColor = blended;
                TipSlider.MinimumTrackColor = blended;
            }, 0, 1);

            // Commit animation: owner, name, rate (ms per frame), length (ms), easing
            animation.Commit(this, "TipColorChange", 16u, 250u, Microsoft.Maui.Easing.Linear);
        }
        catch
        {
            // fallback: set color directly
            var conv = new TipPctToColorConverter();
            TipPercentageLabel.TextColor = (Color)conv.Convert(newPct, typeof(Color), null, CultureInfo.CurrentCulture);
        }
    }

    private Color BlendColors(Color a, Color b, double t)
    {
        float f = (float)Math.Clamp(t, 0.0, 1.0);
        return new Color(
            a.Red + (b.Red - a.Red) * f,
            a.Green + (b.Green - a.Green) * f,
            a.Blue + (b.Blue - a.Blue) * f,
            a.Alpha + (b.Alpha - a.Alpha) * f);
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
