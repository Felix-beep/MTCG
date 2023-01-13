namespace MTCG.MODELS
{
    public class TradeOffer
    {
        public string Username { get; }
        public string CardId { get; }
        public string CardName { get; }
        public string TradeId  { get; }
        public int Rating { get; }

        public TradeOffer(string Username, string CardId, string CardName, string TradeId, int Rating)
        {
            this.Username = Username;
            this.CardId = CardId;
            this.CardName = CardName;
            this.TradeId = TradeId;
            this.Rating = Rating;
        }
    }

    public class user { }

}
