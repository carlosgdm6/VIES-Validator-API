using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ValidadorVIES.Services;
using ValidadorVIES.Models;

namespace ValidadorVIES.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ValidacaoController : ControllerBase
    {
        private readonly VIESService _viesService;

        public ValidacaoController(VIESService viesService)
        {
            _viesService = viesService;
        }

        [HttpGet("{pais}/{nif}")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<RespostaVIES>> ValidarContribuinte(string pais, string nif)
        {
            try
            {
                var resultado = await _viesService.ValidarContribuinteAsync(pais, nif);

                if (!resultado.Valido)
                {
                    return NotFound("Contribuinte inválido ou não encontrado no VIES.");
                }

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao validar contribuinte: {ex.Message}");
            }
        }
    }
}
