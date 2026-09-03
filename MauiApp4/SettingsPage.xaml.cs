using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using MauiApp4.Models;

namespace MauiApp4;

public partial class SettingsPage : ContentPage
{
    public SettingsPage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        double current = Settings.GetDefaultTipPct();
        TipEntry.Text = current.ToString();
        Default15Switch.IsToggled = Math.Abs(current - 15.0) < 0.0001;
    }

    private void Default15Switch_Toggled(object sender, ToggledEventArgs e)
    {
        if (e.Value)
        {
            TipEntry.Text = "15";
            Settings.SetDefaultTipPct(15.0);
        }
    }

    private async void SaveButton_Clicked(object sender, EventArgs e)
    {
        if (double.TryParse(TipEntry.Text, out var parsed))
        {
            Settings.SetDefaultTipPct(parsed);
            Default15Switch.IsToggled = Math.Abs(parsed - 15.0) < 0.0001;
            await DisplayAlert("Settings", "Saved", "OK");
        }
        else
        {
            await DisplayAlert("Error", "Invalid tip percent", "OK");
        }
    }
}
