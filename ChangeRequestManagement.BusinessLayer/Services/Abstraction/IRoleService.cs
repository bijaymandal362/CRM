using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChangeRequestManagement.BusinessLayer.Services.Abstraction
{
    public interface IRoleService
    {
        Task<int> GetRoleIdByRoleNameAsync(string roleName);
    }
}
