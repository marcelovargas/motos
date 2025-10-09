using Microsoft.AspNetCore.Mvc;
using MotoApi.Models;
using MotoApi.Services.Interfaces;

namespace MotoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MotoController : ControllerBase
    {
        private readonly IMotoService _motoService;

        public MotoController(IMotoService motoService)
        {
            _motoService = motoService;
        }

        // POST: api/Moto
        [HttpPost]
        public async Task<ActionResult<Moto>> CreateMoto(Moto moto)
        {
            // Validate the input
            if (moto == null)
            {
                return BadRequest("Moto data is required.");
            }

            try
            {
                var createdMoto = await _motoService.CreateMotoAsync(moto);
                return CreatedAtAction(nameof(GetMotoById), new { id = createdMoto.Identificador }, createdMoto);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: api/Moto/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Moto>> GetMotoById(string id)
        {
            var moto = await _motoService.GetMotoByIdAsync(id);

            if (moto == null)
            {
                return NotFound();
            }

            return moto;
        }
    }
}