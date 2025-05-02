using Redde.Application.DTOs.User;

namespace Redde.Application.Interfaces;

public interface IUserService
{
    Task<IEnumerable<UserResponse>> GetAllUsersAsync();
}
