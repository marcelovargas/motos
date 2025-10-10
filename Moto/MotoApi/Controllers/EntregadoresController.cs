using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MotoApi.DTOs.Request;
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
        private readonly IFileStorageService _fileStorageService;

        public EntregadoresController(IEntregadorService entregadorService, IFileStorageService fileStorageService)
        {
            _entregadorService = entregadorService;
            _fileStorageService = fileStorageService;
        }

       
        [HttpPost]
        [ProducesResponseType(typeof(Entregador), 201)]
        [ProducesResponseType(typeof(DTOs.Response.ErrorResponseDto), 400)]
        [SwaggerOperation(
            Summary = "Cadastra um novo entregador"
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
                
                if (!ModelState.IsValid)
                {
                    return BadRequest(new DTOs.Response.ErrorResponseDto { mensagem = "Dados inválidos" });
                }

                var createdEntregador = await _entregadorService.CreateEntregadorAsync(entregador);
                return Created($"/api/entregadores/{createdEntregador.Identificador}", createdEntregador);
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

        
        [HttpPost("{id}/cnh")]
        [ProducesResponseType(typeof(DTOs.Response.ErrorResponseDto), 200)]
        [ProducesResponseType(typeof(DTOs.Response.ErrorResponseDto), 400)]
        [ProducesResponseType(typeof(DTOs.Response.ErrorResponseDto), 404)]
        [SwaggerOperation(
            Summary = "Enviar foto do CNH"
        )]
        [SwaggerResponse(200, "Foto do CNH atualizada com sucesso", typeof(DTOs.Response.ErrorResponseDto))]
        [SwaggerResponse(400, "Dados inválidos", typeof(DTOs.Response.ErrorResponseDto))]
        [SwaggerResponse(404, "Entregador não encontrado", typeof(DTOs.Response.ErrorResponseDto))]
        public async Task<IActionResult> UpdateCnhImage(string id, [FromBody] CnhUploadRequest request)
        {
            if (string.IsNullOrEmpty(id) || request?.Imagem_cnh == null)
            {
                return BadRequest(new DTOs.Response.ErrorResponseDto { mensagem = "Dados inválidos" });
            }

            try
            {
              
                byte[] imageBytes;
                try
                {
                    imageBytes = Convert.FromBase64String(request.Imagem_cnh);
                }
                catch (FormatException)
                {
                    return BadRequest(new DTOs.Response.ErrorResponseDto { mensagem = "Dados inválidos" });
                }

               
                string extension;
                if (IsPngImage(imageBytes))
                {
                    extension = ".png";
                }
                else if (IsBmpImage(imageBytes))
                {
                    extension = ".bmp";
                }
                else
                {
                    return BadRequest(new DTOs.Response.ErrorResponseDto { mensagem = "Dados inválidos" });
                }

                
                var fileName = $"{id}_cnh{extension}";
                
                
                var imagePath = await _fileStorageService.SaveImageFromBase64Async(imageBytes, "cnh-images", fileName, "");

               
                var updated = await _entregadorService.UpdateEntregadorCnhImageAsync(id, imagePath);

                if (updated)
                {
                    return Ok(new DTOs.Response.ErrorResponseDto { mensagem = "Foto do CNH atualizada com sucesso" });
                }
                else
                {
                    return NotFound(new DTOs.Response.ErrorResponseDto { mensagem = "Entregador não encontrado" });
                }
            }
            catch
            {
                return BadRequest(new DTOs.Response.ErrorResponseDto { mensagem = "Dados inválidos" });
            }
        }

       
        private bool IsPngImage(byte[] imageBytes)
        {
            if (imageBytes.Length < 8) return false;
            return imageBytes[0] == 0x89 && imageBytes[1] == 0x50 && imageBytes[2] == 0x4E && imageBytes[3] == 0x47;
        }

        
        private bool IsBmpImage(byte[] imageBytes)
        {
            if (imageBytes.Length < 2) return false;
            return imageBytes[0] == 0x42 && imageBytes[1] == 0x4D; 
        }
    }
}