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
            CategoryId = request.CategoryId,
            PaymentSchemeId = request.PaymentSchemeId,
            StateId = request.StateId,
            EconomicActivityId = request.EconomicActivityId,
            GovernmentBranchId = request.GovernmentBranchId
        };

        await _unitOfWork.Companies.AddAsync(company);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<IEnumerable<CompanyResponse>> GetAllAsync(int userId, string role)
    {
        IEnumerable<Company> companies;

        if (role == "Admin")
        {
            companies = await _unitOfWork.Companies.GetAllAsync(
                c => c.Category, c => c.PaymentScheme, c => c.State,
                c => c.EconomicActivity, c => c.GovernmentBranch);
        }
        else
        {
            companies = await _unitOfWork.Companies.FindAllAsync(
                c => c.OwnerId == userId,
                c => c.Category, c => c.PaymentScheme, c => c.State,
                c => c.EconomicActivity, c => c.GovernmentBranch);
        }

        return companies.Select(c => new CompanyResponse
        {
            Id = c.Id,
            RNC = c.RNC,
            Name = c.Name,
            CommercialName = c.CommercialName,
            Category = c.Category.Name,
            PaymentScheme = c.PaymentScheme.Name,
            State = c.State.Name,
            EconomicActivity = c.EconomicActivity.Name,
            GovernmentBranch = c.GovernmentBranch.Name
        });
    }

    public async Task<CompanyResponse> GetByIdAsync(int id, int userId, string role)
    {
        var company = await _unitOfWork.Companies.FindAsync(
            c => c.Id == id,
            c => c.Category, c => c.PaymentScheme, c => c.State,
            c => c.EconomicActivity, c => c.GovernmentBranch);

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
            Category = company.Category.Name,
            PaymentScheme = company.PaymentScheme.Name,
            State = company.State.Name,
            EconomicActivity = company.EconomicActivity.Name,
            GovernmentBranch = company.GovernmentBranch.Name
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
        company.CategoryId = request.CategoryId;
        company.PaymentSchemeId = request.PaymentSchemeId;
        company.StateId = request.StateId;
        company.EconomicActivityId = request.EconomicActivityId;
        company.GovernmentBranchId = request.GovernmentBranchId;

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
