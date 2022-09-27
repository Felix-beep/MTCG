using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.BL
{
    internal class Stack
    {
        private List<CardInstance> ListOfCards = new List<CardInstance>();

        public Stack()
        {

        }

        public void AddCard(CardInstance CardToBeAdded)
        {
            ListOfCards.Add(CardToBeAdded);
        }

        public void PrintCards()
        {
            foreach (CardInstance card in ListOfCards)
            {
                card.printCard();
            }
        }
    }
}
