using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEstacionaAPI.Dto.Entidades;
using WEstacionaAPI.Dto.Enums;
using WEstacionaAPI.Dto.Valores;

namespace WEstacionaAPI.DbContexto
{

    public class Vagas
    {
        private readonly IConfiguration _configuration;
        public Vagas(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public Task<Resposta> Salvar(VagasDto vaga)
        {
            var _retorno = new UsuarioDto();
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
                                vagas
                            (numero_vaga, disponivel, tipo, id_estacionamento, preco_hr)
                            VALUES(@numerovaga, @disponivel, @tipo, @idestacionamento, @precohora);

						";

                        _command.Parameters.Clear();
                        _command.Parameters.AddRange(
                            new[] {
                                new NpgsqlParameter("@numerovaga", vaga.NumeroVaga),
                                new NpgsqlParameter("@disponivel", vaga.Disponivel),
                                new NpgsqlParameter("@tipo", (int)vaga.Tipo),
                                new NpgsqlParameter("@idestacionamento", vaga.IdEstacionamento),
                                new NpgsqlParameter("@precohora", vaga.PrecoHr),
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

        public async Task<Resposta<IEnumerable<VagasDto>>> ObtemPeloIdEstacionamento(string id)
        {
            var _resultado = new List<VagasDto>();
            try
            {
                var _connStr = _configuration.GetConnectionString("DefaultConnection");
                using (var _conn = new NpgsqlConnection(_connStr))
                {
                    await _conn.OpenAsync();
                    using (var _command = _conn.CreateCommand())
                    {
                        _command.CommandText = @"
                            SELECT 
                                id,
                                numero_vaga,
                                disponivel,
                                tipo,
                                id_estacionamento,
                                preco_hr 
                            FROM 
                                vagas
                            WHERE 
                                id_estacionamento = @id
                        ";

                        _command.Parameters.AddWithValue("@id", id);

                        using (var _reader = await _command.ExecuteReaderAsync())
                        {
                            while (await _reader.ReadAsync())
                            {
                                _resultado.Add(
                                    new VagasDto
                                    {
                                        Id = _reader.GetInt32(0),
                                        NumeroVaga = _reader.GetInt32(1),
                                        Disponivel = _reader.GetBoolean(2),
                                        Tipo = (ETipoVaga)_reader.GetInt32(3),
                                        IdEstacionamento = _reader.GetInt32(4),
                                        PrecoHr = _reader.GetDecimal(5)
                                    });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return new Resposta<IEnumerable<VagasDto>> { Objeto = _resultado, Sucesso = false, Mensagem = ex.Message };
            }
            return new Resposta<IEnumerable<VagasDto>>
            {
                Objeto = _resultado,
                Sucesso = true,
                Mensagem = ""
            };
        }

        public async Task<Resposta<VagasDto>> ObtemPeloId(string id)
        {
            var _resultado = new VagasDto();
            try
            {
                var _connStr = _configuration.GetConnectionString("DefaultConnection");
                using (var _conn = new NpgsqlConnection(_connStr))
                {
                    await _conn.OpenAsync();
                    using (var _command = _conn.CreateCommand())
                    {
                        _command.CommandText = @"
                            SELECT 
                                id,
                                numero_vaga,
                                disponivel,
                                tipo,
                                id_estacionamento,
                                preco_hr 
                            FROM 
                                vagas
                            WHERE 
                                id = @id
                        ";

                        _command.Parameters.AddWithValue("@id", id);

                        using (var _reader = await _command.ExecuteReaderAsync())
                        {
                            if (await _reader.ReadAsync())
                            {
                                _resultado = new VagasDto
                                {
                                    Id = _reader.GetInt32(0),
                                    NumeroVaga = _reader.GetInt32(1),
                                    Disponivel = _reader.GetBoolean(2),
                                    Tipo = (ETipoVaga)_reader.GetInt32(3),
                                    IdEstacionamento = _reader.GetInt32(4),
                                    PrecoHr = _reader.GetDecimal(5)
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return new Resposta<VagasDto> { Objeto = _resultado, Sucesso = false, Mensagem = ex.Message };
            }
            return new Resposta<VagasDto>
            {
                Objeto = _resultado,
                Sucesso = true,
                Mensagem = ""
            };
        }
    }
}
