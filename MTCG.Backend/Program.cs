// See https://aka.ms/new-console-template for more information

using MTCG.Backend;
using MTCG.MODELS;
using System.Diagnostics.Metrics;

CardBase.LoadCards();

int Counter = 1;

foreach (CardTemplate card in CardBase.getRandomCard().Take(10))
{
    Console.WriteLine(Counter);
    Counter++;
    card.Print();
}

Counter = 1;

foreach (CardTemplate card in CardBase.getRandomCard().Take(10))
{
    Console.WriteLine(Counter);
    Counter++;
    card.Print();
}
