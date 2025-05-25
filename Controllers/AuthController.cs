using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ValidadorVIES.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        [HttpPost("login")]
        public IActionResult Login(string username, string password)
        {
            // Lógica de login fictícia (podes adaptar para autenticar com base de dados, etc.)
            if (username == "admin" && password == "admin123")
            {
                var token = GerarToken("admin");
                return Ok(new { token });
            }

            if (username == "user" && password == "user123")
            {
                var token = GerarToken("user");
                return Ok(new { token });
            }

            return Unauthorized();
        }

        private string GerarToken(string role)
        {
            var chave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("EstaChaveTemDeSerSeguraESecreta123!"));
            var credenciais = new SigningCredentials(chave, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Role, role)
            };

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credenciais
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
