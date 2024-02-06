using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("Link")]
    public class LinkController : Controller
    {
        [HttpPost]
        public async Task<IActionResult> Add(string url,string shortUrl)
        {
            return View();
        }
    }
}
