namespace schoolHub.Models
{
    public class User
    {
        public int id { get; set; }
        public string login { get; set; }
        public string hashed_password { get; set; }
        public string name { get; set; } = string.Empty;

        public int age { get; set; }
    }
}
