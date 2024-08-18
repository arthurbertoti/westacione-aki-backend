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
    public class VeiculoController : ControllerBase
    {
        private readonly Veiculo _veiculo;

        public VeiculoController(Veiculo veiculo)
        {
            _veiculo = veiculo;
        }

        [HttpPost("[action]")]
        public async Task<Resposta> Salvar([FromBody] VeiculoDto param) {
            return await _veiculo.Salvar(param);
        }

        [HttpGet("[action]/{id}")]
        public async Task<Resposta<VeiculoDto>> ObtemPeloId(string id)
        {
            return await _veiculo.ObtemPeloId(id);
        }
    }
}