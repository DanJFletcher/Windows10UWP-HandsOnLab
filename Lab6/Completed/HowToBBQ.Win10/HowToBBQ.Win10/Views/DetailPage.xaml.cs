using HowToBBQ.Win10.ViewModels;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using System.Threading.Tasks;
using System;

namespace HowToBBQ.Win10.Views
{
    public sealed partial class DetailPage : Page
    {
        public DetailPage()
        {
            InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Disabled;
        }

        private async void ButtonFilePick_Click(object sender, RoutedEventArgs e)
        {
            await GetImage(false);
        }


        private async void ButtonCamera_Click(object sender, RoutedEventArgs e)
        {
            await GetImage(true);
        }

        async Task GetImage(bool useCamera)
        {
            try
            {
                var detailViewModel = this.DataContext as DetailPageViewModel;
                BBQImage.Source = await detailViewModel.SelectImage(useCamera);
                BBQImage.Visibility = (BBQImage.Source == null) ? Visibility.Visible : Visibility.Visible;
            }
            catch (Exception ex)
            {
                throw;
            }

        }
    }
}

