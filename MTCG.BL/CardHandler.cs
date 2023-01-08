using MTCG.MODELS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.BL
{
    public static class CardHandler
    {
        public static CardInstance CreateCard(string Name)
        {
            CardTemplate cardtemplate = null;

            // get cardtemplate from Database with Name

            CardInstance cardinstace = new(cardtemplate);

            return cardinstace;
        }

        public static CardInstance GetCardInstance(string id)
        {
            CardInstance cardinstace = null;

            // get cardinstace from Database with Name

            return cardinstace;
        }

        public static CardTemplate GetCardTemplate(string name)
        {
            CardTemplate cardtemplate = null;

            // get cardtemplate from Database with Name

            return cardtemplate;
        }
    }
}
