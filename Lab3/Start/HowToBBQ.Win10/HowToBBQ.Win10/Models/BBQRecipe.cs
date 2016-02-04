using System;
using Template10.Mvvm;

namespace HowToBBQ.Win10.Models
{
    public class BBQRecipe : BindableBase
    {
        string id;
        public string Id
        {
            get { return id; }
            set
            {
                Set(ref id, value);
            }
        }

        string name;
        public string Name {

            get { return name; }
            set
            {
                Set(ref name, value);
            }

        }

        string shortDesc;
        public string ShortDesc
        {
            get { return shortDesc; }

            set
            {
                Set(ref shortDesc, value);
            }
        }


        string ingredients;
        public string Ingredients {

            get { return ingredients;  }

            set
            {
                Set(ref ingredients, value);
            }
        }

        string directions;
        public string Directions {

            get { return directions; }

            set
            {
                Set(ref directions, value);
            }

        }

        int prepTime;
        public int PrepTime
        {
            get { return prepTime; }

            set
            {
                Set(ref prepTime, value);
            }
        }

        int totalTime;
        public int TotalTime
        {
            get { return totalTime; }

            set
            {
                Set(ref totalTime, value);
            }
        }

        int serves;
        public int Serves
        {
            get { return serves; }

            set
            {
              Set(ref serves, value); 
            }

        }

        string imagePath;
        public string ImagePath
        {
            get { return imagePath; }
            set
            {
                Set(ref imagePath, value);
            }
        }

        public Uri ImageUri
        {
            get
            {
                if (!string.IsNullOrEmpty(ImagePath)) return new Uri(ImagePath, UriKind.RelativeOrAbsolute);
                else return null;
            }
        }
    }
}
