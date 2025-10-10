using Microsoft.AspNetCore.Mvc;
using MotoApi.Models;
using MotoApi.Services.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace MotoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EntregadoresController : ControllerBase
    {
        private readonly IEntregadorService _entregadorService;

        public EntregadoresController(IEntregadorService entregadorService)
        {
            _entregadorService = entregadorService;
        }

       
        [HttpPost]
        [ProducesResponseType(typeof(Entregador), 201)]
        [ProducesResponseType(typeof(DTOs.Response.ErrorResponseDto), 400)]
        [SwaggerOperation(
            Summary = "Cadastra um novo entregador",
            Description = "Adiciona um novo entregador ao sistema com os dados fornecidos"
        )]
        [SwaggerResponse(201, "Entregador criado com sucesso", typeof(Entregador))]
        [SwaggerResponse(400, "Dados inválidos", typeof(DTOs.Response.ErrorResponseDto))]
        public async Task<IActionResult> CreateEntregador(Entregador entregador)
        {
            // Validate the input
            if (entregador == null)
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

                var createdEntregador = await _entregadorService.CreateEntregadorAsync(entregador);
                return Created($"/api/entregadores/{createdEntregador.Identificador}", createdEntregador);
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
    }
}