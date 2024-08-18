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

    public class Reservas
    {
        private readonly IConfiguration _configuration;
        public Reservas(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public Task<Resposta> Salvar(ReservasDto reservas)
        {
            var _retorno = new ReservasDto();
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
                                public.reservas
                                (
                                    id_usuario,
                                    id_vaga,
                                    dt_inicio,
                                    dt_final,
                                    status,
                                    id_veiculo,
                                    dt_criacao
                                )
                            VALUES
                                (
                                    @id_usuario,
                                    @id_vaga,
                                    @dt_inicio,
                                    @dt_final,
                                    @status,
                                    @id_veiculo,
                                    @dt_criacao
                                );
                        ";

                        _command.Parameters.Clear();
                        _command.Parameters.AddRange(
                            new[] {
                        new NpgsqlParameter("@id_usuario", reservas.IdUsuario),
                        new NpgsqlParameter("@id_vaga", reservas.IdVaga),
                        new NpgsqlParameter("@dt_inicio", reservas.DataInicio),
                        new NpgsqlParameter("@dt_final", reservas.DataFinal),
                        new NpgsqlParameter("@status", reservas.Status),
                        new NpgsqlParameter("@id_veiculo", reservas.idVeiculo),
                        new NpgsqlParameter("@dt_criacao", reservas.DataCriacao),
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

    }
}
