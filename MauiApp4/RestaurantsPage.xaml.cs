using MauiApp4.Models;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using System.Collections.ObjectModel;

namespace MauiApp4;

public partial class RestaurantsPage : ContentPage
{
    public ObservableCollection<Restaurant> Restaurants { get; } = new ObservableCollection<Restaurant>();

    public RestaurantsPage()
    {
        InitializeComponent();
        Restaurants.Add(new Restaurant { Name = "D'licious", TipPct = 12, ImageUrl = "resurant1.jpg", Rating = 5 });
        Restaurants.Add(new Restaurant { Name = "Torve-hallen", TipPct = 10, ImageUrl = "resurant2.jpg", Rating = 4 });
        Restaurants.Add(new Restaurant { Name = "Ristorante Fratelli", TipPct = 5, ImageUrl = "resurant3.jpg", Rating = 4 });
        Restaurants.Add(new Restaurant { Name = "Restaurant Colosseum", TipPct = 15, ImageUrl = "resurant4.jpg", Rating = 4 });
        Restaurants.Add(new Restaurant { Name = "Restaurant Flammen", TipPct = 20, ImageUrl = "resurant5.jpg", Rating = 4 });

        BindingContext = this;
    }

    private async void AddToolbarItem_Clicked(object sender, EventArgs e)
    {
        string name = await DisplayPromptAsync("Add Restaurant", "Name:", "OK", "Cancel", "", -1, Keyboard.Default, "restaurant");
        if (string.IsNullOrWhiteSpace(name))
            return;

        double defaultTip = Preferences.Get("DefaultTipPct", 15.0);
        string tipDefaultStr = defaultTip.ToString();
        string tipStr = await DisplayPromptAsync("Tip percent", "Tip percent (e.g. 15):", "OK", "Cancel", tipDefaultStr, -1, Keyboard.Numeric, tipDefaultStr);
        double tip = defaultTip;
        if (!string.IsNullOrWhiteSpace(tipStr) && double.TryParse(tipStr, out var parsed))
            tip = parsed;

        string ratingStr = await DisplayPromptAsync("Rating", "Rating 0-5:", "OK", "Cancel", "0", -1, Keyboard.Numeric, "0");
        int rating = 0;
        if (!string.IsNullOrWhiteSpace(ratingStr) && int.TryParse(ratingStr, out var parsedRating))
            rating = Math.Max(0, Math.Min(5, parsedRating));

        Restaurants.Add(new Restaurant { Name = name, TipPct = tip, ImageUrl = "https://via.placeholder.com/100", Rating = rating });
    }



    private async void DeleteSwipeItem_Invoked(object sender, EventArgs e)
    {
        if (sender is SwipeItem swipeItem && swipeItem.BindingContext is Restaurant r)
        {
            if (await DisplayAlert("Delete", $"Delete {r.Name}?", "Yes", "No"))
                Restaurants.Remove(r);
        }
    }

    private async void RestaurantsView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.Count == 0) return;

        if (e.CurrentSelection[0] is not Restaurant r) return;

        string ratingStr = await DisplayPromptAsync("Set Rating", "Rating 0-5:", "OK", "Cancel", r.Rating.ToString(), -1, Keyboard.Numeric, r.Rating.ToString());
        if (!string.IsNullOrWhiteSpace(ratingStr) && int.TryParse(ratingStr, out var rating))
        {
            rating = Math.Max(0, Math.Min(5, rating));
            r.Rating = rating;
        }

        RestaurantsView.SelectedItem = null;
    }
}
