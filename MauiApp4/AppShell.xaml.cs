namespace MauiApp4
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(DetailsPage), typeof(DetailsPage));
        }

        private async void OnFlyoutMenuItemClicked(object sender, EventArgs e)
        {
            if (sender is MenuItem mi && mi.CommandParameter is string route)
            {
                await Shell.Current.GoToAsync(route);
            }
        }
    }
}
