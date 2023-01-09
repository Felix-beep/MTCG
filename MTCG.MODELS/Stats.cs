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
        public int Elo { get; }
        public int Wins { get; }
        public int Losses { get; }

        public Stats(string Username, int Elo, int Wins, int Losses) {
            this.Username = Username;
            this.Elo = Elo;
            this.Wins = Wins;
            this.Losses = Losses;
        }
    }
}
