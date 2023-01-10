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
        public string Owner { get; }
        public string PackID { get; }
        public List<CardTemplate> Templates { get; } 

        public Pack(string Username, string PackId)
        {
            Templates= new List<CardTemplate>();
            Owner = Username;
        }

        public void AddCard(CardTemplate template)
        {
            Templates.Add(template);
        }
    }
}
