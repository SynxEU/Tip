using System;
using System.ComponentModel;
using System.Globalization;

namespace MauiApp4.Models
{
    public class Tip : INotifyPropertyChanged
    {
        private string _billAmount = string.Empty;
        private string _tipAmount = "0,00 kr.";
        private string _totalAmount = "0,00 kr.";
        private string _roundedAmount = "0,00 kr.";
        private double _tipPct = 15;
        private CultureInfo _culture = CultureInfo.CreateSpecificCulture("da-DK");

        public event PropertyChangedEventHandler? PropertyChanged;

        public string BillAmount
        {
            get => _billAmount;
            set
            {
                if (value == _billAmount) return;
                _billAmount = value;
                OnPropertyChanged(nameof(BillAmount));
                CalculateTip();
            }
        }

        public string TipAmount
        {
            get => _tipAmount;
            private set { _tipAmount = value; OnPropertyChanged(nameof(TipAmount)); }
        }

        public string TotalAmount
        {
            get => _totalAmount;
            private set { _totalAmount = value; OnPropertyChanged(nameof(TotalAmount)); }
        }

        public string RoundedAmount
        {
            get => _roundedAmount;
            private set { _roundedAmount = value; OnPropertyChanged(nameof(RoundedAmount)); }
        }

        public double TipPct
        {
            get => _tipPct;
            set
            {
                if (Math.Abs(value - _tipPct) < 0.0001) return;
                _tipPct = value;
                OnPropertyChanged(nameof(TipPct));
                CalculateTip();
            }
        }

        public CultureInfo Culture
        {
            get => _culture;
            private set
            {
                _culture = value;
                OnPropertyChanged(nameof(Culture));
            }
        }

        public void SetCulture(CultureInfo culture)
        {
            if (culture == null) return;
            var old = _culture;
            _culture = culture;

            // try to preserve numeric value across cultures
            if (double.TryParse(_billAmount, NumberStyles.Any, old, out var amount))
                _billAmount = amount.ToString("G", _culture);

            OnPropertyChanged(nameof(BillAmount));
            CalculateTip();
        }

        public void CalculateTip()
        {
            const double Max = 99999.0;

            if (!double.TryParse(_billAmount, NumberStyles.Any, _culture, out double amount))
            {
                TipAmount = 0.ToString("C", _culture);
                TotalAmount = 0.ToString("C", _culture);
                RoundedAmount = 0.ToString("C", _culture);
                return;
            }

            if (amount < 0)
            {
                amount = 0;
                _billAmount = amount.ToString("G", _culture);
                OnPropertyChanged(nameof(BillAmount));
            }

            if (amount > Max)
            {
                amount = Max;
                _billAmount = amount.ToString("G", _culture);
                OnPropertyChanged(nameof(BillAmount));
            }

            double tip = amount * (TipPct / 100.0);
            double total = amount + tip;

            TipAmount = tip.ToString("C", _culture);
            TotalAmount = total.ToString("C", _culture);
            RoundedAmount = Math.Round(total, MidpointRounding.AwayFromZero).ToString("C", _culture);
        }

        private void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
