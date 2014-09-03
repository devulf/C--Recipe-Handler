using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.IO;

namespace Receptsamling
{
    class Program
    {


        static void Main(string[] args)
        {
            bool exit = false;

            IList<Recipe> recept = new List<Recipe>();
            do
            {
                switch (GetMenuChoice())
                {
                    case 0:

                        exit = true;
                        return;

                    case 1:

                        recept = LoadRecipes();
                        break;

                    case 2:

                        SaveRecipes(recept);
                        break;

                    case 3:

                        var recipe = CreateRecipe();
                        if (recipe != null)
                        {
                            recept.Add(recipe);
                            recept = recept.OrderBy(rec => rec.Name).ToList();
                        }
                        break;



                    case 4:

                        DeleteRecipe(recept);

                        break;

                    case 5:

                        ViewRecipe(recept);

                        break;

                    case 6:

                        ViewRecipe(recept, true);
                        break;

                    default:
                        continue;
                }

                ContinueOnKeyPressed();

            } while (!exit);
        }

        // Rensar konsollen och används för att komma tillbaka till huvudmenyn
        static void ContinueOnKeyPressed()
        {
            Console.BackgroundColor = ConsoleColor.DarkGreen;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();
            Console.WriteLine("======================================================");
            Console.WriteLine("|  Tryck på valfri tangent för att återgå till menyn |");
            Console.WriteLine("======================================================");
            Console.ResetColor();
            Console.ReadKey();
            Console.Clear();
        }

        private static int GetMenuChoice()
        {
            do
            {
                Console.Clear();
                Console.BackgroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine("============================");
                Console.WriteLine("=                          =");
                Console.WriteLine("=    Receptsamling         =");
                Console.WriteLine("=                          =");
                Console.WriteLine("============================");
                Console.ResetColor();

                Console.WriteLine("- Arkiv ------------------------------");
                Console.WriteLine("0. Avsluta.");
                Console.WriteLine("1. Öppna textfil med recept.");
                Console.WriteLine("2. Spara recept på textfil.");
                Console.WriteLine("-Redigera--------------------------");
                Console.WriteLine("3. Lägg till nytt recept.");
                Console.WriteLine("4. Ta bort recept.");
                Console.WriteLine("-Visa--------------------------");
                Console.WriteLine("5. Visa recept.");
                Console.WriteLine("6. Visa alla recept.");

                Console.WriteLine("-___________________________");
                Console.WriteLine("Ange menyval [0-6]:");

                int menuChoice;
                if (int.TryParse(Console.ReadLine(), out menuChoice) && menuChoice >= 0 && menuChoice <= 6)
                {
                    return menuChoice;
                }

                Console.BackgroundColor = ConsoleColor.Red;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("FEL! Ange ett nummer mellan 0 och 6.");
                Console.ResetColor();
                ContinueOnKeyPressed();

            } while (true);
        }
        // Hämtar recept från textfilen Recipes.txt
        private static IList<Recipe> LoadRecipes()
        {

            try
            {
                RecipeRepository loadrecipe = new RecipeRepository("Recipes.txt");
                IList<Recipe> load = loadrecipe.Load();
                Console.BackgroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine("Recept har laddats");
                Console.ResetColor();
                return load;

            }

            catch
            {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.WriteLine("Receptet kunde inte laddas in.");
                Console.ResetColor();
                ContinueOnKeyPressed();
                return null;
            }


        }

        private static IList<string> ReadDirections()
        {
            IList<string> direct = new List<string>();
            while (true)
            {
                Console.WriteLine("Ange instruktion: {0}", (direct.Count() + 1));
                string line = Console.ReadLine();


                if (line == "" || line == null)
                {
                    if (direct == null)
                    {
                        Console.WriteLine("Ange minst en instruktion.");
                    }

                    else
                    {
                        Console.WriteLine("Avbryta? Y/N");
                        if (Console.ReadLine() == "y")
                        {
                            break;
                        }
                    }
                }
                direct.Add(line);
            }
            return direct;
        }




