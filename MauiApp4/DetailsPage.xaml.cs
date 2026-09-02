using Microsoft.Maui.Controls;
using System;

namespace MauiApp4;

[QueryProperty(nameof(Name), "name")]
[QueryProperty(nameof(Feedback), "feedback")]
public partial class DetailsPage : ContentPage
{
    public DetailsPage() => InitializeComponent();

    string _name;
    public string Name
    {
        get => _name;
        set { _name = value; WelcomeLabel.Text = $"Welcome, {_name}"; }
    }

    string _feedback;
    public string Feedback
    {
        get => _feedback;
        set
        {
            _feedback = value;
            if (!string.IsNullOrWhiteSpace(_feedback))
                PersonLabel.Text = _feedback;
        }
    }

    async void CloseButton_Clicked(object s, EventArgs e) => await Shell.Current.GoToAsync("..", true);

}
