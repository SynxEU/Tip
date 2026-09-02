using Microsoft.Maui.Controls;

namespace MauiApp4
{
    public partial class FeedbackPage : ContentPage
    {
        public FeedbackPage()
        {
            InitializeComponent();
        }

        private async void CloseButton_Clicked(object sender, EventArgs e)
            => await (Shell.Current.Navigation.ModalStack.Count > 0
                ? Shell.Current.Navigation.PopModalAsync(true)
                : Shell.Current.GoToAsync("..", true));

        private async void DetailsButton_Clicked(object sender, EventArgs e)
        {
            string name = NameEntry?.Text ?? string.Empty;
            string feedback = FeedbackEditor?.Text ?? string.Empty;
            await Shell.Current.GoToAsync($"{nameof(DetailsPage)}?name={Uri.EscapeDataString(name)}&feedback={Uri.EscapeDataString(feedback)}", true);
        }
    }
}
