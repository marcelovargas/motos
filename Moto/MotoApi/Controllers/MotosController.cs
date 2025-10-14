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
        [ProducesResponseType(typeof(DTOs.Response.MotoDetailsResponseDto), 201)]
        [ProducesResponseType(typeof(DTOs.Response.ErrorResponseDto), 400)]
        [SwaggerOperation(
            Summary = "Cria uma nova moto",
            Description = "Adiciona uma nova moto ao sistema com os dados fornecidos"
        )]
        [SwaggerResponse(201, "Moto criada com sucesso", typeof(DTOs.Response.MotoDetailsResponseDto))]
        [SwaggerResponse(400, "Dados inválidos", typeof(DTOs.Response.ErrorResponseDto))]
        public async Task<IActionResult> CreateMoto([FromBody] DTOs.Request.CreateMotoRequest request)
        {
            // Validate the input
            if (request == null)
            {
                return BadRequest(new DTOs.Response.ErrorResponseDto { mensagem = "Dados inválidos" });
            }

            try
            {
                // Convert DTO to Moto model
                var moto = new Moto
                {
                    Identificador = request.Identificador,
                    Ano = request.Ano,
                    Modelo = request.Modelo,
                    Placa = request.Placa
                };

                // Validate model state
                if (!ModelState.IsValid)
                {
                    return BadRequest(new DTOs.Response.ErrorResponseDto { mensagem = "Dados inválidos" });
                }

                var createdMoto = await _motoService.CreateMotoAsync(moto);
                
                // Convert created moto to DTO for response without locacoes
                var motoDto = new DTOs.Response.MotoDetailsResponseDto
                {
                    Identificador = createdMoto.Identificador,
                    Ano = createdMoto.Ano,
                    Modelo = createdMoto.Modelo,
                    Placa = createdMoto.Placa
                };
                
                return CreatedAtAction(nameof(GetMotoById), new { id = createdMoto.Identificador }, motoDto);
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
        [ProducesResponseType(typeof(DTOs.Response.MotoDetailsResponseDto), 200)]
        [ProducesResponseType(typeof(DTOs.Response.ErrorResponseDto), 400)]
        [ProducesResponseType(typeof(DTOs.Response.ErrorResponseDto), 404)]
        [SwaggerOperation(
            Summary = "Busca moto por identificador"
                    )]
        [SwaggerResponse(200, "Moto encontrada", typeof(DTOs.Response.MotoDetailsResponseDto))]
        [SwaggerResponse(400, "Request mal formada", typeof(DTOs.Response.ErrorResponseDto))]
        [SwaggerResponse(404, "Moto não encontrada", typeof(DTOs.Response.ErrorResponseDto))]
        public async Task<ActionResult<DTOs.Response.MotoDetailsResponseDto>> GetMotoById(string id)
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

            // Convert Moto model to DTO without locacoes
            var motoDto = new DTOs.Response.MotoDetailsResponseDto
            {
                Identificador = moto.Identificador,
                Ano = moto.Ano,
                Modelo = moto.Modelo,
                Placa = moto.Placa
            };

            return motoDto;
        }


        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<DTOs.Response.MotoDetailsResponseDto>), 200)]
        [ProducesResponseType(typeof(DTOs.Response.ErrorResponseDto), 400)]
        [SwaggerOperation(
            Summary = "Consulta motos existentes"
        )]
        [SwaggerResponse(200, "Lista de motos retornada com sucesso", typeof(IEnumerable<DTOs.Response.MotoDetailsResponseDto>))]
        [SwaggerResponse(400, "Request mal formada", typeof(DTOs.Response.ErrorResponseDto))]
        public async Task<ActionResult<IEnumerable<DTOs.Response.MotoDetailsResponseDto>>> GetMotos([FromQuery] string? placa = null)
        {
            try
            {
                var motos = await _motoService.GetMotosAsync(placa);
                
                // Convert each Moto to MotoDetailsResponseDto to exclude locacoes
                var motosDto = motos.Select(m => new DTOs.Response.MotoDetailsResponseDto
                {
                    Identificador = m.Identificador,
                    Ano = m.Ano,
                    Modelo = m.Modelo,
                    Placa = m.Placa
                });
                
                return Ok(motosDto);
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
            Summary = "Elimina uma moto"
          
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
            catch (InvalidOperationException ex)
            {
                return BadRequest(new DTOs.Response.ErrorResponseDto { mensagem = ex.Message });
            }
            catch
            {
                return BadRequest(new DTOs.Response.ErrorResponseDto { mensagem = "Dados inválidos" });
            }
        }


        [HttpPut("{id}/placa")]
        [ProducesResponseType(typeof(DTOs.Response.ErrorResponseDto), 200)]
        [ProducesResponseType(typeof(DTOs.Response.ErrorResponseDto), 400)]
        [SwaggerOperation(
            Summary = "Modifica a placa de uma moto"
        )]
        [SwaggerResponse(200, "Placa atualizada com sucesso", typeof(DTOs.Response.ErrorResponseDto))]
        [SwaggerResponse(400, "Dados inválidos", typeof(DTOs.Response.ErrorResponseDto))]
        public async Task<IActionResult> UpdateMotoPlaca(string id, [FromBody] DTOs.Request.PlacaUpdateRequest request)
        {
            if (string.IsNullOrEmpty(id) || request?.Placa == null)
            {
                return BadRequest(new DTOs.Response.ErrorResponseDto { mensagem = "Dados inválidos" });
            }

            try
            {
                var updated = await _motoService.UpdateMotoPlacaAsync(id, request.Placa);

                if (updated)
                {
                    return Ok(new DTOs.Response.ErrorResponseDto { mensagem = "Placa atualizada com sucesso" });
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