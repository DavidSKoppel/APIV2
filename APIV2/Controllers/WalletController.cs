using APIV2.Models;
using APIV2.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APIV2.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class WalletController : Controller
    {
        //private readonly GambleonContext _context;
        private IWalletRepository _walletRepository;
        private readonly IConfiguration _configuration;
        public WalletController(IWalletRepository walletRepository, IConfiguration configuration)
        {
            _walletRepository = walletRepository;
            _configuration = configuration;
        }
        
        [HttpGet("UserId/{id}")]
        public async Task<IActionResult> GetWalletByUserId(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var wallet = await _walletRepository.GetWalletByUserId(id);
            
            if(wallet == null)
                return NotFound();

            return Ok(wallet);
        }

        [HttpGet("{id}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetWallet(int id)
        {
            if (!await _walletRepository.entityExists(id))
                return NotFound();
            var wallet = await _walletRepository.GetById(id);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(wallet);
        }

        //api/wallet
        [HttpGet, Authorize(Roles = "Admin")]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetWallets()
        {
            var walletes = await _walletRepository.GetAllAsync();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(walletes);
        }

        //api/wallets
        [HttpPost, Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateWallet([FromBody] Wallet createWallet)
        {
            if (createWallet == null)
            {
                return BadRequest(ModelState);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _walletRepository.Insert(createWallet);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.GetBaseException().Message);
                return StatusCode(500, ModelState);
            }
            return CreatedAtAction("Getwallet", new { id = createWallet.Id }, createWallet);
        }


        //api/wallets/id
        [HttpPut("{id}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateWallet(int id, [FromBody] Wallet updateWallet)
        {
            if (updateWallet == null)
            {
                return BadRequest(ModelState);
            }
            if (id != updateWallet.Id)
            {
                return BadRequest(ModelState);
            }
            if (!await _walletRepository.entityExists(id))
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _walletRepository.Update(updateWallet);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.GetBaseException().Message);
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }


        // DELETE: api/wallets/3
        [HttpDelete("{id}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteWallet(int id)
        {
            if (!await _walletRepository.entityExists(id))
                return NotFound();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                await _walletRepository.Delete(id);
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
