using WEstacionaAPI.Dto.Enums;

namespace WEstacionaAPI.Dto.Entidades
{
    public class Vagas
    {
        public int Id { get; set; }
        public int IdEstacionamento { get; set; }
        public int NumeroVaga { get; set; }
        public ETipoVaga Tipo { get; set; }
        public bool Coberto { get; set; }
    }
}