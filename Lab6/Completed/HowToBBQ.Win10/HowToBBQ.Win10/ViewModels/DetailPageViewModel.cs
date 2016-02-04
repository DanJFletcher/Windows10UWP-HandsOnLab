using HowToBBQ.Win10.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        private WriteableBitmap bitmap;

        public async Task<WriteableBitmap> SelectImageFromPicker()
        {
            var picker = new FileOpenPicker();
            picker.ViewMode = PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".jpeg");
            picker.FileTypeFilter.Add(".png");

            StorageFile file = await picker.PickSingleFileAsync();

            if (file != null)
            {

                ImageProperties imgProp = await file.Properties.GetImagePropertiesAsync();
                var savedPictureStream = await file.OpenAsync(FileAccessMode.Read);

                //set image properties and show the taken photo
                bitmap = new WriteableBitmap((int)imgProp.Width, (int)imgProp.Height);
                await bitmap.SetSourceAsync(savedPictureStream);
                bitmap.Invalidate();

                SaveImageAsync(file);

                return bitmap;
            }
            else return null;
        }

        private async void SaveImageAsync(StorageFile file)
        {

            if (file != null)
            {
                StorageFile newImageFile = await file.CopyAsync(ApplicationData.Current.LocalFolder, Guid.NewGuid().ToString());

                recipe.ImagePath = newImageFile.Path;
            }
        }

        private async Task<WriteableBitmap> TakePicture()
        {
            CameraCaptureUI captureUI = new CameraCaptureUI();
            captureUI.PhotoSettings.Format = CameraCaptureUIPhotoFormat.Jpeg;
            captureUI.PhotoSettings.CroppedSizeInPixels = new Size(600, 600);

            StorageFile photo = await captureUI.CaptureFileAsync(CameraCaptureUIMode.Photo);

            if (photo != null)
            {

                WriteableBitmap bitmap = new WriteableBitmap(600, 600);
                IRandomAccessStream stream = await photo.OpenAsync(FileAccessMode.Read);
                bitmap.SetSource(stream);

                SaveImageAsync(photo);

                return bitmap;
            }

            return null;
        }

        public async Task<WriteableBitmap> SelectImage(bool useCamera)
        {
            if (useCamera) return await TakePicture();
            else return await SelectImageFromPicker();
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

        public void ShareBBQRecipe()
        {

            if (!String.IsNullOrEmpty(recipe.Id))
            {
                DataTransferManager.ShowShareUI();

            }

        }

        private async void DataTransferManager_DataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            DataRequestDeferral deferral = args.Request.GetDeferral();
            args.Request.Data.Properties.Title = recipe.Name;
            args.Request.Data.SetText(recipe.ShortDesc);

            StorageFile storageFile = await Windows.Storage.StorageFile.GetFileFromApplicationUriAsync(recipe.ImageUri);

            args.Request.Data.SetBitmap(RandomAccessStreamReference.CreateFromFile(storageFile));
            deferral.Complete();
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