        private static IList<Ingredient> ReadIngredients()
        {

            Ingredient ingStruct = new Ingredient { };
            IList<Ingredient> ingredients = new List<Ingredient>();
            while (true)
            {
                while (true)
                {
                    Console.Write("Ange namnet på ingrediens {0} : ", (ingredients.Count() + 1));
                    ingStruct.Name = Console.ReadLine();

                    if (ingStruct.Name == "" || ingStruct.Name == null)
                    {
                        Console.BackgroundColor = ConsoleColor.Red;
                        Console.WriteLine("Du har inte angett någon ingrediens. Vill du avbryta inmatningen? y/n");
                        Console.ResetColor();

                        if (Console.ReadLine() == "y")
                        {
                            if (ingredients.Count() == 0)
                            {
                                return null;
                            }
                            return ingredients;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
                Console.Write("Ange mängd: ");

                ingStruct.Amount = String.Format("{0}", Console.ReadLine());
                Console.Write("Ange mått: ");

                ingStruct.Measure = String.Format("{0}", Console.ReadLine());
                ingredients.Add(ingStruct);
            }
        }


        private static string ReadRecipeName()
        {
            Console.Write("Ange receptets namn: ");
            return Console.ReadLine();
        }

        // Sparar recept i en ny textfil. Finns det inga recept att spara presenteras felmedellande
        private static void SaveRecipes(IList<Recipe> recipes)
        {


            if (recipes.Count() != 0)
            {
                try
                {
                    Console.Clear();
                    RecipeRepository saver = new RecipeRepository("newrecipes.txt");
                    saver.Save(recipes);
                    Console.BackgroundColor = ConsoleColor.DarkGreen;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.WriteLine("===============================================");
                    Console.WriteLine("|            Recepten har sparats             |");
                    Console.WriteLine("===============================================");
                    Console.ResetColor();

                }
                catch
                {
                    Console.BackgroundColor = ConsoleColor.DarkRed;
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("=================================================");
                    Console.WriteLine("| Ett fel inträffade då recepten skulle sparas. |");
                    Console.WriteLine("=================================================");
                    Console.ResetColor();

                }
            }
            else
            {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("===============================================");
                Console.WriteLine("|      Det finns inga recept att spara        |");
                Console.WriteLine("===============================================");
                Console.ResetColor();

            }
        }


        private static Recipe CreateRecipe()
        {
            var name = ReadRecipeName();
            if (String.IsNullOrWhiteSpace(name))
            {
                return null;
            }

            var ingredients = ReadIngredients();
            if (!ingredients.Any())
            {
                return null;
            }

            var directions = ReadDirections();
            if (!directions.Any())
            {
                return null;
            }

            return new Recipe(name, ingredients, directions);
        }

        //Ta bort valfritt recept. FInns det inga recept att ta bort presenteras felmedellande
        static void DeleteRecipe(IList<Recipe> recipes)
        {
            Console.Clear();
            if (recipes.Count() != 0)
            {

                Console.BackgroundColor = ConsoleColor.DarkGreen;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("=======================================");
                Console.WriteLine("|      Välj recept att ta bort        |");
                Console.WriteLine("=======================================");
                Console.ResetColor();

                Recipe rec = GetRecipe(String.Format("Välj ett recept att ta bort: [1-{0}]| y/n", recipes.Count()), recipes);
                Console.WriteLine(String.Format("Vill du ta bort receptet {0} y/n?", rec.Name));
                if (Console.ReadLine() == "y")
                {
                    recipes.Remove(rec);
                    Console.BackgroundColor = ConsoleColor.DarkGreen;
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("=======================================");
                    Console.WriteLine("|     Receptet har tagits bort        |");
                    Console.WriteLine("=======================================");
                    Console.ResetColor();
                }

                else
                {
                    Console.BackgroundColor = ConsoleColor.DarkGreen;
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("============================================================");
                    Console.WriteLine("|           Du valde ej recept. Åter till huvudmeny         |");
                    Console.WriteLine("============================================================");
                    Console.ResetColor();
                    GetMenuChoice();
                }
            }
            else
            {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("=================================================");
                Console.WriteLine("= Det finns inga recept att ta bort             =");
                Console.WriteLine("=================================================");
                Console.ResetColor();

            }


        }
        // Hämtar valfritt recept
        static Recipe GetRecipe(string header, IList<Recipe> recipe)
        {
            do
            {
                Console.WriteLine("0. Avbryt\n");

                for (int i = 0; i < recipe.Count; i++)
                {
                    Console.Write("\n{0}. {1} ", i + 1, recipe[i].Name);
                }

                bool exit = false;
                int getMenuChoice;

                string choice = Console.ReadLine();

                while (!exit)
                {

                    if (String.IsNullOrWhiteSpace(choice) || choice == "0")
                    {
                        Console.BackgroundColor = ConsoleColor.Red;
                        Console.WriteLine("\nVill du verkligen avsluta? Tryck Esc. Annars tryck Enter!\n");
                        Console.ResetColor();
                        ConsoleKeyInfo keyPress = Console.ReadKey(true);
                        if (keyPress.Key == ConsoleKey.Escape)
                        {
                            exit = true;
                            return null;
                        }

                        else
                        {
                            break;
                        }
                    }

                    if (int.TryParse(choice, out getMenuChoice) && getMenuChoice > 0 && getMenuChoice <= recipe.Count())
                    {
                        return recipe[getMenuChoice - 1];
                    }

                    else if (int.TryParse(choice, out getMenuChoice) && getMenuChoice == 0)
                    {
                        return null;
                    }

                    else
                    {
                        Console.Clear();
                        Console.BackgroundColor = ConsoleColor.Red;
                        Console.WriteLine("\nFel ange ett tal mellan 0 och {0}\n\n", recipe.Count());
                        Console.ResetColor();
                        break;
                    }
                }
            } while (true);

        }




        //Visar antingen alla recept(6) eller ett enstaka(5). 
        static void ViewRecipe(IList<Recipe> recipes, bool viewAll = false)
        {
            RecipeView recView = new RecipeView();
            int count = 1;
            int choice;

            try
            {
                if (!viewAll)
                {
                    Console.Clear();
                    foreach (Recipe recipe in recipes)
                    {

                        Console.WriteLine("{0}: {1}", count, recipe.Name);
                        count++;
                    }
                    Console.Write("Välj recept: ");
                    choice = int.Parse(Console.ReadLine());
                    choice--;
                    Console.Clear();

                    recView.Render(recipes[choice]);
                }
                else
                {
                    Console.Clear();
                    recView.Render(recipes);
                }
            }
            catch
            {
                Console.WriteLine("Det gick inte att läsa in ett eller flera recept!");
            }

        }
    }
}

