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

        public User(int id, string name)
        {
            Id = id;
            Name = name;
            CardList = new Stack();
        }

        public void AddCard(CardTemplate CardToAdd)
        {
            CardList.AddCard(new CardInstance(CardToAdd));
        }
    }
}
