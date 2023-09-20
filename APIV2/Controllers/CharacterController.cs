using APIV2.Models;
using APIV2.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace APIV2.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CharacterController : ControllerBase
    {
        private ICharacterRepository _characterRepository;
        public CharacterController(ICharacterRepository characterRepository)
        {
            _characterRepository = characterRepository;
        }

        // GET: api/characters/1
        [HttpGet("{id}")/*, Authorize(Roles = "Admin")*/]
        public async Task<IActionResult> GetCharacter(int id)
        {
            if (!await _characterRepository.entityExists(id))
                return NotFound();
            var character = await _characterRepository.GetById(id);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(character);
        }

        //api/character
        [HttpGet]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetCharacters()
        {
            var characteres = await _characterRepository.GetAllAsync();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(characteres);
        }

        //api/characters
        [HttpPost]
        public async Task<IActionResult> CreateCharacter([FromBody] Character createCharacter)
        {
            if (createCharacter == null)
            {
                return BadRequest(ModelState);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _characterRepository.Insert(createCharacter);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.GetBaseException().Message);
                return StatusCode(500, ModelState);
            }
            return CreatedAtAction("Getcharacter", new { id = createCharacter.Id }, createCharacter);
        }


        //api/characters/id
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCharacter(int id, [FromBody] Character updateCharacter)
        {
            if (updateCharacter == null)
            {
                return BadRequest(ModelState);
            }
            if (id != updateCharacter.Id)
            {
                return BadRequest(ModelState);
            }
            if (!await _characterRepository.entityExists(id))
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _characterRepository.Update(updateCharacter);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.GetBaseException().Message);
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }


        // DELETE: api/characters/3
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCharacter(int id)
        {
            if (!await _characterRepository.entityExists(id))
                return NotFound();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                await _characterRepository.Delete(id);
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