using WsEstacinaAPI.Dto.Enums;

namespace WsEstacinaAPI.Dto.Entidades
{
    public class VeiculoDto
    {
        public int Id { get; set; }
        public int IdUsuario { get; set; }
        public string Placa { get; set; } = string.Empty;
        public string Marca { get; set; } = string.Empty;
        public string Modelo { get; set; } = string.Empty;
        public string Cor { get; set; } = string.Empty;
        public ETipoVeiculo Tipo { get; set; }
    }
}