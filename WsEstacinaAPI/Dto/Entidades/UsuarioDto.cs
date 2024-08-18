using WEstacionaAPI.Dto.Enums;

namespace WEstacionaAPI.Dto.Entidades
{
    public class UsuarioDto
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