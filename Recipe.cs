using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace Receptsamling
{
    class Recipe : IComparable, IComparable<Recipe>
    {
        private IList<string> _directions;
        private IList<Ingredient> _ingredients;
        private string _name;
        

        //Egenskaper
        public ReadOnlyCollection<string>  Directions 
        {
            get
            {
                return new ReadOnlyCollection<string> (_directions);
            }
            set
              {
                this._directions = value;
            }
        }

        public ReadOnlyCollection<Ingredient> Ingredients
        {
            get
                { 
                    return new ReadOnlyCollection<Ingredient>(_ingredients); 
                }
            set
            {
                this._ingredients = value;
            }
        }

        public string Name
        {
            get {
                return _name; 
            }
            set
            {
                if (String.IsNullOrWhiteSpace(value))
                {
                    throw new Exception();
                }
                this._name = value;
            }
        }

        //Konstruktorer
        public void Add(Ingredient ingredient)
        {
            _ingredients.Add(ingredient);
        }

        public void Add(string direction)
        {
            _directions.Add(direction);
        }

        public int CompareTo(object obj)
        {
            if (obj == null)
            {
                return 1;
            }

            Recipe other = obj as Recipe;
            if (other == null)
            {
                throw new ArgumentException();
            }
            return Name.CompareTo(other.Name);


        }

        public int CompareTo(Recipe other)
        {
            if (other == null)
            {
                return 1;
            }
            return Name.CompareTo(other.Name);
        }

        public Recipe(string name)
        {
            this.Name = name;
            _ingredients = new List<Ingredient>();
            _directions = new List<string>();
        }

        public Recipe(string name, IList<Ingredient> ingredients, IList <string> directions)
        {
            if (directions != null)
            {
                this._directions = directions.ToList();
            }
            else
            {
                _directions = new List<string>();
            }
            this._ingredients = ingredients != null ? ingredients.ToList() : new List<Ingredient>();
            Name = name;
            

        }


    }
}