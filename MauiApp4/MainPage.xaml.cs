using System.Globalization;

namespace MauiApp4
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void AmountEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            CalculateTip();
        }

        private void TipSlider_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            TipPercentageLabel.Text = $"{e.NewValue:0}%";

            CalculateTip();
        }

        private void Tip10Button_Clicked(object sender, EventArgs e)
        {
            TipSlider.Value = 10;
        }

        private void Tip25Button_Clicked(object sender, EventArgs e)
        {
            TipSlider.Value = 25;
        }

        private void Tip35Button_Clicked(object sender, EventArgs e)
        {
            TipSlider.Value = 35;
        }

        private void Tip50Button_Clicked(object sender, EventArgs e)
        {
            TipSlider.Value = 50;
        }

        private void Tip75Button_Clicked(object sender, EventArgs e)
        {
            TipSlider.Value = 75;
        }

        private void CalculateTip()
        {
            if (!double.TryParse(
                    AmountEntry.Text,
                    NumberStyles.Any,
                    CultureInfo.CurrentCulture,
                    out double amount))
            {
                TipLabel.Text = 0.0.ToString(
                    "C",
                    CultureInfo.CreateSpecificCulture("da-DK"));

                TotalLabel.Text = 0.0.ToString(
                    "C",
                    CultureInfo.CreateSpecificCulture("da-DK"));

                return;
            }

            double tip = amount * (TipSlider.Value / 100);
            double total = amount + tip;

            CultureInfo danishCulture =
                CultureInfo.CreateSpecificCulture("da-DK");

            TipLabel.Text = tip.ToString("C", danishCulture);
            TotalLabel.Text = total.ToString("C", danishCulture);
        }

        private void RoundDownButton_Clicked(object sender, EventArgs e)
        {
            if (!TryCalculateTotal(out double total))
                return;

            double roundedDown = Math.Floor(total / 10) * 10;

            RoundDownLabel.Text = roundedDown.ToString(
                "C",
                CultureInfo.CreateSpecificCulture("da-DK"));
        }

        private void RoundUpButton_Clicked(object sender, EventArgs e)
        {
            if (!TryCalculateTotal(out double total))
                return;

            double roundedUp = Math.Ceiling(total / 10) * 10;

            RoundUpLabel.Text = roundedUp.ToString(
                "C",
                CultureInfo.CreateSpecificCulture("da-DK"));
        }

        private bool TryCalculateTotal(out double total)
        {
            total = 0;

            if (!double.TryParse(
                    AmountEntry.Text,
                    NumberStyles.Any,
                    CultureInfo.CurrentCulture,
                    out double amount))
            {
                return false;
            }

            double tip = amount * (TipSlider.Value / 100);

            total = amount + tip;

            return true;
        }
    }
}
