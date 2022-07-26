using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ChangeRequestMangement.UI.Controllers
{
    public class DocumentController:BaseController
    {
		private readonly IWebHostEnvironment _environment;

		public DocumentController(IWebHostEnvironment environment) 
        {
            _environment = environment;
        }
        public IActionResult Download(string documentPath) 
        {
            const string documentFolder = "CRMFiles";
            var filePath = Path.Combine(_environment.WebRootPath, documentFolder, documentPath);
            var net = new WebClient();
            var data = net.DownloadData(filePath);
            var contentType = "APPLICATION/octet-stream";
            var fileName = documentPath;
            var content = new MemoryStream(data);
            return File(content, contentType, fileName);
        }
    }
}
