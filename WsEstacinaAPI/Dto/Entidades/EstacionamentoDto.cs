namespace WEstacionaAPI.Dto.Entidades
{
    public class EstacionamentoDto
    {
        public int Id { get; set; }
        public int IdUsuario { get; set; }
        public string Nome { get; set; } = string.Empty;
        public int CapacidadeTotal { get; set; }
        public int VagasDisponiveis { get; set; }
        public DateTime? DtCriacao { get; set; }
        public string Rua { get; set; } = string.Empty;
        public string Cidade { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public string Numero { get; set; } = string.Empty;
        public string Observacao { get; set; } = string.Empty;
        public string Bairro { get; set; } = string.Empty;
        
    }

    public class EstaciomanentoEnderecoDto
    {
        public string Rua { get; set; } = string.Empty;
        public string Cidade { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public string Numero { get; set; } = string.Empty;
        public string Observacao { get; set; } = string.Empty;
        public string Bairro { get; set; } = string.Empty;
    }
}