using MTCG.BL;

Console.WriteLine("Hello, World!");

CardTemplate myCard1 = new CardTemplate("Goblin", 20, Elements.Normal, Types.Monster, Factions.Goblin);
CardTemplate myCard2 = new CardTemplate("Elf", 20, Elements.Normal, Types.Monster, Factions.Elf);

User myUser = new User(12, "Felix");
myUser.AddCard(myCard2);
myUser.AddCard(myCard1);
myUser.AddCard(myCard2);
myUser.AddCard(myCard2);
myUser.CardList.PrintCards();
