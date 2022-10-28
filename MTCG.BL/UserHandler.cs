using MTCG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.BL
{
    internal static class UserHandler
    {
        /*public static User createNewUser()
        {
            //return new User();
        }*/

        public static bool moveElo(User winner, User loser, int Elo)
        {
            if(loser.Elo < Elo)
            {
                Elo = loser.Elo;
            }
            if (winner.Elo + Elo > 1000)
            {
                Elo = 1000 - winner.Elo;
            }
            bool noerror = winner.AddElo(Elo) && loser.RemoveElo(Elo);
            return noerror;
        }
    }
}
