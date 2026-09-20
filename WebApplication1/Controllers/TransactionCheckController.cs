using Microsoft.AspNetCore.Mvc;
using Contracts.Models;
using System.Text.Json;
using Contracts.Interfaces;

namespace FraudDetectionAPI.Controllers
{
    [Route("transaction/check")]
    [ApiController]
    public class TransactionCheckController(IRiskEngine riskEngine, IDatabase database) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> PostCheckTransaction([FromBody] Transaction transaction, CancellationToken cancellationToken)
        {
            var result = await riskEngine.TransactionCheck(transaction, cancellationToken);

            await database.SaveTransaction(transaction, cancellationToken);

            return Ok(JsonSerializer.Serialize(result));
        }

        [HttpGet]
        public async Task<IActionResult> GetTransactionCheck([FromQuery] Guid transactionId, CancellationToken cancellationToken)
        {
            var transaction = await database.GetTransaction(transactionId, cancellationToken);
            return Ok(JsonSerializer.Serialize(transaction));
        }
    }
}
