namespace WEstacionaAPI.Dto.Valores
{
    public class Resposta
    {
        public string? Mensagem { get; set; }
        public bool Sucesso { get; set; }
        public object? Objeto { get; set; }
        public string? Token { get; set; }

        public Resposta()
        {
            Sucesso = true;
        }

        public Resposta(Exception ex)
        {
            Sucesso = false;
            Mensagem = ex.Message;
        }
    }

    public class Resposta<T>
    {
        public string? Mensagem { get; set; }
        public bool Sucesso { get; set; }
        public T? Objeto { get; set; }
        public string? Token { get; set; }

        public Resposta()
        {
            Sucesso = true;
        }

        public Resposta(Exception ex)
        {
            Sucesso = false;
            Mensagem = ex.Message;
        }
    }
}
