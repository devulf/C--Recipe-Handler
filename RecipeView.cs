using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Receptsamling
{

    class RecipeView
    {

        //ALla recept
        public void Render(IList<Recipe> recipes)
        {
            foreach (Recipe recipe in recipes)
            {
                Render(recipe);
            }
        }


        //Presenterar de olika recepten som går att välja 
        public void Render(Recipe recipe)
        {
            RenderHeader(recipe.Name);
            Console.WriteLine("=========================================");
            foreach (Ingredient i in recipe.Ingredients)
            {
                Console.WriteLine("{0}{1} {2}", i.Amount, i.Measure, i.Name);
           
            }
           
            Console.WriteLine("=========================================");
            Console.WriteLine();
            foreach (string i in recipe.Directions)
            {
                Console.WriteLine(i);
            }
            Console.WriteLine();
        }

        //Recepttitel
        void RenderHeader(string header)
        {
            Console.WriteLine("------------------------------");
            Console.BackgroundColor = ConsoleColor.Red;
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine();
            Console.WriteLine("{0}", header);
            Console.WriteLine();
            Console.ResetColor();
        }

    }
}




