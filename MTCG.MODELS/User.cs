using MTCG.MODELS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Models
{
    public class User
    {
        public int Id { get; }
        public string Name { get; }
        public Stack CardList { get; }
        public Deck DeckList { get; }

        public int elo;
        public int Elo { get { return elo; } }
        public int Gold { get; set; }

        private List<Pack> CardPacks;

        public User(int id, string name)
        {
            CardPacks = new List<Pack>();
            elo = 1000;
            Id = id;
            Name = name;
            CardList = new Stack();
            DeckList = new Deck();
        }

        public void AddPack(Pack PackToAdd)
        {
            CardPacks.Add(PackToAdd);
        }

        public void AddCard(CardTemplate CardToAdd)
        {
            CardList.AddCard(new CardInstance(CardToAdd));
        }

        public void AddCard(CardInstance CardToAdd)
        {
            CardList.AddCard(CardToAdd);
        }

        public void AddCardToDeck(CardInstance CardToAdd)
        {
            DeckList.AddCard(CardToAdd);
        }

        public bool RemoveCard(CardInstance CardToRemove)
        {
            return CardList.RemoveCard(CardToRemove);
        }

        public bool RemoveElo(int elo)
        {
            if (this.elo - elo < 0) { return false; }
            else this.elo -= elo;
            return true;
        }
        public bool AddElo(int elo)
        {
            if (this.elo + elo > 1000) { return false; }
            else this.elo += elo;
            return true;
        }
    }
}
