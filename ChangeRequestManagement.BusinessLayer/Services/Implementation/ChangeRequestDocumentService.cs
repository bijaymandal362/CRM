using ChangeRequestManagement.BusinessLayer.Services.Abstraction;
using ChangeRequestManagement.Entities;
using ChangeRequestManagement.Entities.Context;
using ChangeRequestManagment.Models.ViewModels.ChangeRequestDocument;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChangeRequestManagement.BusinessLayer.Services.Implementation
{
    public class ChangeRequestDocumentService : IChangeRequestDocumentService
    {
        private readonly CRMDbContext _context;
        private readonly ILogger<ChangeRequestDocumentService> _logger;

        public ChangeRequestDocumentService(
            CRMDbContext context,
            ILogger<ChangeRequestDocumentService> logger
            ) 
        {
            _context = context;
            _logger = logger;
        }

        public async Task AddChangeRequestDocumentAsync(List<AddChangeRequestDocumentViewModel> model)
        {
            try
            {
                foreach (var item in model) 
                {
                    var changeRequestDocumentData = new ChangeRequestDocument
                    {
                        DocumentPath = item.DocumentPath,
                        ChangeRequestId = item.ChangeRequestId,
                        ChangeRequestStatusId = item.ChangeRequestStatusId
                    };

                    var changeRequestDocumentResult = await _context.ChangeRequestDocument.AddAsync(changeRequestDocumentData);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex) 
            {
                _logger.LogError($"Exception occured: {ex.Message}");
            }
        }

        public async Task<List<ChangeRequestDocumentInfoViewModel>> GetChangeRequestDocumentByChangeRequestId(int changeRequestId) 
        {
            var changeRequestData = await _context.ChangeRequestDocument
                .Include(x=> x.ChangeRequest)
                .Where(x=> x.ChangeRequestId == changeRequestId)
                .ToListAsync();
            var changeRequestDocumentList = new List<ChangeRequestDocumentInfoViewModel>();
            foreach (var item in changeRequestData) 
            {
                changeRequestDocumentList.Add(new ChangeRequestDocumentInfoViewModel 
                {
                    ChangeRequestDocumentId = item.ChangeRequestDocumentId,
                    ChangeRequestId = item.ChangeRequestId,
                    DocumentPath = item.DocumentPath,
                    ChangeRequestStatusId = item.ChangeRequestStatusId
                });
            }
            return changeRequestDocumentList;
        }
    }
}
