namespace PadelApp.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Alias { get; set; }
        public string Number { get; set; }
    }
}
