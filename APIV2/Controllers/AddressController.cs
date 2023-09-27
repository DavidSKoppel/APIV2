using APIV2.Dtos.Address;
using APIV2.Models;
using APIV2.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APIV2.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private IAddressRepository _addressRepository;
        public AddressController(IAddressRepository addressRepository)
        {
            _addressRepository = addressRepository;
        }

        // GET: api/Addresss/1
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAddress(int id)
        {
            if (!await _addressRepository.entityExists(id))
                return NotFound();
            var address = await _addressRepository.GetById(id);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(address);
        }

        //api/Address
        [HttpGet, Authorize(Roles = "Admin")]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetAddresses()
        {
            var addresses = await _addressRepository.GetAllAsync();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(addresses);
        }

        [HttpGet("GetAllAddressInfo")]
        public async Task<IActionResult> GetAllInfoAddresses()
        {
            var addresses = await _addressRepository.GetAllAsync();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var addressDto = new List<AddressDto>();

            foreach (var address in addresses)
            {
                addressDto.Add(new AddressDto
                {
                    Address1 = address.Address1,
                    PostalCode = (int)address.PostalCode
                });
            }
            return Ok(addressDto);
        }

        //api/Addresss
        [HttpPost, Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateAddress([FromBody] Address createAddress)
        {
            if (createAddress == null)
            {
                return BadRequest(ModelState);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _addressRepository.Insert(createAddress);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.GetBaseException().Message);
                return StatusCode(500, ModelState);
            }

            return CreatedAtAction("GetAddress", new { id = createAddress.Id }, createAddress);
        }


        //api/Addresss/id
        [HttpPut("{id}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateAddress(int id, [FromBody] Address updateAddress)
        {
            if (updateAddress == null)
            {
                return BadRequest(ModelState);
            }
            if (id != updateAddress.Id)
            {
                return BadRequest(ModelState);
            }
            if (!await _addressRepository.entityExists(id))
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _addressRepository.Update(updateAddress);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.GetBaseException().Message);
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }


        // DELETE: api/Addresss/3
        [HttpDelete("{id}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteAddress(int id)
        {
            if (!await _addressRepository.entityExists(id))
                return NotFound();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _addressRepository.Delete(id);
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
