using WEstacionaAPI.Dto.Enums;

namespace WEstacionaAPI.Dto.Entidades
{
    public class TabelaPrecoDto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public ETipoVeiculo TipoVeiculo { get; set; }
        public int IdEstacionamento { get; set; }
        public int IdUsuario { get; set; }
        public decimal PrecoHr { get; set; }
    }
}
