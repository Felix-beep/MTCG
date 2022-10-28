using MTCG.Backend;
using MTCG.MODELS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.BL
{
    public static class PackHandler
    {
        public static Pack GetRandomPack()
        {
            if (CardBase.cardsLoaded) { 
                Pack newPack = new Pack(5);
                newPack.fillRandomly(CardBase.DicToList());
                return newPack;
            }
            return null;
        }

        public static Pack GetRandomPack(int size)
        {
            if (CardBase.cardsLoaded)
            {
                Pack newPack = new Pack(size);
                newPack.fillRandomly(CardBase.DicToList());
                return newPack;
            }
            return null;
        }

        public static Pack GetEmptyPack(int size)
        {
            Pack newPack = new Pack(size);
            return newPack;
        }
    }
}
