using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using MTCG.MODELS;

namespace MTCG.DatabaseAccess
{
    public static class QueueAccess
    {
        public static List<QueueEntry> Queue = new();
        
        public static QueueEntry EnterQueue(User UserToEnter, Deck DeckToEnter)
        {
            lock(Queue)
            {
                QueueEntry openQueue = null;
                try
                {
                    openQueue = Queue.First(x => x.Open == true);
                }   catch(InvalidOperationException ex)
                {
                    Console.WriteLine("- No open Queue found");
                }

                if(openQueue == null)
                {
                    QueueEntry newEntry = new QueueEntry(UserToEnter, DeckToEnter);
                    Queue.Add(newEntry);
                    return newEntry;
                }
                else 
                {
                    openQueue.Join(UserToEnter, DeckToEnter);
                }
                return openQueue;
            }
        }
    }
}
