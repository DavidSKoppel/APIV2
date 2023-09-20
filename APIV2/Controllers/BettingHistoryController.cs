using APIV2.Models;
using APIV2.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace APIV2.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BettingHistoryController : ControllerBase
    {
        private IBettingHistoryRepository _bettingHistoryRepository;
        public BettingHistoryController(IBettingHistoryRepository bettingHistoryRepository)
        {
            _bettingHistoryRepository = bettingHistoryRepository;
        }

        [HttpGet("UserId/{id}")/*, Authorize(Roles = "Admin")*/]
        public async Task<IActionResult> GetBettingHistoriesByUserId(int id)
        {
            var bettingHistory = await _bettingHistoryRepository.GetBettingHistoriesByUserId(id);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (bettingHistory == null)
                return NotFound();

            return Ok(bettingHistory);
        }

        // GET: api/bettingHistorys/1
        [HttpGet("{id}")/*, Authorize(Roles = "Admin")*/]
        public async Task<IActionResult> GetBettingHistory(int id)
        {
            if (!await _bettingHistoryRepository.entityExists(id))
                return NotFound();
            var bettingHistory = await _bettingHistoryRepository.GetById(id);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(bettingHistory);
        }

        //api/bettingHistory
        [HttpGet]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetBettingHistories()
        {
            var bettingHistoryes = await _bettingHistoryRepository.GetAllAsync();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(bettingHistoryes);
        }

        //api/bettingHistorys
        [HttpPost]
        public async Task<IActionResult> CreateBettingHistory([FromBody] BettingHistory createBettingHistory)
        {
            if (createBettingHistory == null)
            {
                return BadRequest(ModelState);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _bettingHistoryRepository.Insert(createBettingHistory);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.GetBaseException().Message);
                return StatusCode(500, ModelState);
            }
            return CreatedAtAction("GetbettingHistory", new { id = createBettingHistory.Id }, createBettingHistory);
        }


        //api/bettingHistorys/id
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBettingHistory(int id, [FromBody] BettingHistory updateBettingHistory)
        {
            if (updateBettingHistory == null)
            {
                return BadRequest(ModelState);
            }
            if (id != updateBettingHistory.Id)
            {
                return BadRequest(ModelState);
            }
            if (!await _bettingHistoryRepository.entityExists(id))
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _bettingHistoryRepository.Update(updateBettingHistory);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.GetBaseException().Message);
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }


        // DELETE: api/bettingHistorys/3
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBettingHistory(int id)
        {
            if (!await _bettingHistoryRepository.entityExists(id))
                return NotFound();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                await _bettingHistoryRepository.Delete(id);
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
