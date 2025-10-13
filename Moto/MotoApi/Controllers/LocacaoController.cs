using Microsoft.AspNetCore.Mvc;
using MotoApi.DTOs.Response;
using MotoApi.Models;
using MotoApi.Services.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace MotoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LocacaoController : ControllerBase
    {
        private readonly ILocacaoService _locacaoService;

        public LocacaoController(ILocacaoService locacaoService)
        {
            _locacaoService = locacaoService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(Locacao), 201)]
        [ProducesResponseType(typeof(ErrorResponseDto), 400)]
        [SwaggerOperation(
            Summary = "Cria uma nova locação",
            Description = "Registra uma nova locação de moto para um entregador"
        )]
        [SwaggerResponse(201, "Locação criada com sucesso", typeof(Locacao))]
        [SwaggerResponse(400, "Dados inválidos", typeof(ErrorResponseDto))]
        public async Task<IActionResult> CreateLocacao([FromBody] Locacao locacao)
        {
            // Validate the input
            if (locacao == null)
            {
                return BadRequest(new ErrorResponseDto { mensagem = "Dados inválidos" });
            }

            try
            {
                // Validate required fields
                if (string.IsNullOrEmpty(locacao.EntregadorId) ||
                    string.IsNullOrEmpty(locacao.MotoId))
                {
                    return BadRequest(new ErrorResponseDto { mensagem = "Dados inválidos" });
                }

                // Validate plan
                if (locacao.Plano <= 0)
                {
                    return BadRequest(new ErrorResponseDto { mensagem = "Dados inválidos" });
                }

                // Validate dates
                if (locacao.DataPrevisaoTermino < locacao.DataInicio ||
                    locacao.DataTermino < locacao.DataInicio)
                {
                    return BadRequest(new ErrorResponseDto { mensagem = "Dados inválidos" });
                }

                // Call the service to create the locacao
                var createdLocacao = await _locacaoService.CreateLocacaoAsync(locacao);
                
                return Created($"/api/locacao/{createdLocacao.EntregadorId}-{createdLocacao.MotoId}", createdLocacao);
            }
            catch (Exception)
            {
                return BadRequest(new ErrorResponseDto { mensagem = "Dados inválidos" });
            }
        }
    }
}