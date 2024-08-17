namespace WsEstacinaAPI.Dto.Entidades
{
    public class EstacionamentoDto
    {
        public int Id { get; set; }
        public int IdUsuario { get; set; }
        public string Nome { get; set; } = string.Empty;
        public int CapacidadeTotal { get; set; }
        public int VagasDisponiveis { get; set; }
        public DateTime DtCriacao { get; set; }
    }
}