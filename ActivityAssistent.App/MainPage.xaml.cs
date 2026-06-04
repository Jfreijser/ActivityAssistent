using ActivityAssistent.App.Components;
using Microsoft.AspNetCore.Components.WebView.Maui;

namespace ActivityAssistent.App
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            blazorWebView.RootComponents.Add(
                new RootComponent
                {
                    Selector = "#app",
                    ComponentType = typeof(Routes)
                });
        }
    }
}
