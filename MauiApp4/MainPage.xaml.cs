using System.Globalization;

namespace MauiApp4
{
    public partial class MainPage : ContentPage
    {
        private static readonly CultureInfo _danishCulture = CultureInfo.CreateSpecificCulture("da-DK");

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

        private void TryUpdateLabels(double tip, double total)
        {
            TipLabel.Text = tip.ToString("C", _danishCulture);
            TotalLabel.Text = total.ToString("C", _danishCulture);
            RoundedLabel.Text = Math.Round(total, MidpointRounding.AwayFromZero).ToString("C", _danishCulture);
        }

        private void TipButton_Clicked(object sender, EventArgs e)
        {
            if (sender is Button button && double.TryParse(button.Text.Replace("%", ""), out double percentage))
            {
                TipSlider.Value = percentage;
            }
        }

        private void CalculateTip()
        {
            if (!double.TryParse(
                    AmountEntry.Text,
                    NumberStyles.Any,
                    CultureInfo.CurrentCulture,
                    out double amount))
            {
                TryUpdateLabels(0.0, 0.0);
                return;
            }

            double tip = amount * (TipSlider.Value / 100);
            double total = amount + tip;

            TryUpdateLabels(tip, total);
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
