using Microsoft.AspNetCore.Mvc;
using MotoApi.Models;
using MotoApi.Services.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace MotoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MotosController : ControllerBase
    {
        private readonly IMotoService _motoService;

        public MotosController(IMotoService motoService)
        {
            _motoService = motoService;
        }

       
        [HttpPost]
        [ProducesResponseType(typeof(Moto), 201)]
        [ProducesResponseType(typeof(DTOs.Response.ErrorResponseDto), 400)]
        [SwaggerOperation(
            Summary = "Cria uma nova moto",
            Description = "Adiciona uma nova moto ao sistema com os dados fornecidos"
        )]
        [SwaggerResponse(201, "Moto criada com sucesso", typeof(Moto))]
        [SwaggerResponse(400, "Dados inválidos", typeof(DTOs.Response.ErrorResponseDto))]
        public async Task<IActionResult> CreateMoto(Moto moto)
        {
            // Validate the input
            if (moto == null)
            {
                return BadRequest(new DTOs.Response.ErrorResponseDto { mensagem = "Dados inválidos" });
            }

            try
            {
                // Validate model state
                if (!ModelState.IsValid)
                {
                    return BadRequest(new DTOs.Response.ErrorResponseDto { mensagem = "Dados inválidos" });
                }

                var createdMoto = await _motoService.CreateMotoAsync(moto);
                return CreatedAtAction(nameof(GetMotoById), new { id = createdMoto.Identificador }, createdMoto);
            }
            catch (ArgumentException)
            {
                return BadRequest(new DTOs.Response.ErrorResponseDto { mensagem = "Deu algum problema" });
            }
            catch
            {
                return BadRequest(new DTOs.Response.ErrorResponseDto { mensagem = "Deu algum problema" });
            }
        }

       
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Moto), 200)]
        [ProducesResponseType(typeof(DTOs.Response.ErrorResponseDto), 400)]
        [ProducesResponseType(typeof(DTOs.Response.ErrorResponseDto), 404)]
        [SwaggerOperation(
            Summary = "Busca moto por identificador",
            Description = "Retorna os dados de uma moto específica baseado no identificador fornecido"
        )]
        [SwaggerResponse(200, "Moto encontrada", typeof(Moto))]
        [SwaggerResponse(400, "Request mal formada", typeof(DTOs.Response.ErrorResponseDto))]
        [SwaggerResponse(404, "Moto não encontrada", typeof(DTOs.Response.ErrorResponseDto))]
        public async Task<ActionResult<Moto>> GetMotoById(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest(new DTOs.Response.ErrorResponseDto { mensagem = "Request mal formada" });
            }

            var moto = await _motoService.GetMotoByIdAsync(id);

            if (moto == null)
            {
                return NotFound(new DTOs.Response.ErrorResponseDto { mensagem = "Moto não encontrada" });
            }

            return moto;
        }

        
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Moto>), 200)]
        [ProducesResponseType(typeof(DTOs.Response.ErrorResponseDto), 400)]
        [SwaggerOperation(
            Summary = "Consulta motos existentes",
            Description = "Retorna uma lista de motos, com opção de filtrar pela placa"
        )]
        [SwaggerResponse(200, "Lista de motos retornada com sucesso", typeof(IEnumerable<Moto>))]
        [SwaggerResponse(400, "Request mal formada", typeof(DTOs.Response.ErrorResponseDto))]
        public async Task<ActionResult<IEnumerable<Moto>>> GetMotos([FromQuery] string? placa = null)
        {
            try
            {
                var motos = await _motoService.GetMotosAsync(placa);
                return Ok(motos);
            }
            catch (Exception)
            {
                return BadRequest(new DTOs.Response.ErrorResponseDto { mensagem = "Request mal formada" });
            }
        }

        
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(DTOs.Response.ErrorResponseDto), 200)]
        [ProducesResponseType(typeof(DTOs.Response.ErrorResponseDto), 400)]
        [SwaggerOperation(
            Summary = "Elimina uma moto",
            Description = "Remove uma moto existente pelo identificador"
        )]
        [SwaggerResponse(200, "Moto eliminada com sucesso", typeof(DTOs.Response.ErrorResponseDto))]
        [SwaggerResponse(400, "Dados inválidos", typeof(DTOs.Response.ErrorResponseDto))]
        public async Task<IActionResult> DeleteMoto(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest(new DTOs.Response.ErrorResponseDto { mensagem = "Dados inválidos" });
            }

            try
            {
                var deleted = await _motoService.DeleteMotoAsync(id);
                
                if (deleted)
                {
                    return Ok(new DTOs.Response.ErrorResponseDto { mensagem = "Moto eliminada com sucesso" });
                }
                else
                {
                    return BadRequest(new DTOs.Response.ErrorResponseDto { mensagem = "Dados inválidos" });
                }
            }
            catch (ArgumentException)
            {
                return BadRequest(new DTOs.Response.ErrorResponseDto { mensagem = "Dados inválidos" });
            }
            catch
            {
                return BadRequest(new DTOs.Response.ErrorResponseDto { mensagem = "Dados inválidos" });
            }
        }
    }
}