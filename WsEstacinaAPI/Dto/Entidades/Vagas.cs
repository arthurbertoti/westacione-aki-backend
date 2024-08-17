using WsEstacinaAPI.Dto.Enums;

namespace WsEstacinaAPI.Dto.Entidades
{
    public class Vagas
    {
        public int Id { get; set; }
        public int IdEstacionamento { get; set; } = string.Empty;
        public int NumeroVaga { get; set; }
        public ETipoVaga Tipo { get; set; }
        public bool Coberto { get; set; }
    }
}