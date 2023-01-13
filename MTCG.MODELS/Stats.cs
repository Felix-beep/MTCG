using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.MODELS
{
    public class Stats
    {
        public string Username { get; }
        public int Elo { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }

        public Stats(string Username, int Elo, int Wins, int Losses) {
            this.Username = Username;
            this.Elo = Elo;
            this.Wins = Wins;
            this.Losses = Losses;
        }

        public void AddLoss()
        {
            Losses++;
        }

        public void AddWin()
        {
            Wins++;
        }

        public void AddElo(int Elo)
        {
            this.Elo += Elo;
            if (Elo < 0)
            {
                Elo = 0;
            } else if ( Elo > 1000)
            {
                Elo = 1000;
            }
        }
    }
}
