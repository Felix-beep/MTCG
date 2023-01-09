using MTCG.MODELS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Models
{
    public class User
    {
        public string Name { get; }
        public string Password { get; }
        public string Bio { get; }
        public string Picture { get;  }
        public int Gold { get; set; }

        public User(string name, string password, string bio, string picture, int gold)
        {
            Name = name;
            Password = password;
            Bio = bio;
            Picture = picture;
            Gold = gold;
        }
    }
}
