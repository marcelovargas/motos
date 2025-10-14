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
        [ProducesResponseType(201)]
        [ProducesResponseType(typeof(ErrorResponseDto), 400)]
        [SwaggerOperation(
            Summary = "Cria uma nova locação"
           
        )]
        [SwaggerResponse(201, "Locação criada com sucesso")]
        [SwaggerResponse(400, "Dados inválidos", typeof(ErrorResponseDto))]
        public async Task<IActionResult> CreateLocacao([FromBody] CreateLocacaoRequest request)
        {
            
            if (request == null)
            {
                return BadRequest(new ErrorResponseDto { mensagem = "Dados inválidos" });
            }

            if (string.IsNullOrEmpty(request.entregador_id) ||
                string.IsNullOrEmpty(request.moto_id))
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
                
                await _locacaoService.CreateLocacaoAsync(locacao);
                
                return StatusCode(201); // Retorna apenas status 201, sem corpo
            }
            catch (ArgumentException)
            {
                return BadRequest(new ErrorResponseDto { mensagem = "Dados inválidos" });
            }
            catch (Exception)
            {
                return BadRequest(new ErrorResponseDto { mensagem = "Dados inválidos" });
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(GetLocacaoResponse), 200)]
        [ProducesResponseType(typeof(ErrorResponseDto), 400)]
        [ProducesResponseType(typeof(ErrorResponseDto), 404)]
        [SwaggerOperation(
            Summary = "Consulta uma locação por ID"
        )]
        [SwaggerResponse(200, "Locação encontrada", typeof(GetLocacaoResponse))]
        [SwaggerResponse(400, "Dados inválidos", typeof(ErrorResponseDto))]
        [SwaggerResponse(404, "Locação não encontrada", typeof(ErrorResponseDto))]
        public async Task<IActionResult> GetLocacaoById(string id)
        {
            // Validate the input
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest(new ErrorResponseDto { mensagem = "Dados inválidos" });
            }

            try
            {
                var locacao = await _locacaoService.GetByIdAsync(id);
                
                if (locacao == null)
                {
                    return NotFound(new ErrorResponseDto { mensagem = "Locação não encontrada" });
                }

                // Determinar o valor diário com base no plano
                var valorDiaria = PlanosLocacao.GetValorPorDia(locacao.Plano);

                var response = new GetLocacaoResponse
                {
                    Identificador = locacao.Identificador,
                    ValorDiaria = valorDiaria,
                    EntregadorId = locacao.EntregadorId,
                    MotoId = locacao.MotoId,
                    DataInicio = locacao.DataInicio,
                    DataTermino = locacao.DataTermino,
                    DataPrevisaoTermino = locacao.DataPrevisaoTermino,
                    DataDevolucao = locacao.DataTermino // Assumindo que a data de devolução é a mesma que a data de término
                };

                return Ok(response);
            }
            catch (Exception)
            {
                return BadRequest(new ErrorResponseDto { mensagem = "Dados inválidos" });
            }
        }
    }
}