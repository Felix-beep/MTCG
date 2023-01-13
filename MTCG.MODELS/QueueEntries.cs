using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.MODELS
{
    public class QueueEntries
    {
        public User User1 { get; set; }
        public User User2 { get; set; }

        public Deck Deck1 { get; set; }
        public Deck Deck2 { get; set; }

        public bool Finished { get; set; }
        public int Winner { get; set; }

        public User GetWinner()
        {
            switch (Winner)
            {
                case -1: return User1; 
                case 1 : return User2;
                default: return null;
            }
        }
    }
}
