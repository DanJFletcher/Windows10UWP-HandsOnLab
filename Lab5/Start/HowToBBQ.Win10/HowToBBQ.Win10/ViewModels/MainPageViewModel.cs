using Template10.Mvvm;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Template10.Services.NavigationService;
using Windows.UI.Xaml.Navigation;
using System.Collections.ObjectModel;
using HowToBBQ.Win10.Models;
using Windows.UI.Xaml.Controls;

namespace HowToBBQ.Win10.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {

        ObservableCollection<Models.BBQRecipe> recipes = default(ObservableCollection<Models.BBQRecipe>);
        public ObservableCollection<Models.BBQRecipe> Recipes { get { return recipes; } private set { Set(ref recipes, value); } }

        string _searchText = default(string);
        public string SearchText { get { return _searchText; } set { Set(ref _searchText, value); } }

        Models.BBQRecipe selected = default(Models.BBQRecipe);

        public MainPageViewModel()
        {
            if (recipes == null)
            {
                recipes = App.RecipeService.GetRecipes();
            }
        }


        public BBQRecipe Selected
        {
            get { return selected; }
            set
            {
                var recipe = value as Models.BBQRecipe;
                Set(ref selected, recipe);
            }
        }

        public void GotoDetailsPage(object sender, object parameter)
        {
            var arg = parameter as ItemClickEventArgs;
            var item = arg.ClickedItem as BBQRecipe;
            selected = item;
            NavigationService.Navigate(typeof(Views.DetailPage), Selected.Id);

        }

        public void GotoSettings() =>
            NavigationService.Navigate(typeof(Views.SettingsPage), 0);

        public void GotoPrivacy() =>
            NavigationService.Navigate(typeof(Views.SettingsPage), 1);

        public void GotoAbout() =>
            NavigationService.Navigate(typeof(Views.SettingsPage), 2);

    }
}

