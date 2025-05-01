using Redde.Application.DTOs.Company;

namespace Redde.Application.Interfaces;

public interface ICompanyService
{
    Task CreateAsync(int userId, CompanyRequest request);
    Task<IEnumerable<CompanyResponse>> GetAllAsync(int userId, string role);
    Task<CompanyResponse> GetByIdAsync(int id, int userId, string role);
    Task UpdateAsync(int id, int userId, CompanyRequest request);
    Task DeleteAsync(int id, int userId);
}
