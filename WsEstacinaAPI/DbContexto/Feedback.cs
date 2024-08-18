using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEstacionaAPI.Dto.Entidades;
using WEstacionaAPI.Dto.Valores;

namespace WEstacionaAPI.DbContexto
{

    public class Feedback
    {
        private readonly IConfiguration _configuration;
        public Feedback(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public Task<Resposta> Salvar(FeedbackDto feedback)
        {
            var _retorno = new FeedbackDto();
            try
            {
                var _connStr = _configuration.GetConnectionString("DefaultConnection");
                using (var _conn = new NpgsqlConnection(_connStr))
                {
                    _conn.Open();
                    using (var _command = _conn.CreateCommand())
                    {
                        _command.CommandText = $@"
                            INSERT INTO 
                                public.feedback
                                (
                                    comentario,
                                    dt_envio,
                                    id_usuario,
                                    id_estacionamento
                                )
                            VALUES
                                (
                                    @comentario,
                                    @dt_envio,
                                    @id_usuario,
                                    @id_estacionamento
                                );
                        ";

                        _command.Parameters.Clear();
                        _command.Parameters.AddRange(
                            new[] {
                        new NpgsqlParameter("@comentario", feedback.Comentario),
                        new NpgsqlParameter("@dt_envio", feedback.DataEnvio),
                        new NpgsqlParameter("@id_usuario", feedback.IdUsuario),
                        new NpgsqlParameter("@id_estacionamento", feedback.IdEstacionamento),
                            }
                        );

                        var _reader = _command.ExecuteReader();
                    }
                }
            }
            catch (Exception ex)
            {
                return Task.FromResult(new Resposta { Objeto = _retorno, Sucesso = false, Mensagem = ex.Message });
            }
            return Task.FromResult(new Resposta { Objeto = _retorno, Sucesso = true });
        }

        public async Task<Resposta> ObterFeedbackPorId(int id)
        {
            FeedbackDto feedback = null;
            try
            {
                var _connStr = _configuration.GetConnectionString("DefaultConnection");
                using (var _conn = new NpgsqlConnection(_connStr))
                {
                    await _conn.OpenAsync();
                    using (var _command = _conn.CreateCommand())
                    {
                        _command.CommandText = $@"
                            SELECT 
                                id,
                                comentario,
                                dt_envio,
                                id_usuario,
                                id_estacionamento
                            FROM 
                                public.feedback
                            WHERE 
                                id = @id;
                        ";

                        _command.Parameters.AddWithValue("@id", id);

                        using (var _reader = await _command.ExecuteReaderAsync())
                        {
                            if (await _reader.ReadAsync())
                            {
                                feedback = new FeedbackDto
                                {
                                    Id = _reader.GetInt32(0),
                                    Comentario = _reader.GetString(1),
                                    DataEnvio = _reader.GetDateTime(2),
                                    IdUsuario = _reader.GetInt32(3),
                                    IdEstacionamento = _reader.GetInt32(4)
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return new Resposta { Sucesso = false, Mensagem = ex.Message };
            }
            return new Resposta { Objeto = feedback, Sucesso = true };
        }

    }
}
