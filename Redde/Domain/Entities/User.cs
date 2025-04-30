namespace Redde.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? Password { get; set; }
        public int RoleId { get; set; }
        public int? CompanyId { get; set; }
        public Role Role { get; set; } = null!;
        public Company? Company { get; set; }
        public string? RefreshToken { get; set; }
        public string? Provider { get; set; }
        public string? ProviderId { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
    }
}
