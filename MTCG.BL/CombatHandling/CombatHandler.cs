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
            Console.WriteLine("\n--------------------------------------------------");
            Console.WriteLine($"{User1.Name} vs {User2.Name}");
            Console.WriteLine($"\nDeck {User1.Name}:");
            foreach(CardInstance card in Deck1.DeckList)
            {
                Console.WriteLine(card.CardName + ": " + card.BaseCard.Type + " - " + card.BaseCard.Element + " - " + card.BaseCard.Faction + " - " + card.BaseCard.Power + " - " + card.Rating);
            }
            Console.WriteLine($"\nDeck {User2.Name}:");
            foreach (CardInstance card in Deck2.DeckList)
            {
                Console.WriteLine(card.CardName + ": " + card.BaseCard.Type + " - " + card.BaseCard.Element + " - " + card.BaseCard.Faction + " - " + card.BaseCard.Power + " - " + card.Rating);
            }
            Console.WriteLine("\n--------------------------------------------------");
            Console.WriteLine("Combat Starts!");
            while (Deck1.DeckList.Count > 0 && Deck2.DeckList.Count > 0 && Counter > 0)
            {
                Console.WriteLine("\n--------------------------------------------------");
                Console.WriteLine($"Round {101-Counter}");
                Counter--;
                CardInstance Card1 = Deck1.PopRandomCard();
                CardInstance Card2 = Deck2.PopRandomCard();
                if (Card1 == null || Card2 == null) return 2;
                Console.WriteLine($"{User1.Name}'s {Card1.BaseCard.Name} vs {User2.Name}'s {Card2.BaseCard.Name}");
                switch (FightCalculations.Compare(Card1, Card2))
                {
                    case 1:
                        Console.WriteLine($"{User1.Name}'s {Card1.BaseCard.Name} Won!");
                        Deck1.AddCard(Card1);
                        Deck1.AddCard(Card2); 
                        foreach(CardInstance Card in CardsSaved){
                            Deck1.AddCard(Card);
                        }
                        CardsSaved.Clear();
                        break;
                    case 2:
                        Console.WriteLine($"{User2.Name}'s {Card2.BaseCard.Name} Won!");
                        Deck2.AddCard(Card1);
                        Deck2.AddCard(Card2);
                        foreach (CardInstance Card in CardsSaved){
                            Deck2.AddCard(Card);
                        }
                        CardsSaved.Clear(); 
                        break;
                    case 0:
                        Console.WriteLine($"Its a Draw! Both cards will be saved, and the next winner will get both.");
                        CardsSaved.Add(Card1);
                        CardsSaved.Add(Card2); break;
                    default:  break;
                }
                Console.WriteLine($"{User2.Name}'s Deck: {Deck1.DeckList.Count} Saved: {CardsSaved.Count} {User2.Name}'s Deck: {Deck2.DeckList.Count}");
            }
            Console.WriteLine("\n--------------------------------------------------\n");
            if (Deck2.DeckList.Count == 0)
            {
                Console.WriteLine("Player 1 has won\n");
                Console.WriteLine("--------------------------------------------------\n");
                return 1;
            }
            if(Deck1.DeckList.Count == 0)
            {
                Console.WriteLine("Player 2 has won\n");
                Console.WriteLine("--------------------------------------------------\n");
                return 2;
            }
            if(Counter == 0)
            {
                Console.WriteLine("The Game has been decided a draw\n");
                Console.WriteLine("--------------------------------------------------\n");
                return 0;
            }
            return 3;
        }
    }
}