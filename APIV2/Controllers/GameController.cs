using APIV2.Models;
using APIV2.Service.Interfaces;
using APIV2.Service.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace APIV2.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class GameController : Controller
    {
        //private readonly GambleonContext _context;
        private IGameRepository _gameRepository;
        private readonly IConfiguration _configuration;
        public GameController(IGameRepository gameRepository, IConfiguration configuration)
        {
            _gameRepository = gameRepository;
            _configuration = configuration;
        }
        [HttpGet("{id}")/*, Authorize(Roles = "Admin")*/]
        public async Task<IActionResult> GetGame(int id)
        {
            if (!await _gameRepository.entityExists(id))
                return NotFound();
            var game = await _gameRepository.GetById(id);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(game);
        }

        //api/game
        [HttpGet]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetGames()
        {
            var gamees = await _gameRepository.GetAllAsync();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(gamees);
        }

        //api/games
        [HttpPost]
        public async Task<IActionResult> CreateGame([FromBody] Game createGame)
        {
            if (createGame == null)
            {
                return BadRequest(ModelState);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _gameRepository.Insert(createGame);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.GetBaseException().Message);
                return StatusCode(500, ModelState);
            }
            return CreatedAtAction("Getgame", new { id = createGame.Id }, createGame);
        }


        //api/games/id
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGame(int id, [FromBody] Game updateGame)
        {
            if (updateGame == null)
            {
                return BadRequest(ModelState);
            }
            if (id != updateGame.Id)
            {
                return BadRequest(ModelState);
            }
            if (!await _gameRepository.entityExists(id))
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _gameRepository.Update(updateGame);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.GetBaseException().Message);
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }


        // DELETE: api/games/3
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGame(int id)
        {
            if (!await _gameRepository.entityExists(id))
                return NotFound();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                await _gameRepository.Delete(id);
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
