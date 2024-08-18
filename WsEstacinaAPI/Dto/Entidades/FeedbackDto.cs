
using System;

namespace WEstacionaAPI.Dto.Entidades
{   
    public class FeedbackDto
    {
        public int Id { get; set; }
        public string Comentario { get; set; } =  string.Empty;
        public DateTime? DataEnvio { get; set; }
        public int IdUsuario { get; set; }
        public int IdEstabelecimento { get; set; }
    }
}
