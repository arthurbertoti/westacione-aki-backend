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
    public class VagasController : ControllerBase
    {
        private readonly Vagas _vagas;

        public VagasController(Vagas usuarioPlei)
        {
            _vagas = usuarioPlei;
        }

        [HttpPost("[action]")]
        public async Task<Resposta> Salvar([FromBody] VagasDto param) {
            return await _vagas.Salvar(param);
        }

        [HttpGet("[action]/{id}")]
        public async Task<Resposta<VagasDto>> ObtemPeloId(string id)
        {
            return await _vagas.ObtemPeloId(id);
        }
        [HttpGet("[action]/{id}")]
        public async Task<Resposta<IEnumerable<VagasDto>>> ObtemPeloIdEstacionamento(string id)
        {
            return await _vagas.ObtemPeloIdEstacionamento(id);
        }
    }
}