using HowToBBQ.Win10.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HowToBBQ.Win10.Services.RecipeServices
{
    public interface IRecipeServices
    {
        ObservableCollection<BBQRecipe> GetRecipes();
        BBQRecipe Get(string id);
        void Remove(string id);
        void Save(BBQRecipe item);
    }
}
