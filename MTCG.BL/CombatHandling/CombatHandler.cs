using System.Collections.Specialized;
using System.Diagnostics.Metrics;
using System.Reflection;
using System.Reflection.Metadata;
using MTCG.MODELS;

namespace MTCG.BL
{
    public class CombatHandler
    {
        private Deck Deck1 { get; set; }

        private User User1 { get; set; }
        private Deck Deck2 { get; set; }

        private User User2 { get; set; }

        private List<CardInstance> CardsSaved = new List<CardInstance>();

        public CombatHandler(QueueEntry QueueSpot)
        {
            Deck1 = QueueSpot.Deck1;
            Deck2 = QueueSpot.Deck2;
            User1 = QueueSpot.User1;
            User2 = QueueSpot.User2;
        }

        public int StartCombat() // 1 = player1 won, 0 = draw, 2 = player2 won, 2 = error
        {
            if (Deck1.DeckList.Count != 4 || Deck2.DeckList.Count != 4) return 2;
            int Counter = 100;

            while(Deck1.DeckList.Count > 0 && Deck2.DeckList.Count > 0 && Counter > 0)
            {
                Counter--;
                CardInstance Card1 = Deck1.PopRandomCard();
                CardInstance Card2 = Deck2.PopRandomCard();
                if (Card1 == null || Card2 == null) return 2; 
                switch(FightCalculations.Compare(Card1, Card2))
                {
                    case -1:
                        Deck1.AddCard(Card1);
                        Deck1.AddCard(Card2); 
                        foreach(CardInstance Card in CardsSaved){
                            Deck1.AddCard(Card);
                        }
                        CardsSaved.Clear();
                        break;
                    case 1:
                        Deck2.AddCard(Card1);
                        Deck2.AddCard(Card2);
                        foreach (CardInstance Card in CardsSaved){
                            Deck2.AddCard(Card);
                        }
                        CardsSaved.Clear(); 
                        break;
                    case 0:
                        CardsSaved.Add(Card1);
                        CardsSaved.Add(Card2); break;
                    default:  break;
                }
                Console.WriteLine($"PlayerA Deck: {Deck1.DeckList.Count} Stack: {CardsSaved.Count} PlayerB Deck: {Deck2.DeckList.Count}");
            }
            if(Deck1.DeckList.Count == 0)
            {
                Console.WriteLine("Player 1 has won");
                return 1;
            }
            if(Deck2.DeckList.Count == 0)
            {
                Console.WriteLine("Player 2 has won");
                return 2;
            }
            if(Counter == 0)
            {
                Console.WriteLine("The Game has been decided a draw");
                return 0;
            }
            return 2;
        }
    }
}