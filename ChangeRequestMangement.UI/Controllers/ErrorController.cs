using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChangeRequestMangement.UI.Controllers
{

    [AllowAnonymous]
    public class ErrorController : BaseController
    {
        [Route("Error")]
        public IActionResult Error()
        {
            ViewData["ErrorTitle"] = "Internal Server Error";
            ViewData["Message"] = "We will work on fixing that right away.";
            ViewData["StatusCode"] = StatusCodes.Status500InternalServerError;
            return View();
        }

        [HttpGet("Error/PageNotFound/{statusCode}")]
        public IActionResult PageNotFound(int statusCode) 
        {
            ViewData["ErrorTitle"] = "Page Not Found";
            ViewData["Message"] = "Sorry, your requested page is not found.";
            ViewData["StatusCode"] = statusCode;
            return View("Error");
        }
    }
}
