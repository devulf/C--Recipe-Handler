using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Receptsamling
{

    struct Ingredient
    {
        public string Amount
        {
            get;

            set;

        }

        public string Measure
        {
            get;

            set;

        }

        public string Name
        {
            get;

            set;

        }

        //Visar mängd, mått och receptets namn i rätt format
        public override string ToString()
        {
            string viewFormat;
            viewFormat = string.Format("Mängd: {0}, Mått: {1}, Namn: {2}", Amount, Measure, Name);

            return viewFormat;

        }
    }

}