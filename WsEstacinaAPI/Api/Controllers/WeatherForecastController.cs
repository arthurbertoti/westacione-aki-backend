using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WEstacionaAPI.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class EspImportacaoEDIController : Controller
    {

        public EspImportacaoEDIController(EspImportacaoEDINegocio negocio)
        {
            _negocio = negocio;
        }
        [Authorize, HttpPost("[action]")]
        public async Task<IEnumerable<EIEDIConsultaDto>> Obtem([FromBody] EIEDIConsultaParamDto param) => await _negocio.Obtem(param.ConfOrigemID);

        [Authorize, HttpPost("[action]")]
        public async Task<Resposta> Importar([FromBody] EIEDIParamDto param) => await _negocio.Importar(param.ConfOrigemID, param.Leiaute, param.ConteudoEDI, param.NomeArquivo);
    }
}