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

    public class Veiculo
    {
        private readonly IConfiguration _configuration;
        public Veiculo(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public Task<Resposta> Salvar(VeiculoDto param)
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
                            INSERT INTO veiculo
                            (placa, marca, modelo, cor, id_usuario)
                            VALUES(@placa, @marca, @modelo, @cor, @idusuario);
						";

                        _command.Parameters.Clear();
                        _command.Parameters.AddRange(
                            new[] {
                                new NpgsqlParameter("@placa", param.Placa),
                                new NpgsqlParameter("@marca", param.Marca),
                                new NpgsqlParameter("@modelo", param.Modelo),
                                new NpgsqlParameter("@idestacionamento", param.Cor),
                                new NpgsqlParameter("@precohora", param.IdUsuario),
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

        public async Task<Resposta<VeiculoDto>> ObtemPeloId(string id)
        {
            var _resultado = new VeiculoDto();
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
                                placa,
                                marca,
                                modelo,
                                cor,
                                id_usuario
                            FROM 
                                public.veiculo;
                        ";

                        _command.Parameters.AddWithValue("@id", id);

                        using (var _reader = await _command.ExecuteReaderAsync())
                        {
                            if (await _reader.ReadAsync())
                            {
                                _resultado = new VeiculoDto
                                {
                                    Id = _reader.GetInt32(0),
                                    Placa = _reader.GetString(1),
                                    Marca = _reader.GetString(2),
                                    Modelo = _reader.GetString(3),
                                    Cor = _reader.GetString(4),
                                    IdUsuario= _reader.GetInt32(5)
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return new Resposta<VeiculoDto> { Objeto = _resultado, Sucesso = false, Mensagem = ex.Message };
            }
            return new Resposta<VeiculoDto>
            {
                Objeto = _resultado,
                Sucesso = true,
                Mensagem = ""
            };
        }
    }
}
