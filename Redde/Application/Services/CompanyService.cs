using Redde.Application.DTOs.Company;
using Redde.Application.Interfaces;
using Redde.Domain.Entities;

namespace Redde.Application.Services;

public class CompanyService(IUnitOfWork unitOfWork) : ICompanyService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task CreateAsync(int userId, CompanyRequest request)
    {
        var company = new Company
        {
            RNC = request.RNC,
            Name = request.Name,
            CommercialName = request.CommercialName,
            OwnerId = userId,
            Category = request.Category,
            PaymentScheme = request.PaymentScheme,
            State = request.State,
            EconomicActivity = request.EconomicActivity,
            GovernmentBranch = request.GovernmentBranch
        };

        await _unitOfWork.Companies.AddAsync(company);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<IEnumerable<CompanyResponse>> GetAllAsync(int userId, string role)
    {
        IEnumerable<Company> companies;

        if (role == "Admin")
        {
            companies = await _unitOfWork.Companies.GetAllAsync();
        }
        else
        {
            companies = await _unitOfWork.Companies.FindAllAsync(c => c.OwnerId == userId);
        }

        return companies.Select(c => new CompanyResponse
        {
            Id = c.Id,
            RNC = c.RNC,
            Name = c.Name,
            CommercialName = c.CommercialName,
            Category = c.Category,
            PaymentScheme = c.PaymentScheme,
            State = c.State,
            EconomicActivity = c.EconomicActivity,
            GovernmentBranch = c.GovernmentBranch
        });
    }

    public async Task<CompanyResponse> GetByIdAsync(int id, int userId, string role)
    {
        var company = await _unitOfWork.Companies.FindAsync(c => c.Id == id);

        if (company == null || (role != "Admin" && company.OwnerId != userId))
        {
            throw new UnauthorizedAccessException("No tienes acceso a esta empresa.");
        }

        return new CompanyResponse
        {
            Id = company.Id,
            RNC = company.RNC,
            Name = company.Name,
            CommercialName = company.CommercialName,
            Category = company.Category,
            PaymentScheme = company.PaymentScheme,
            State = company.State,
            EconomicActivity = company.EconomicActivity,
            GovernmentBranch = company.GovernmentBranch
        };
    }

    public async Task UpdateAsync(int id, int userId, CompanyRequest request)
    {
        var company = await _unitOfWork.Companies.FindAsync(c => c.Id == id);

        if (company == null || company.OwnerId != userId)
        {
            throw new UnauthorizedAccessException("No puedes editar esta empresa.");
        }

        company.RNC = request.RNC;
        company.Name = request.Name;
        company.CommercialName = request.CommercialName;
        company.Category = request.Category;
        company.PaymentScheme = request.PaymentScheme;
        company.State = request.State;
        company.EconomicActivity = request.EconomicActivity;
        company.GovernmentBranch = request.GovernmentBranch;

        _unitOfWork.Companies.Update(company);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id, int userId)
    {
        var company = await _unitOfWork.Companies.FindAsync(c => c.Id == id);

        if (company == null || company.OwnerId != userId)
        {
            throw new UnauthorizedAccessException("No puedes eliminar esta empresa.");
        }

        await _unitOfWork.Companies.DeleteAsync(company.Id);
        await _unitOfWork.SaveChangesAsync();
    }
}
