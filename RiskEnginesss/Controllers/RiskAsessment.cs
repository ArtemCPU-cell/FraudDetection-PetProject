using Microsoft.AspNetCore.Mvc;

namespace RiskEngine.Controllers
{
    [ApiController]
    [Route("risk-assessment/check")]
    public class RiskAsessment : Controller
    {
        [HttpPost]
        public IActionResult Check()
        {
            return Ok();
        }
    }
}
