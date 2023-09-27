using APIV2.Models;
using APIV2.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APIV2.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserTypeController : ControllerBase
    {
        private IUserTypeRepository _userTypeRepository;
        public UserTypeController(IUserTypeRepository userTypeRepository)
        {
            _userTypeRepository = userTypeRepository;
        }

        // GET: api/userTypes/1
        [HttpGet("{id}")/*, Authorize(Roles = "Admin")*/]
        public async Task<IActionResult> GetUserType(int id)
        {
            if (!await _userTypeRepository.entityExists(id))
                return NotFound();
            var userType = await _userTypeRepository.GetById(id);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(userType);
        }

        //api/userType
        [HttpGet, Authorize(Roles = "Admin")]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetUserTypes()
        {
            var userTypees = await _userTypeRepository.GetAllAsync();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(userTypees);
        }

        //api/userTypes
        [HttpPost, Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateUserType([FromBody] UserType createUserType)
        {
            if (createUserType == null)
            {
                return BadRequest(ModelState);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _userTypeRepository.Insert(createUserType);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.GetBaseException().Message);
                return StatusCode(500, ModelState);
            }
            return CreatedAtAction("GetuserType", new { id = createUserType.Id }, createUserType);
        }


        //api/userTypes/id
        [HttpPut("{id}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateUserType(int id, [FromBody] UserType updateUserType)
        {
            if (updateUserType == null)
            {
                return BadRequest(ModelState);
            }
            if (id != updateUserType.Id)
            {
                return BadRequest(ModelState);
            }
            if (!await _userTypeRepository.entityExists(id))
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _userTypeRepository.Update(updateUserType);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.GetBaseException().Message);
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }


        // DELETE: api/userTypes/3
        [HttpDelete("{id}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUserType(int id)
        {
            if (!await _userTypeRepository.entityExists(id))
                return NotFound();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                await _userTypeRepository.Delete(id);
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
