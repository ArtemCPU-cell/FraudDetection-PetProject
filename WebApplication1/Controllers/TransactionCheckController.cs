using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FraudDetectionAPI.Repositories;

namespace FraudDetectionAPI.Controllers
{
    [Route("transaction/check")]
    [ApiController]
    public class TransactionCheckController : ControllerBase
    {
        [HttpPost]
        public IActionResult PostCheckTransaction([FromBody] Transaction transaction)
        {
            return Ok(transaction.TransactionId);
        }

        [HttpGet]
        public IActionResult GetTransactionCheck([FromQuery] string transactionId)
        {
            return Ok(transactionId);
        }
    }
}
