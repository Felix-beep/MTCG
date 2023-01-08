using MTCG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.MODELS
{
    public class CardList : IPrintable
    {
        protected List<CardInstance> mylist = new List<CardInstance>();
        public List<CardInstance> MyList
        {
            get { return mylist; }
        }

        public int Size
        {
            get { return mylist.Count; }
        }

        public CardList()
        {

        }

        public CardInstance GetRandomCard()
        {
            if (Size == 0) return null;

            int index = 0;
            if (Size != 1)
            {
                Random r = new Random();
                index = r.Next(0, Size);
            }
            return mylist[index];
        }
        public CardInstance PopRandomCard()
        {
            if (Size == 0) return null;

            int index = 0;
            if (Size != 1)
            {
                Random r = new Random();
                index = r.Next(0, Size);
            }
            CardInstance card = mylist[index];
            mylist.RemoveAt(index);
            return card;
        }

        public virtual void AddCard(CardInstance Card)
        {
            mylist.Add(Card);
        }

        public virtual bool RemoveCard(CardInstance Card)
        {
            foreach (CardInstance card in mylist)
            {
                if (card.ID == Card.ID)
                {
                    MyList.Remove(card);
                    return true;
                }
            }
            return false;
        }

        public override void Print()
        {
            foreach (CardInstance card in MyList)
            {
                card.Print();
            }
        }
    }
}
