using Microsoft.AspNetCore.Mvc;
using MotoApi.DTOs.Request;
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
        public async Task<IActionResult> CreateLocacao([FromBody] CreateLocacaoRequest request)
        {
            // Validate the input
            if (request == null)
            {
                return BadRequest(new ErrorResponseDto { mensagem = "Dados inválidos" });
            }

          
            if (string.IsNullOrEmpty(request.entregador_id) ||
                string.IsNullOrEmpty(request.moto_id))
            {
                return BadRequest(new ErrorResponseDto { mensagem = "Dados inválidos" });
            }

          
            if (request.plano <= 0)
            {
                return BadRequest(new ErrorResponseDto { mensagem = "Dados inválidos" });
            }

            if (request.data_previsao_termino < request.data_inicio ||
                (request.data_termino.HasValue && request.data_termino.Value < request.data_inicio))
            {
                return BadRequest(new ErrorResponseDto { mensagem = "Dados inválidos" });
            }

            // Map the DTO to the model
            var locacao = new Locacao
            {
                EntregadorId = request.entregador_id,
                MotoId = request.moto_id,
                DataInicio = request.data_inicio,
                DataTermino = request.data_termino,
                DataPrevisaoTermino = request.data_previsao_termino,
                Plano = request.plano
            };

            try
            {
                // Call the service to create the locacao
                var createdLocacao = await _locacaoService.CreateLocacaoAsync(locacao);
                
                return Created($"/api/locacao/{createdLocacao.Identificador}", createdLocacao);
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResponseDto { mensagem = $"Erro ao criar locação: {ex.Message}" });
            }
        }
    }
}