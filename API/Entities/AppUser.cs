namespace API.Entities
{
    public class AppUser
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public byte[] PassswordHash { get; set; }
        public byte[] PassswordSalt { get; set; }
    }
}