namespace Redde.Application.DTOs.User;

public class UserResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Role { get; set; } = null!;
    public string? Company { get; set; }
}
