using ChangeRequestManagment.Models.ViewModels.ChangeRequestDocument;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChangeRequestManagement.BusinessLayer.Services.Abstraction
{
    public interface IChangeRequestDocumentService
    {
        Task AddChangeRequestDocumentAsync(List<AddChangeRequestDocumentViewModel> list);
        Task<List<ChangeRequestDocumentInfoViewModel>> GetChangeRequestDocumentByChangeRequestId(int changeRequestId);
    }
}
