using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.MODELS
{
    public class Pack
    {
        public string PackID { get; }
        public List<CardTemplate> Templates { get; } 

        public Pack(string PackId)
        {
            Templates= new List<CardTemplate>();
        }

        public void AddCard(CardTemplate template)
        {
            Templates.Add(template);
        }
    }
}
