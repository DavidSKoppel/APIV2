using APIV2.Dtos.BettingGame;
using APIV2.Models;
using APIV2.Service.Interfaces;
using APIV2.Service.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APIV2.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class BettingGameController : Controller
    {
        //private readonly GambleonContext _context;
        private IBettingGameRepository _bettingGameRepository;
        private readonly IConfiguration _configuration;
        public BettingGameController(IBettingGameRepository bettingGameRepository, IConfiguration configuration)
        {
            _bettingGameRepository = bettingGameRepository;
            _configuration = configuration;
        }

        [HttpGet("GetCurrentBettingGames")]
        public async Task<IActionResult> GetCurrentBettingGames()
        {
            var games = await _bettingGameRepository.GetAllCurrentGames();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var playedGames = new List<GetCurrentGamesBeingPlayed>();
            foreach (var game in games)
            {
                var characters = new List<CharacterDto>();
                var bettingHistories = new List<BettingHistoryDto>();
                foreach (var bet in game.BettingHistories)
                {
                    bettingHistories.Add(new BettingHistoryDto
                    {
                        Id = bet.Id,
                        WalletId = bet.Id,
                        BettingAmount = bet.BettingAmount,
                        BettingGameId = bet.BettingGameId,
                        BettingResult = bet.BettingResult,
                        BettingCharacterId = bet.BettingCharacterId,
                        Outcome = bet.Outcome
                    });
                }
                foreach (var character in game.Game.Characters)
                {
                    characters.Add(new CharacterDto
                    {
                        Id = character.Id,
                        Name = character.Name,
                        Odds = character.Odds,
                        GameId = character.GameId
                    });
                }
                playedGames.Add(new GetCurrentGamesBeingPlayed
                {
                    Id = game.Id,
                    GameId = game.GameId,
                    WinnerId = game.WinnerId,
                    beingPlayed = game.beingPlayed,
                    PlannedTime = game.PlannedTime.Value.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss"),
                    BettingHistories = bettingHistories,
                    Game = new GameDto()
                    {                                                                             
                        Id = game.Game.Id,
                        Name = game.Game.Name,
                        Desc = game.Game.Desc,
                        Characters = characters
                    }
                });
            }
            return Ok(playedGames);
        }

        [HttpGet("{id}")/*, Authorize(Roles = "Admin")*/]
        public async Task<IActionResult> GetBettingGame(int id)
        {
            if (!await _bettingGameRepository.entityExists(id))
                return NotFound();
            var bettingGame = await _bettingGameRepository.GetById(id);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(bettingGame);
        }

        //api/bettingGame
        [HttpGet, Authorize(Roles = "Admin")]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetBettingGames()
        {
            var bettingGamees = await _bettingGameRepository.GetAllAsync();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(bettingGamees);
        }

        //api/bettingGames
        [HttpPost]
        public async Task<IActionResult> CreateBettingGame([FromBody] BettingGame createBettingGame)
        {
            if (createBettingGame == null)
            {
                return BadRequest(ModelState);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _bettingGameRepository.Insert(createBettingGame);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.GetBaseException().Message);
                return StatusCode(500, ModelState);
            }
            return CreatedAtAction("GetbettingGame", new { id = createBettingGame.Id }, createBettingGame);
        }


        //api/bettingGames/id
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBettingGame(int id, [FromBody] BettingGame updateBettingGame)
        {
            if (updateBettingGame == null)
            {
                return BadRequest(ModelState);
            }
            if (id != updateBettingGame.Id)
            {
                return BadRequest(ModelState);
            }
            if (!await _bettingGameRepository.entityExists(id))
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _bettingGameRepository.Update(updateBettingGame);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.GetBaseException().Message);
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }


        // DELETE: api/bettingGames/3
        [HttpDelete("{id}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteBettingGame(int id)
        {
            if (!await _bettingGameRepository.entityExists(id))
                return NotFound();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                await _bettingGameRepository.Delete(id);
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