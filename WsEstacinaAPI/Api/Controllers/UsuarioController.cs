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
        public async Task<ActionResult> LoginUsuario([FromBody]UserLogin userLogin)
        {
            return Ok( await _usuarioPlei.LoginUsuario(userLogin.Usuario, userLogin.Senha));
        }

        [HttpPost("[action]")]
        public async Task<Resposta> Salvar([FromBody] UsuarioDto param) {
            return await _usuarioPlei.Salvar(param);
        }
    }
    public class UserLogin
    {
        public string Usuario { get; set; }
        public string Senha { get; set; }

    }
}