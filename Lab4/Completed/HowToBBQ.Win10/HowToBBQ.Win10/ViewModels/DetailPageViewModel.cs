using HowToBBQ.Win10.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Common;
using Template10.Mvvm;
using Template10.Services.NavigationService;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Media.Capture;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Popups;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

namespace HowToBBQ.Win10.ViewModels
{
    public class DetailPageViewModel : ViewModelBase
    {

        private string recipeId = "0";

        private BBQRecipe recipe;
        public BBQRecipe Recipe { get { return recipe; } private set { Set(ref recipe, value); } }
        public DetailPageViewModel()
        {

            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
            {
                recipe = new BBQRecipe
                {
                    Name = "Dry Glazed Pork Tenderloin",
                    ShortDesc = "",
                    Ingredients = string.Concat("3 light brown sugar, packed", Environment.NewLine,
                                            "3 clove garlic, minced", Environment.NewLine,
                                            "1 tbsp finely grated orange zest", Environment.NewLine,
                                            "3 tbsp paprika", Environment.NewLine,
                                            "1 tbsp sesame seeds", Environment.NewLine,
                                            "1 tbsp ground ginger", Environment.NewLine,
                                            "1 tbsp ground coriander", Environment.NewLine,
                                            "2 tsp fine salt", Environment.NewLine,
                                            "2 tsp ground black pepper", Environment.NewLine,
                                            "1 tsp cream of tartar", Environment.NewLine,
                                            "3 1 lb. pork tenderloins"),
                    Directions = string.Concat("1. For dry glaze, stir brown sugar, garlic and orange zest to blend. In a separate bowl, stir remaining ingredients, then add to brown sugar mixture. Set aside until ready to use.", Environment.NewLine,
                                           "2. Clean pork tenderloin of any connective tissue. Preheat grill to medium and clean well. Rub tenderloins completely with dry glaze and immediately place on grill. Grill, uncovered, for about 8 minutes on each side until an internal temperature of 165°F is reached, rotating tenderloins 90° on each side. Let pork sit for a moment before slicing and serving."
                                           ),
                    Serves = 10,
                    PrepTime = 0,
                    TotalTime = 0,

                    ImagePath = "ms-appx:///Assets/DryGlazedPorkTenderloin.jpg"
                };
            }
            else
            {
                recipe = new BBQRecipe();
            }


        }

        public void SaveBBQRecipe()
        {

            App.RecipeService.Save(recipe);
            ShowMessage("Record has been saved", false);
        }

        public void DeleteBBQRecipe()
        {

            if (!String.IsNullOrEmpty(recipe.Id))
            {
                App.RecipeService.Remove(recipe.Id);
                ShowMessage("Record has been deleted", true);

            }

        }


        public override Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            if (state.Any())
            {
                // use cache value(s)
                if (state.ContainsKey(nameof(recipeId))) recipeId = state[nameof(recipeId)]?.ToString();
                // clear any cache
                state.Clear();
            }
            else
            {
                // use navigation parameter
                recipeId = parameter?.ToString();
                if (recipeId != null)
                {
                    Recipe = App.Recipes.Find(x => x.Id == recipeId);
                }
                else
                {
                    Recipe = new BBQRecipe();
                }
            }
            return Task.CompletedTask;
        }

        public override Task OnNavigatedFromAsync(IDictionary<string, object> state, bool suspending)
        {

            if (suspending)
            {
                // persist into cache
                state[nameof(recipeId)] = recipeId;
            }
            return base.OnNavigatedFromAsync(state, suspending);
        }

        public override Task OnNavigatingFromAsync(NavigatingEventArgs args)
        {
            args.Cancel = false;
            return Task.CompletedTask;
        }

        private async Task ShowMessage(string message, bool goBack = false)
        {
            var messageDialog = new MessageDialog(message);

            await messageDialog.ShowAsync();

            if (goBack) NavigationService.Navigate(typeof(Views.MainPage));
        }
    }
}

