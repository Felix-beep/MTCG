
using MTCG.Models;

Console.WriteLine("Hello, World!");

CardTemplate myCard1 = new CardTemplate("Goblin", 20, Elements.Normal, Types.Monster, Factions.Goblin);
CardTemplate myCard2 = new CardTemplate("Elf", 20, Elements.Normal, Types.Monster, Factions.Elf);

User firstUser = new User(1, "Rupert");
firstUser.AddCard(myCard1);
firstUser.AddCard(myCard1);
firstUser.AddCard(myCard1);
firstUser.AddCard(myCard1);

User secondUser = new User(2, "Stefan");
secondUser.AddCard(myCard2);
secondUser.AddCard(myCard2);
secondUser.AddCard(myCard2);
secondUser.AddCard(myCard2);

firstUser.CardList.PrintCards();
