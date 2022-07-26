
using ChangeRequestManagement.BusinessLayer.Services.Abstraction;
using ChangeRequestManagement.Entities.Context;
using ChangeRequestManagment.Models.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static ChangeRequestManagment.Models.Enums.AuthenticationValidationEnum;

namespace ChangeRequestManagement.BusinessLayer.Services.Implementation
{
    public class AccountService : IAccountService
    {
        private readonly CRMDbContext _context;
        private readonly ILogger<AccountService> _logger;

        public AccountService(CRMDbContext context, ILogger<AccountService> logger) 
        {
            _context = context;
            _logger = logger;
        }

        public async Task<AuthenticationInfoViewModel> LoginPersonAsync(string email, string password) 
        {
            var personInfo = await _context.Person
                .Include(x=> x.Role)
                .Where(x => x.EmailAddress == email)
                .FirstOrDefaultAsync();
            if (personInfo != null)
            {
                var checkIfPasswordMatches = BCrypt.Net.BCrypt.Verify(password, personInfo.Password);
                if (checkIfPasswordMatches)
                {
                    return new AuthenticationInfoViewModel
                    {
                        PersonId = personInfo.PersonId,
                        FirstName = personInfo.FirstName,
                        LastName = personInfo.LastName,
                        EmailAddress = personInfo.EmailAddress,
                        IsSuccess = true,
                        ErrorType = Error.NoAnyError,
                        Role = personInfo.Role.RoleName
                    };
                }
                else 
                {
                    return new AuthenticationInfoViewModel 
                    {
                        IsSuccess = false,
                        ErrorType = Error.PasswordIsIncorrectOrNoMatch
                    };
                }
            }
            else 
            {
                return new AuthenticationInfoViewModel
                {
                    IsSuccess = false,
                    ErrorType = Error.EmailAddressNotFound
                };
            }
        }

        public async Task<PersonInfoViewModel> FindPersonByIdAsync(int id) 
        {
            var info = await (from r in _context.Role
                              join p in _context.Person on r.RoleId equals p.RoleId
                              where p.PersonId == id
                              select new { p.PersonId, p.FirstName, p.LastName, p.EmailAddress, r.RoleName})
                              .FirstAsync();
            if (info != null) 
            {
                return new PersonInfoViewModel
                {
                    Id = info.PersonId,
                    FirstName = info.FirstName,
                    LastName = info.LastName,
                    EmailAddress = info.EmailAddress,
                    RoleName = info.RoleName,
                    IsSuccess = true
                };
            }
            else 
            {
                return new PersonInfoViewModel
                {
                    IsSuccess = false
                };
            }
        }

        public async Task<PersonInfoViewModel> FindPersonByEmailAsync(string emailAddress) 
        {
            var info = await (from r in _context.Role
                              join p in _context.Person on r.RoleId equals p.RoleId
                              where p.EmailAddress == emailAddress
                              select new { p.PersonId, p.FirstName, p.LastName, p.EmailAddress, r.RoleName })
                              .FirstAsync();
            if (info != null)
            {
                return new PersonInfoViewModel
                {
                    Id = info.PersonId,
                    FirstName = info.FirstName,
                    LastName = info.LastName,
                    EmailAddress = info.EmailAddress,
                    RoleName = info.RoleName,
                    IsSuccess = true
                };
            }
            else
            {
                return new PersonInfoViewModel
                {
                    IsSuccess = false
                };
            }
        }
    }
}
