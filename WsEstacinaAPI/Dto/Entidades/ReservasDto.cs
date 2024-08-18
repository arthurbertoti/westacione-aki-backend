using System.Reflection.Metadata.Ecma335;
using WEstacionaAPI.Dto.Enums;

namespace WEstacionaAPI.Dto.Entidades
{
    public class ReservasDto
    {
        public int Id { get; set; }
        public int IdUsuario { get; set; }
        public int IdVaga { get; set; }
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFinal { get; set; }
        public EStatusReserva Status { get; set; }
        public int idVeiculo { get; set; }
        public DateTime? DataCriacao { get; set; }

    }
}
