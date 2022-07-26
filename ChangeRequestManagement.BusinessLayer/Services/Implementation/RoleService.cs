using ChangeRequestManagement.BusinessLayer.Services.Abstraction;
using ChangeRequestManagement.Entities.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChangeRequestManagement.BusinessLayer.Services.Implementation
{
    public class RoleService: IRoleService
    {
        private readonly CRMDbContext _context;
        private readonly ILogger<RoleService> _logger;

        public RoleService(
            CRMDbContext context,
            ILogger<RoleService> logger) 
        {
            _context = context;
            _logger = logger;
        }

        public async Task<int> GetRoleIdByRoleNameAsync(string roleName) 
        {
        return await _context.Role
                .Where(x => x.RoleName == roleName.ToUpper())
                .Select(x => x.RoleId)
                .FirstAsync();
        }
    }
}
