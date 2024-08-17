using WsEstacinaAPI.Dto.Enums;

namespace WsEstacinaAPI.Dto.Entidades
{
    public class TabelaPreco
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public ETipoVeiculo TipoVeiculo { get; set; }
        public int IdEstabelecimento { get; set; }
        public int IdUsuario { get; set; }
        public decimal PrecoHr { get; set; }
    }
}
