using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;

namespace ViewCounterExample.Controllers
{
    public abstract class BaseController : Controller
    {
        //Get UserId from Claims
        protected string UserId => HttpContext
            .User?
            .Claims?
            .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?
            .Value ?? Guid.NewGuid().ToString();
    }
}
