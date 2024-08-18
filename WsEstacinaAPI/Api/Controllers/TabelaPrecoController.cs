using Microsoft.AspNetCore.Mvc;
using WEstacionaAPI.DbContexto;
using WEstacionaAPI.Dto.Entidades;
using WEstacionaAPI.Dto.Valores;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace WEstacionaAPI.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class TabelaPrecoController : ControllerBase
    {
        private readonly TabelaPreco _tabelaPreco;

        public TabelaPrecoController(IConfiguration configuration)
        {
            _tabelaPreco = new TabelaPreco(configuration);
        }

        // POST: api/TabelaPreco/Criar
        [HttpPost("[action]")]
        public async Task<IActionResult> Criar([FromBody] TabelaPrecoDto tabelaPrecoDto)
        {
            if (string.IsNullOrWhiteSpace(tabelaPrecoDto.Nome))
            {
                return BadRequest(new Resposta { Sucesso = false, Mensagem = "O campo Nome é obrigatório e não pode ser vazio." });
            }

            if (tabelaPrecoDto.IdEstacionamento <= 0)
            {
                return BadRequest(new Resposta { Sucesso = false, Mensagem = "O campo ID do Estabelecimento deve ser maior que zero." });
            }

            if (tabelaPrecoDto.IdUsuario <= 0)
            {
                return BadRequest(new Resposta { Sucesso = false, Mensagem = "O campo ID do Usuário deve ser maior que zero." });
            }

            if (tabelaPrecoDto.PrecoHr <= 0)
            {
                return BadRequest(new Resposta { Sucesso = false, Mensagem = "O campo Preço por Hora deve ser maior que zero." });
            }

            var resposta = await _tabelaPreco.Salvar(tabelaPrecoDto);

            if (!resposta.Sucesso)
            {
                return StatusCode(500, resposta.Mensagem);
            }

            return CreatedAtAction(nameof(ObterPeloId), new { id = tabelaPrecoDto.Id }, tabelaPrecoDto);
        }

        // GET: api/TabelaPreco/ObterPorId
        [HttpGet("[action]")]
        public async Task<IActionResult> ObterPeloId(int id)
        {
            if (id == 0)
            {
                return BadRequest(new Resposta { Sucesso = false, Mensagem = "Id está zerado!" });
            }

            var resposta = await _tabelaPreco.ObterPorId(id);

            if (!resposta.Sucesso)
            {
                return NotFound(resposta);
            }

            return Ok(resposta);
        }

        // GET: api/TabelaPreco/ObterPorEstabelecimento
        [HttpGet("[action]")]
        public async Task<IActionResult> ObterPorEstacionamento(int idEstabelecimento)
        {
            if (idEstabelecimento == 0)
            {
                return BadRequest(new Resposta { Sucesso = false, Mensagem = "Id do Estabelecimento está zerado!" });
            }

            var resposta = await _tabelaPreco.ObterPorEstacionamento(idEstabelecimento);

            if (!resposta.Sucesso)
            {
                return NotFound(resposta);
            }

            return Ok(resposta);
        }

        // DELETE: api/TabelaPreco/Deletar
        [HttpDelete("[action]")]
        public async Task<IActionResult> Deletar(int id)
        {
            if (id == 0)
            {
                return BadRequest(new Resposta { Sucesso = false, Mensagem = "Id está zerado!" });
            }

            var resposta = await _tabelaPreco.Deletar(id);

            if (!resposta.Sucesso)
            {
                return StatusCode(500, resposta.Mensagem);
            }

            return NoContent();
        }

        // UPDATE: api/TabelaPreco/Atualizar
        [HttpPost("[action]")]
        public async Task<IActionResult> Atualizar([FromBody] TabelaPrecoDto tabelaPrecoDto)
        {
            if (tabelaPrecoDto.Id == 0)
            {
                return BadRequest(new Resposta { Sucesso = false, Mensagem = "Para atualizar registro deve informar id diferente de 0" });
            }

            var resposta = await _tabelaPreco.Atualizar(tabelaPrecoDto);

            if (!resposta.Sucesso)
            {
                return StatusCode(500, resposta.Mensagem);
            }

            return NoContent();
        }
    }
}
