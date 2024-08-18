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

    public class Estacionamento
    {
        private readonly IConfiguration _configuration;
        public Estacionamento(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public Task<Resposta> Salvar(EstacionamentoDto estacionamento)
        {
            var _retorno = new EstacionamentoDto();
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
                                public.estacionamento
                                (
                                    nome,
                                    capacidade_total,
                                    vagas_disponiveis,
                                    id_usuario,
                                    dt_criacao,
                                    endereco    
                                )
                            VALUES
                                (
                                    @nome,
                                    @capacidade_total,
                                    @vagas_disponiveis,
                                    @id_usuario,
                                    @dt_criacao,
                                    @endereco
                                );
                        ";

                        _command.Parameters.Clear();
                        _command.Parameters.AddRange(
                            new[] {
                        new NpgsqlParameter("@nome", estacionamento.Nome),
                        new NpgsqlParameter("@capacidade_total", estacionamento.CapacidadeTotal),
                        new NpgsqlParameter("@vagas_disponiveis", estacionamento.VagasDisponiveis),
                        new NpgsqlParameter("@id_usuario", estacionamento.IdUsuario),
                        new NpgsqlParameter("@dt_criacao", estacionamento.DtCriacao),
                        new NpgsqlParameter("@endereco", estacionamento.Endereco)
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
        public async Task<Resposta> ObterEstacionamentoPorId(int id)
        {
            EstacionamentoDto estacionamento = null;
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
                                capacidade_total,
                                vagas_disponiveis,
                                id_usuario,
                                dt_criacao,
                                endereco
                            FROM 
                                public.estacionamento
                            WHERE 
                                id = @id;
                        ";

                        _command.Parameters.AddWithValue("@id", id);

                        using (var _reader = await _command.ExecuteReaderAsync())
                        {
                            if (await _reader.ReadAsync())
                            {
                                estacionamento = new EstacionamentoDto
                                {
                                    Id = _reader.GetInt32(0),
                                    Nome = _reader.GetString(1),
                                    CapacidadeTotal = _reader.GetInt32(2),
                                    VagasDisponiveis = _reader.GetInt32(3),
                                    IdUsuario = _reader.GetInt32(4),
                                    DtCriacao = _reader.GetDateTime(5),
                                    Endereco = _reader.GetString(6)
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
            return new Resposta { Objeto = estacionamento, Sucesso = true };
        }
        public async Task<Resposta<List<EstacionamentoDto>>> ObterEstacionamentoPorUsuario(int id)
        {
            var listaEstacionamentos = new List<EstacionamentoDto>();
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
                                capacidade_total,
                                vagas_disponiveis,
                                id_usuario,
                                dt_criacao,
                                endereco
                            FROM 
                                public.estacionamento
                            WHERE 
                                id_usuario = @id;
                        ";

                        _command.Parameters.AddWithValue("@id", id);

                        using (var _reader = await _command.ExecuteReaderAsync())
                        {
                            while (await _reader.ReadAsync())
                            {
                                var estacionamento = new EstacionamentoDto
                                {
                                    Id = _reader.GetInt32(0),
                                    Nome = _reader.GetString(1),
                                    CapacidadeTotal = _reader.GetInt32(2),
                                    VagasDisponiveis = _reader.GetInt32(3),
                                    IdUsuario = _reader.GetInt32(4),
                                    DtCriacao = _reader.GetDateTime(5),
                                    Endereco = _reader.GetString(6),
                                };
                                listaEstacionamentos.Add(estacionamento);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return new Resposta<List<EstacionamentoDto>>(ex);
            }
            return new Resposta<List<EstacionamentoDto>> { Objeto = listaEstacionamentos, Sucesso = true };
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
                        // Primeiro, verificar se o estacionamento existe
                        _command.CommandText = $@"
                            SELECT 
                                id
                            FROM 
                                public.estacionamento
                            WHERE 
                                id = @id;
                        ";

                        _command.Parameters.AddWithValue("@id", id);

                        using (var _reader = await _command.ExecuteReaderAsync())
                        {
                            if (!await _reader.ReadAsync())
                            {
                                // Estacionamento não encontrado
                                return new Resposta { Sucesso = false, Mensagem = "Estacionamento não encontrado." };
                            }
                        }

                        // Excluir o estacionamento
                        _command.CommandText = $@"
                            DELETE FROM 
                                public.estacionamento
                            WHERE 
                                id = @id;
                        ";

                        var rowsAffected = await _command.ExecuteNonQueryAsync();
                        if (rowsAffected == 0)
                        {
                            return new Resposta { Sucesso = false, Mensagem = "Nenhum estacionamento foi excluído." };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return new Resposta { Sucesso = false, Mensagem = ex.Message };
            }
            return new Resposta { Sucesso = true, Mensagem = "Estacionamento excluído com sucesso." };
        }
        public async Task<Resposta> Atualizar(EstacionamentoDto estacionamento)
        {
            try
            {
                var _connStr = _configuration.GetConnectionString("DefaultConnection");
                using (var _conn = new NpgsqlConnection(_connStr))
                {
                    await _conn.OpenAsync();

                    // Verificar se o estacionamento existe
                    using (var _command = _conn.CreateCommand())
                    {
                        _command.CommandText = $@"
                            SELECT 
                                id
                            FROM 
                                public.estacionamento
                            WHERE 
                                id = @id;
                        ";

                        _command.Parameters.AddWithValue("@id", estacionamento.Id);

                        using (var _reader = await _command.ExecuteReaderAsync())
                        {
                            if (!await _reader.ReadAsync())
                            {
                                // Estacionamento não encontrado
                                return new Resposta { Sucesso = false, Mensagem = "Estacionamento não encontrado." };
                            }
                        }
                    }

                    // Atualizar o estacionamento
                    using (var _command = _conn.CreateCommand())
                    {
                        _command.CommandText = $@"
                            UPDATE 
                                public.estacionamento
                            SET 
                                nome = @nome,
                                capacidade_total = @capacidade_total,
                                vagas_disponiveis = @vagas_disponiveis,
                                id_usuario = @id_usuario,
                                dt_criacao = @dt_criacao,
                                endereco = @endereco
                            WHERE 
                                id = @id;
                         ";

                        _command.Parameters.AddWithValue("@nome", estacionamento.Nome);
                        _command.Parameters.AddWithValue("@capacidade_total", estacionamento.CapacidadeTotal);
                        _command.Parameters.AddWithValue("@vagas_disponiveis", estacionamento.VagasDisponiveis);
                        _command.Parameters.AddWithValue("@id_usuario", estacionamento.IdUsuario);
                        _command.Parameters.AddWithValue("@dt_criacao", estacionamento.DtCriacao);
                        _command.Parameters.AddWithValue("@endereco", estacionamento.Endereco);
                        _command.Parameters.AddWithValue("@id", estacionamento.Id);

                        var rowsAffected = await _command.ExecuteNonQueryAsync();
                        if (rowsAffected == 0)
                        {
                            return new Resposta { Sucesso = false, Mensagem = "Nenhum estacionamento foi atualizado." };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return new Resposta { Sucesso = false, Mensagem = ex.Message };
            }

            return new Resposta { Sucesso = true, Mensagem = "Estacionamento atualizado com sucesso." };
        }

    }
}
