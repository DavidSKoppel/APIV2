using APIV2.Models;
using APIV2.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace APIV2.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private ITransactionRepository _transactionRepository;
        public TransactionController(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        [HttpGet("UserId/{id}")]
        public async Task<IActionResult> GetTransactionsByUserId(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var transaction = await _transactionRepository.GetTransactionsByUserId(id);
            
            if (transaction == null)
                return NotFound();

            return Ok(transaction);
        }

        // GET: api/transactions/1
        [HttpGet("{id}")/*, Authorize(Roles = "Admin")*/]
        public async Task<IActionResult> GetTransaction(int id)
        {
            if (!await _transactionRepository.entityExists(id))
                return NotFound();
            var transaction = await _transactionRepository.GetById(id);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(transaction);
        }

        //api/transaction
        [HttpGet]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetTransactions()
        {
            var transactiones = await _transactionRepository.GetAllAsync();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(transactiones);
        }

        //api/transactions
        [HttpPost]
        public async Task<IActionResult> CreateTransaction([FromBody] Transaction createTransaction)
        {
            if (createTransaction == null)
            {
                return BadRequest(ModelState);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _transactionRepository.Insert(createTransaction);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.GetBaseException().Message);
                return StatusCode(500, ModelState);
            }
            return CreatedAtAction("Gettransaction", new { id = createTransaction.Id }, createTransaction);
        }


        //api/transactions/id
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTransaction(int id, [FromBody] Transaction updateTransaction)
        {
            if (updateTransaction == null)
            {
                return BadRequest(ModelState);
            }
            if (id != updateTransaction.Id)
            {
                return BadRequest(ModelState);
            }
            if (!await _transactionRepository.entityExists(id))
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _transactionRepository.Update(updateTransaction);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.GetBaseException().Message);
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }


        // DELETE: api/transactions/3
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransaction(int id)
        {
            if (!await _transactionRepository.entityExists(id))
                return NotFound();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                await _transactionRepository.Delete(id);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.GetBaseException().Message);
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
    }
}
