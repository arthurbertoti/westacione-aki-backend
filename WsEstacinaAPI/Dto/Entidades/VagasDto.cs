using WsEstacinaAPI.Dto.Enums;

namespace WsEstacinaAPI.Dto.Entidades
{
    public class VagasDto
    {
        public int Id { get; set; }
        public int IdEstacionamento { get; set; }
        public bool Disponivel { get; set; }
        public int NumeroVaga { get; set; }
        public ETipoVaga Tipo { get; set; }
        public decimal PrecoHr { get; set; }
    }
}