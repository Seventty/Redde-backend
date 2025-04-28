namespace Redde.Domain.Entities
{
    public class Company
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public int OwnerId { get; set; }

        public User Owner { get; set; } = null!;

    }
}
