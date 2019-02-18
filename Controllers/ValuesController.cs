using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthBug.Controllers
{
    [Route("[controller]")]
    public class ValueController : Controller
    {
        [HttpGet]
        [Authorize(AuthenticationSchemes = ApiKeyDefaults.AuthenticationScheme)]
        public int GetValue() => 3;

        [HttpGet("/OtherValue")]
        [AllowAnonymous]
        public int GetOtherValue() => 5;
    }
}
