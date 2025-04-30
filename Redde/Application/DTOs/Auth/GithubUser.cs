namespace Redde.Application.DTOs.Auth
{
    public class GithubUser
    {
        public int Id { get; set; }
        public string Login { get; set; } = null!;
        public string? Email { get; set; }
        public string? Name { get; set; }
    }
}
