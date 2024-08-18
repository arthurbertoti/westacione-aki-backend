using Npgsql;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WEstacionaAPI.Dto.Entidades;
using WEstacionaAPI.Dto.Enums;
using WEstacionaAPI.Dto.Valores;

namespace WEstacionaAPI.DbContexto
{
    public class TabelaPreco
    {
        private readonly IConfiguration _configuration;

        public TabelaPreco(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<Resposta> Salvar(TabelaPrecoDto tabelaPrecoDto)
        {
            try
            {
                var _connStr = _configuration.GetConnectionString("DefaultConnection");
                using (var _conn = new NpgsqlConnection(_connStr))
                {
                    await _conn.OpenAsync();
                    using (var _command = _conn.CreateCommand())
                    {
                        _command.CommandText = $@"
                            INSERT INTO 
                                public.tabela_preco
                                (
                                    nome,
                                    tipo_veiculo,
                                    id_estacionamento,
                                    id_usuario,
                                    preco_hr
                                )
                            VALUES
                                (
                                    @nome,
                                    @tipo_veiculo,
                                    @id_estacionamento,
                                    @id_usuario,
                                    @preco_hr
                                )
                            RETURNING id;
                        ";

                        _command.Parameters.AddWithValue("@nome", tabelaPrecoDto.Nome);
                        _command.Parameters.AddWithValue("@tipo_veiculo", (int)tabelaPrecoDto.TipoVeiculo);
                        _command.Parameters.AddWithValue("@id_estacionamento", tabelaPrecoDto.IdEstacionamento);
                        _command.Parameters.AddWithValue("@id_usuario", tabelaPrecoDto.IdUsuario);
                        _command.Parameters.AddWithValue("@preco_hr", tabelaPrecoDto.PrecoHr);

                        var newId = (int)await _command.ExecuteScalarAsync();
                        return new Resposta { Sucesso = true, Objeto = new { Id = newId } };
                    }
                }
            }
            catch (Exception ex)
            {
                return new Resposta(ex);
            }
        }

        public async Task<Resposta<TabelaPrecoDto>> ObterPorId(int id)
        {
            TabelaPrecoDto tabelaPreco = null;
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
                                nome,
                                tipo_veiculo,
                                id_estacionamento,
                                id_usuario,
                                preco_hr
                            FROM 
                                public.tabela_preco
                            WHERE 
                                id = @id;
                        ";

                        _command.Parameters.AddWithValue("@id", id);

                        using (var _reader = await _command.ExecuteReaderAsync())
                        {
                            if (await _reader.ReadAsync())
                            {
                                tabelaPreco = new TabelaPrecoDto
                                {
                                    Id = _reader.GetInt32(0),
                                    Nome = _reader.GetString(1),
                                    TipoVeiculo = (ETipoVeiculo)_reader.GetInt32(2),
                                    IdEstacionamento = _reader.GetInt32(3),
                                    IdUsuario = _reader.GetInt32(4),
                                    PrecoHr = _reader.GetDecimal(5)
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return new Resposta<TabelaPrecoDto>(ex);
            }
            return new Resposta<TabelaPrecoDto> { Objeto = tabelaPreco, Sucesso = tabelaPreco != null };
        }

        public async Task<Resposta<List<TabelaPrecoDto>>> ObterPorEstacionamento(int idEstacionamento)
        {
            var tabelaPrecos = new List<TabelaPrecoDto>();
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
                                nome,
                                tipo_veiculo,
                                id_estacionamento,
                                id_usuario,
                                preco_hr
                            FROM 
                                public.tabela_preco
                            WHERE 
                                id_estacionamento = @idEstacionamento;
                        ";

                        _command.Parameters.AddWithValue("@idEstacionamento", idEstacionamento);

                        using (var _reader = await _command.ExecuteReaderAsync())
                        {
                            while (await _reader.ReadAsync())
                            {
                                tabelaPrecos.Add(new TabelaPrecoDto
                                {
                                    Id = _reader.GetInt32(0),
                                    Nome = _reader.GetString(1),
                                    TipoVeiculo = (ETipoVeiculo)_reader.GetInt32(2),
                                    IdEstacionamento = _reader.GetInt32(3),
                                    IdUsuario = _reader.GetInt32(4),
                                    PrecoHr = _reader.GetDecimal(5)
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return new Resposta<List<TabelaPrecoDto>>(ex);
            }
            return new Resposta<List<TabelaPrecoDto>> { Objeto = tabelaPrecos, Sucesso = tabelaPrecos.Count > 0 };
        }

        public async Task<Resposta> Deletar(int id)
        {
            try
            {
                var _connStr = _configuration.GetConnectionString("DefaultConnection");
                using (var _conn = new NpgsqlConnection(_connStr))
                {
                    await _conn.OpenAsync();
                    using (var _command = _conn.CreateCommand())
                    {
                        // Verificar se o registro existe
                        _command.CommandText = $@"
                            SELECT 
                                id
                            FROM 
                                public.tabela_preco
                            WHERE 
                                id = @id;
                        ";

                        _command.Parameters.AddWithValue("@id", id);

                        using (var _reader = await _command.ExecuteReaderAsync())
                        {
                            if (!await _reader.ReadAsync())
                            {
                                return new Resposta { Sucesso = false, Mensagem = "Tabela de Preços não encontrada." };
                            }
                        }

                        // Deletar o registro
                        _command.CommandText = $@"
                            DELETE FROM 
                                public.tabela_preco
                            WHERE 
                                id = @id;
                        ";

                        var rowsAffected = await _command.ExecuteNonQueryAsync();
                        if (rowsAffected == 0)
                        {
                            return new Resposta { Sucesso = false, Mensagem = "Nenhum registro foi deletado." };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return new Resposta(ex);
            }
            return new Resposta { Sucesso = true, Mensagem = "Tabela de Preços excluída com sucesso." };
        }

        public async Task<Resposta> Atualizar(TabelaPrecoDto tabelaPrecoDto)
        {
            try
            {
                var _connStr = _configuration.GetConnectionString("DefaultConnection");
                using (var _conn = new NpgsqlConnection(_connStr))
                {
                    await _conn.OpenAsync();

                    // Verificar se o registro existe
                    using (var _command = _conn.CreateCommand())
                    {
                        _command.CommandText = $@"
                            SELECT 
                                id
                            FROM 
                                public.tabela_preco
                            WHERE 
                                id = @id;
                        ";

                        _command.Parameters.AddWithValue("@id", tabelaPrecoDto.Id);

                        using (var _reader = await _command.ExecuteReaderAsync())
                        {
                            if (!await _reader.ReadAsync())
                            {
                                return new Resposta { Sucesso = false, Mensagem = "Tabela de Preços não encontrada." };
                            }
                        }

                        // Atualizar o registro
                        _command.CommandText = $@"
                            UPDATE 
                                public.tabela_preco
                            SET 
                                nome = @nome,
                                tipo_veiculo = @tipo_veiculo,
                                id_estacionamento = @id_estacionamento,
                                id_usuario = @id_usuario,
                                preco_hr = @preco_hr
                            WHERE 
                                id = @id;
                        ";

                        _command.Parameters.AddWithValue("@nome", tabelaPrecoDto.Nome);
                        _command.Parameters.AddWithValue("@tipo_veiculo", (int)tabelaPrecoDto.TipoVeiculo);
                        _command.Parameters.AddWithValue("@id_estacionamento", tabelaPrecoDto.IdEstacionamento);
                        _command.Parameters.AddWithValue("@id_usuario", tabelaPrecoDto.IdUsuario);
                        _command.Parameters.AddWithValue("@preco_hr", tabelaPrecoDto.PrecoHr);
                        _command.Parameters.AddWithValue("@id", tabelaPrecoDto.Id);

                        var rowsAffected = await _command.ExecuteNonQueryAsync();
                        if (rowsAffected == 0)
                        {
                            return new Resposta { Sucesso = false, Mensagem = "Nenhum registro foi atualizado." };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return new Resposta(ex);
            }
            return new Resposta { Sucesso = true, Mensagem = "Tabela de Preços atualizada com sucesso." };
        }
    }
}
