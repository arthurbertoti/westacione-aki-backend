using Microsoft.AspNetCore.Mvc;
using WEstacionaAPI.DbContexto;
using WEstacionaAPI.Dto.Entidades;
using WEstacionaAPI.Dto.Valores;
using System.Threading.Tasks;

namespace WEstacionaAPI.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class EstacionamentoController : ControllerBase
    {
        private readonly Estacionamento _estacionamento;
        public EstacionamentoController(IConfiguration configuration)
        {
            _estacionamento = new Estacionamento(configuration);
        }
        // POST: api/Estacionamento/Criar
        [HttpPost("[action]")]
        public async Task<IActionResult> Criar([FromBody] EstacionamentoDto estacionamento)
        {
            if (string.IsNullOrWhiteSpace(estacionamento.Nome))
            {
                return BadRequest(new Resposta { Sucesso = false, Mensagem = "O campo Nome é obrigatório e não pode ser vazio." });
            }

            if (estacionamento.CapacidadeTotal <= 0)
            {
                return BadRequest(new Resposta { Sucesso = false, Mensagem = "O campo Capacidade Total deve ser maior que zero." });
            }

            if (estacionamento.VagasDisponiveis < 0)
            {
                return BadRequest(new Resposta { Sucesso = false, Mensagem = "O campo Vagas Disponíveis não pode ser negativo." });
            }

            if (estacionamento.IdUsuario <= 0)
            {
                return BadRequest(new Resposta { Sucesso = false, Mensagem = "O campo ID do Usuário deve ser maior que zero." });
            }

            if (estacionamento.DtCriacao == null)
            {
                return BadRequest(new Resposta { Sucesso = false, Mensagem = "O campo Data de Criação é obrigatório e não pode ser nulo." });
            }


            var resposta = await _estacionamento.Salvar(estacionamento);

            if (!resposta.Sucesso)
            {
                return StatusCode(500, resposta.Mensagem);
            }

            return CreatedAtAction(nameof(ObterPeloId), new { id = estacionamento.Id }, estacionamento);
        }
        // GET: api/Estacionamento/ObterPorUsuario
        [HttpGet("[action]")]
        public async Task<IActionResult> ObterPorUsuario(int id)
        {
            if (id == 0)
            {
                return BadRequest(new Resposta { Sucesso = false, Mensagem = "Id está zerado!" });
            }
            var resposta = await _estacionamento.ObterEstacionamentoPorUsuario(id);

            if (!resposta.Sucesso)
            {
                return NotFound(resposta.Mensagem);
            }

            return Ok(resposta);
        }
        [HttpGet("[action]")]
        public async Task<IActionResult> ObterPeloId(int id)
        {
            if (id == 0)
            {
                return BadRequest(new Resposta { Sucesso = false, Mensagem = "Id está zerado!" });
            }
            var resposta = await _estacionamento.ObterEstacionamentoPorId(id);

            if (!resposta.Sucesso)
            {
                return NotFound(resposta);
            }

            return Ok(resposta);
        }
        // DELETE: api/Estacionamento/ObterPeloId
        [HttpDelete("[action]")]
        public async Task<IActionResult> Delete(int id)
        {
            var resposta = await _estacionamento.Deletar(id);

            if (!resposta.Sucesso)
            {
                return StatusCode(500, resposta.Mensagem);
            }

            return NoContent();
        }
        // UPDATE: api/Estaciomaneto/Atualizar
        [HttpPost("[action]")]
        public async Task<IActionResult> Atualizar(EstacionamentoDto estacionamento)
        {
            if (estacionamento.Id == 0)
            {
                return BadRequest(new Resposta { Sucesso = false, Mensagem = "Para atualizar registro deve informa id diferente de 0" });
            }
            var resposta = await _estacionamento.Atualizar(estacionamento);

            if (!resposta.Sucesso)
            {
                return StatusCode(500, resposta.Mensagem);
            }

            return NoContent();
        }
    }
}
