using ActivityAssistent.App.Models;
using ActivityAssistent.App.PageModels;

namespace ActivityAssistent.App.Pages
{
    public partial class MainPage : ContentPage
    {
        public MainPage(MainPageModel model)
        {
            InitializeComponent();
            BindingContext = model;
        }
    }
}