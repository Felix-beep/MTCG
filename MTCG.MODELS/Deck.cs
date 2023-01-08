using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.MODELS
{
    public class Deck : CardList
    {
        public Deck(Deck toCopy)
        {
            mylist = toCopy.mylist;
        }

        public Deck()
        {

        }
    }
}
