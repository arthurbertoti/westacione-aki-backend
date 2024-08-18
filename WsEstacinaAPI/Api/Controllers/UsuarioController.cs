using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WEstacionaAPI.DbContexto;
using WEstacionaAPI.Dto;
using WEstacionaAPI.Dto.Entidades;
using WEstacionaAPI.Dto.Valores;

namespace WEstacionaAPI.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class UsuarioController : Controller
    {
        private readonly Usuario _usuarioPlei;

        public UsuarioController(Usuario usuarioPlei)
        {
            _usuarioPlei = usuarioPlei;
        }

        [HttpPost("[action]")]
        public async Task<Resposta> Salvar([FromBody] UsuarioDto param) {
            return await _usuarioPlei.Salvar(param);
        }

        public async Task<Resposta> Login([FromBody] UsuarioLoginDto param)
        {
            return await _usuarioPlei.LoginUsuario(param);
        }
    }
}