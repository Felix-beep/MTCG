namespace MTCG.MODELS
{
    public class User
    {
        public string Name { get; }
        public string Password { get; }
        public string Bio { get; }
        public string Image { get; }
        public int Gold { get; set; }

        public User(string name, string password, string bio, string image, int gold)
        {
            Name = name;
            Password = password;
            Bio = bio;
            Image = image;
            Gold = gold;
        }
    }
}
