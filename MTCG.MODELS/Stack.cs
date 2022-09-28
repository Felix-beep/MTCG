using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Models
{
    public class Stack
    {
        private List<CardInstance> listOfCards = new List<CardInstance>();
        public List<CardInstance> ListOfCards {
            get { return listOfCards; }
        }

        public Stack()
        {
        }

        public void AddCard(CardInstance CardToBeAdded)
        {
            listOfCards.Add(CardToBeAdded);
        }

        public void PrintCards()
        {
            foreach (CardInstance card in listOfCards)
            {
                card.printCard();
            }
        }
    }
}
