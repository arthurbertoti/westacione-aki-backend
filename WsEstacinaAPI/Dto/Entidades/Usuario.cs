using WsEstacinaAPI.Dto.Enums;

namespace WsEstacinaAPI.Dto.Entidades
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Login { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;
        public ETipoUsuario TipoUsuario { get; set; }
        public DateTime? DtCriacao { get; set; }
    }
}