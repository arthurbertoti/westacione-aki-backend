using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WEstacionaAPI.DbContexto;
using WEstacionaAPI.Dto.Valores;

namespace WEstacionaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthMiddleware : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AuthMiddleware(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /*
        [HttpPost("[Action]")]
        public async Task<IActionResult> GenerateToken(UsuarioAcesso request)

        {
            
            if (string.IsNullOrEmpty(request.Username) || string.IsNullOrWhiteSpace(request.Username))
            {
                return Unauthorized(new Resposta { Sucesso = false, Mensagem = "Username is missing" });
            }

            if (!Request.Headers.TryGetValue("Authorization", out var authorizationHeader))
            {
                return Unauthorized(new Resposta { Sucesso = false, Mensagem = "Authorization header is missing" });
            }

            var authHeader = authorizationHeader.ToString();
            if (!authHeader.StartsWith("Basic "))
            {
                return Unauthorized(new Resposta { Sucesso = false, Mensagem = "Authorization header is not in Basic format" });
            }
            //var issuer = await _databaseService.GetIssuerAsync();
            //var audience = await _databaseService.GetAudienceAsync();

            var base64Credentials = authHeader.Substring("Basic ".Length).Trim();
            var credentials = Encoding.UTF8.GetString(Convert.FromBase64String(base64Credentials)).Split(':');
            if (credentials.Length != 2)
            {
                return Unauthorized(new Resposta { Sucesso = false, Mensagem = "Invalid Basic auth credentials format" });
            }

            var secretKey = credentials[1];

            var storedSecretKey = _configuration["JwtSettings:SecretKey"];
            if (secretKey != storedSecretKey)
            {
                return Unauthorized(new Resposta { Sucesso = false, Mensagem = "Invalid SecretKey" });
            }

            var usuario = new Usuario(_configuration);
            var loginResult = await usuario.LoginUsuario(request.Username, request.Password);

            if (!loginResult.Sucesso)
            {
                return Unauthorized(new Resposta { Sucesso = false, Mensagem = loginResult.Mensagem });
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(storedSecretKey);

            

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, request.Username)
                }),

                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(new Resposta
            {
                Sucesso = true,
                Mensagem = "Token generated successfully",
                Token = tokenString
            });
        }
            */
    }

    public class UsuarioAcesso
    {
        public string Username { get; set; }
        public string Password { get; set; } = string.Empty;
    }
}
