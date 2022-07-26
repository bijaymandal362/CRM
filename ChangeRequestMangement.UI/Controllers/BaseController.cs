using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChangeRequestMangement.UI.Controllers
{
    [Authorize]
    public class BaseController : Controller
    {
    }
}
