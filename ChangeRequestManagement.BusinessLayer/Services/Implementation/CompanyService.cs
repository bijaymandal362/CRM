using ChangeRequestManagement.BusinessLayer.Services.Abstraction;
using ChangeRequestManagement.Entities;
using ChangeRequestManagement.Entities.Context;
using ChangeRequestManagment.Models.ReturnInfo;
using ChangeRequestManagment.Models.ViewModels.Company;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ChangeRequestManagement.BusinessLayer.Services.Implementation
{
    public class CompanyService : ICompanyService
    {
        private readonly CRMDbContext _context;
        private readonly IHttpContextAccessor _httpContext;
        private readonly ILogger<CompanyService> _logger;

        public CompanyService(CRMDbContext context,
            IHttpContextAccessor httpContext,
            ILogger<CompanyService> logger)
        {
            _context = context;
            _httpContext = httpContext;
            _logger = logger;
        }

        public async Task CreateCompanyAsync(CompanyViewModel model)
        {
            try
            {
                ClaimsPrincipal cp = _httpContext.HttpContext.User;
                var checkIfCompanyNameExists = await _context.Company.AnyAsync(c => c.CompanyName == model.CompanyName.ToUpper());
                if (!checkIfCompanyNameExists)
                {
                    await _context.AddAsync(new Company
                    {
                        CompanyName = model.CompanyName.Trim().ToUpper(),
                        InsertPersonId = Convert.ToInt32(cp.FindFirst("PersonId").Value),
                        InsertDate = model.InsertDate
                    });
                    _context.SaveChanges();
                }
                else
                {
                    throw new Exception($"Company name: {model.CompanyName} already exists in our database!");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while adding new company record for companyId: {model.CompanyId}. Exception message: {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<CompanyViewModel>> GetAllCompanyAsync()
        {
            try
            {
                var result = await (from c in _context.Company
                                    select new CompanyViewModel
                                    {
                                        CompanyId = c.CompanyId,
                                        CompanyName = c.CompanyName
                                    }).ToListAsync();
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while fetching all company records. Exception message: {ex.Message}");
                throw;
            }
        }

        public async Task<List<CompanyViewModel>> GetCompanyDropDownAsync()
        {
            try
            {
                var result = await (from c in _context.Company
                                    select new CompanyViewModel
                                    {
                                        CompanyId = c.CompanyId,
                                        CompanyName = c.CompanyName
                                    }).ToListAsync();

                result.Insert(0, new CompanyViewModel { CompanyId = 0, CompanyName = "--Select--" });
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while fetching the drop-down list elements for company. Exception message: {ex.Message}");
                throw;
            }
        }

        public async Task<CompanyViewModel> GetCompanyByIdAsync(int companyId)
        {
            try
            {
                var result = await (from c in _context.Company
                                    where c.CompanyId == companyId
                                    select new CompanyViewModel
                                    {
                                        CompanyId = c.CompanyId,
                                        CompanyName = c.CompanyName
                                    }).FirstOrDefaultAsync();
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while fetching the company record by companyId: {companyId}. Exception message: {ex.Message}");
                throw;
            }
        }

        public async Task<int> GetCompanyIdByClientEmailAsync(string email) 
        {
            try
            {
                var result = await (from c in _context.Company
                                    join cl in _context.Client on 
                                    c.CompanyId equals cl.CompanyId
                                    join p in _context.Person on 
                                    cl.PersonId equals p.PersonId
                                    where p.EmailAddress == email
                                    select new CompanyViewModel
                                    {
                                        CompanyId = c.CompanyId,
                                        CompanyName = c.CompanyName
                                    }).FirstOrDefaultAsync();
                return result.CompanyId;
            }
            catch (Exception ex)
{
                _logger.LogError($"Error while fetching the company record by EmailAddress: {email}. Exception message: {ex.Message}");
                throw;
            }
        }

        public async Task UpdateCompanyAsync(CompanyViewModel model)
        {
            try
            {
                ClaimsPrincipal cp = _httpContext.HttpContext.User;
                var company = await _context.Company.FirstOrDefaultAsync(x => x.CompanyId == model.CompanyId);
                _context.Entry(company).State = EntityState.Detached;

                if (model.CompanyId == company.CompanyId)
                {
                    _context.Company.Update(new Company
                    {
                        CompanyId = model.CompanyId,
                        CompanyName = model.CompanyName.Trim().ToUpper(),
                        InsertPersonId = company.InsertPersonId,
                        InsertDate = company.InsertDate,
                        UpdatePersonId = Convert.ToInt32(cp.FindFirst("PersonId").Value),
                        UpdateDate = model.UpdateDate
                    });
                    await _context.SaveChangesAsync();
                }
                else
                {
                    throw new Exception($"Record for companyId: {model.CompanyId} doesn't exists in our database!");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while updating the company record for companyId: {model.CompanyId}. Exception message: {ex.Message}");
                throw;
            }
        }

        public async Task<InfoHandlerViewModel> DeleteCompanyAsync(int companyId)
        {
            try
            {
                _context.Company.Remove(await _context.Company.FindAsync(companyId));
                _context.SaveChanges();
                return new InfoHandlerViewModel
                {
                    IsSuccess = true,
                    Message = $"The record for companyId: {companyId} is successfully deleted!"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while deleting the record for companyId: {companyId}. Exception Message: {ex.Message}");
                return new InfoHandlerViewModel
                {
                    IsSuccess = false,
                    Message = $"Error while deleting companyId: {companyId}"
                };
            }
        }

        public async Task<int> CountTotalCompaniesAsync() 
        {
            return await _context.Company.CountAsync();
        }
    }
}