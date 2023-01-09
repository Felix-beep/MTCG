using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.MODELS
{
    public class Stack
    {
        public List<CardInstance> CardList { get; }
        public string Username { get; }

        public Stack(string Username)
        {
            CardList = new List<CardInstance>();
            this.Username = Username;
        }
    }
}
