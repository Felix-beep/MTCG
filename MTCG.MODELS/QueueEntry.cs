using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace MTCG.MODELS
{
    public class QueueEntry
    {
        public User User1 { get; set; }
        public User User2 { get; set; }

        public Deck Deck1 { get; set; }
        public Deck Deck2 { get; set; }

        public bool Open;
        public bool Finished { get; set; }
        public int Winner { get; set; }

        public Mutex MutexFinished { get; set; }

        public QueueEntry(User user1, Deck deck1)
        {
            User1 = user1;
            Deck1 = deck1;
            Open = true;
            Finished = false;
            MutexFinished.WaitOne();
        }

        public void Join(User user2, Deck deck2)
        {
            User2 = user2; 
            Deck2 = deck2;
            Open = false;
        }

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
