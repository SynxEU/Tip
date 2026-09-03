using System.ComponentModel;

namespace MauiApp4.Models
{
    public class Restaurant : INotifyPropertyChanged
    {
        private string _imageUrl = string.Empty;
        private string _name = string.Empty;
        private double _tipPct;
        private int _rating;

        public event PropertyChangedEventHandler? PropertyChanged;

        public string ImageUrl
        {
            get => _imageUrl;
            set { if (value == _imageUrl) return; _imageUrl = value; OnPropertyChanged(nameof(ImageUrl)); }
        }

        public string Name
        {
            get => _name;
            set { if (value == _name) return; _name = value; OnPropertyChanged(nameof(Name)); }
        }

        public double TipPct
        {
            get => _tipPct;
            set { if (value == _tipPct) return; _tipPct = value; OnPropertyChanged(nameof(TipPct)); }
        }

        public int Rating
        {
            get => _rating;
            set { if (value == _rating) return; _rating = value; OnPropertyChanged(nameof(Rating)); }
        }

        private void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}

