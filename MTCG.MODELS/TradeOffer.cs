namespace MTCG.MODELS
{
    public class TradeOffer
    {
        public string Username { get; }
        public string CardId { get; }
        public string TradeId  { get; }
        public int Rating { get; }

        public TradeOffer(string Username, string CardId, string TradeId, int Rating)
        {
            this.Username = Username;
            this.CardId = CardId;
            this.TradeId = TradeId;
            this.Rating = Rating;
        }
    }
}
