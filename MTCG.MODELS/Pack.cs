using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.MODELS
{
    public class Pack : CardList
    {
        public int Packsize { get; }

        public Pack(int packsize)
        {
            Packsize = packsize;
        }

        public void fillRandomly(List<CardTemplate> listOfAllCards)
        {
            Random r = new Random();
            if (listOfAllCards.Count == 0) return;
            for (int i = Size; i < Packsize; i++)
            {
                int index = 0;
                if (listOfAllCards.Count > 1) { index = r.Next(0, listOfAllCards.Count - 1); }
                AddCard(new CardInstance(listOfAllCards[index]));
            }
            return;
        }

        public void AddCard(CardTemplate cardToAdd)
        {
            if (Size >= Packsize) return;
            AddCard(new CardInstance(cardToAdd));
            return;
        }

        public override void AddCard(CardInstance cardToAdd)
        {
            if (Size >= Packsize) return;
            AddCard(cardToAdd);
            return;
        }
    }
}
