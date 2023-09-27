using APIV2.Dtos.Game;
using APIV2.Models;
using APIV2.Service.Interfaces;
using APIV2.Service.Repositories;
using Microsoft.AspNetCore.Authorization;
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
        [HttpGet("Random")]
        public async Task<IActionResult> GetRandomGame()
        {
            var game = await _gameRepository.GetRandomGameWithCharacters();
            var characters = new List<CharacterDto>();
            foreach (var character in game.Characters)
            {
                characters.Add(new CharacterDto
                {
                    Id = character.Id,
                    Name = character.Name,
                    Odds = character.Odds,
                    GameId = character.GameId
                });
            }
            GameDto randomGame = new GameDto() 
            {
                Id = game.Id,
                Name = game.Name,
                Characters = characters
            }; 
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(randomGame);
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
        [HttpGet, Authorize(Roles = "Admin")]
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
        [HttpPost, Authorize(Roles = "Admin")]
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
        [HttpPut("{id}"), Authorize(Roles = "Admin")]
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
        [HttpDelete("{id}"), Authorize(Roles = "Admin")]
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
