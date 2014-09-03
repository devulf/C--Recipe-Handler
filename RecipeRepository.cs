using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Receptsamling
{
    class RecipeRepository
    {
        enum RecipeReadStatus
        {
            Indefinite, New, Ingredient, Direction
        }

        private string _path;

        public string Path
        {
            get
            {
                return this._path;
            }
            set
            {
                _path = value;
            }
        }

        //Konstruktor
        public RecipeRepository(string path)
        {
            this.Path = path;
        }


        //Läser in fil med rätt format.
        public IList<Recipe> Load()
        {
           

            RecipeReadStatus status = RecipeReadStatus.Indefinite;
            StreamReader reader = new StreamReader(_path);

            IList<Recipe> rec = new List<Recipe>();
            IList<Ingredient> ingredientsList = new List<Ingredient>();
            IList<string> directionsList = new List<string>();

            string name = null;
            string line = "";
            while ((line = reader.ReadLine()) != null)
            {

                
                if (line == "[Recept]")
                {
                    status = RecipeReadStatus.New;
                }


                else if (line == "[Ingredienser]")
                {
                    status = RecipeReadStatus.Ingredient;
                }



                else if (line == "[Instruktioner]")
                {
                    status = RecipeReadStatus.Direction;
                }

                else
                {
                    if (status == RecipeReadStatus.New)
                    {
                        name = line;
                        rec.Add(new Recipe(line));
                    }

                    else if (status == RecipeReadStatus.Ingredient)
                    {
                        string[] splitText = line.Split(';');
                        if (splitText.Length != 3)
                        {
                            throw new ArgumentException("Inte ett recept i giltigt format!");
                        }

                        string[] rowForm = line.Split(';');

                        Ingredient ingredient = new Ingredient { Name = rowForm[2], Amount = rowForm[0], Measure = rowForm[1] };
                        rec.Last().Add(ingredient);
                    }


                    else if (status == RecipeReadStatus.Direction)
                    {
                        rec.Last().Add("{}" + line);
                    }
                }

            }
            rec = rec.OrderBy(res => res.Name).ToList();
            reader.Close();
            return rec;
        }


        //Används för att spara fil.
        public void Save(IList<Recipe> recipes)
        {
            using (StreamWriter saver = new StreamWriter(_path))
            {
                if (recipes == null)
                {
                    throw new Exception("Inga recept att spara");
                }

                for (int i = 0; i < recipes.Count(); i++)
                {
                    saver.WriteLine("[Recept]");
                    saver.WriteLine(recipes[i].Name);
                    saver.WriteLine("[Ingredienser]");
                    for (int x = 0; x < recipes[i].Ingredients.Count(); x++)
                    {
                        saver.WriteLine(recipes[i].Ingredients[x]);
                    }
                    saver.WriteLine("[Instruktioner]");
                    for (int y = 0; y < recipes[i].Directions.Count(); y++)
                    {
                        saver.WriteLine(recipes[i].Directions[y]);
                    }
                }
                saver.Close();
            }
        }
    }
}
