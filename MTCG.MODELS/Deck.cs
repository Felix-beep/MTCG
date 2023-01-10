using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.MODELS
{
    public class Deck
    {
        public string Username { get; }

        public List<CardInstance> DeckList { get; }
        public Deck(Deck toCopy)
        {
            DeckList = toCopy.DeckList;
            Username = toCopy.Username;
        }

        public Deck(string Username)
        {
            DeckList = new();
            this.Username = Username;
        }

        public CardInstance GetRandomCard()
        {
            if (DeckList.Count == 0) return null;

            int index = 0;
            if (DeckList.Count != 1)
            {
                Random r = new Random();
                index = r.Next(0, DeckList.Count);
            }
            return DeckList[index];
        }
        public CardInstance PopRandomCard()
        {
            if (DeckList.Count == 0) return null;

            int index = 0;
            if (DeckList.Count != 1)
            {
                Random r = new Random();
                index = r.Next(0, DeckList.Count);
            }
            CardInstance card = DeckList[index];
            DeckList.RemoveAt(index);
            return card;
        }

        public virtual void AddCard(CardInstance Card)
        {
            DeckList.Add(Card);
        }

        public virtual bool RemoveCard(CardInstance Card)
        {
            foreach (CardInstance card in DeckList)
            {
                if (card.ID == Card.ID)
                {
                    DeckList.Remove(card);
                    return true;
                }
            }
            return false;
        }
    }
}
