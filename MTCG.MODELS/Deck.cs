using MTCG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.MODELS
{
    public class Deck
    {
        private CardInstance[] mydeck = new CardInstance[4];
        public CardInstance[] MyDeck {
            get { return mydeck; }
        }

        private Deck()
        {
        }

        void CreateTopDeck(Stack myStack)
        {
            foreach( CardInstance card in myStack.ListOfCards)
            {

            }
        }
    }
}
