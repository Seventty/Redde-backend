using Microsoft.EntityFrameworkCore;
using Redde.Application.DTOs.User;
using Redde.Application.Interfaces;
using Redde.Domain.Entities;

namespace Redde.Application.Services;

public class UserService(IUnitOfWork unitOfWork) : IUserService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<IEnumerable<UserResponse>> GetAllUsersAsync()
    {
        var users = await _unitOfWork.Users.GetAllAsync(u => u.Role, u => u.Company);

        return users.Select(u => new UserResponse
        {
            Id = u.Id,
            Name = u.Name,
            Email = u.Email,
            Role = u.Role.Name,
            Company = u.Company?.Name
        });
    }
}
