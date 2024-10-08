﻿using Microsoft.AspNetCore.Mvc.RazorPages;
using Npgsql;
using System;
using System.Collections.Generic;
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
        public async Task<Resposta> Salvar(EstacionamentoDto estacionamento)
        {
            var _retorno = new EstacionamentoDto();
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
                                public.estacionamento
                                (
                                    nome,
                                    capacidade_total,
                                    vagas_disponiveis,
                                    id_usuario,
                                    dt_criacao,
                                    rua,
                                    numero,
                                    bairro,
                                    cidade,
                                    estado,
                                    observacao
                                )
                            VALUES
                                (
                                    @nome,
                                    @capacidade_total,
                                    @vagas_disponiveis,
                                    @id_usuario,
                                    @dt_criacao,
                                    @rua,
                                    @numero,
                                    @bairro,
                                    @cidade,
                                    @estado,
                                    @observacao
                                )
                            RETURNING id;
                        ";

                        _command.Parameters.Clear();
                        _command.Parameters.AddRange(
                            new[]
                            {
                                new NpgsqlParameter("@nome", estacionamento.Nome),
                                new NpgsqlParameter("@capacidade_total", estacionamento.CapacidadeTotal),
                                new NpgsqlParameter("@vagas_disponiveis", estacionamento.VagasDisponiveis),
                                new NpgsqlParameter("@id_usuario", estacionamento.IdUsuario),
                                new NpgsqlParameter("@dt_criacao", estacionamento.DtCriacao),
                                new NpgsqlParameter("@rua", estacionamento.Rua),
                                new NpgsqlParameter("@numero", estacionamento.Numero),
                                new NpgsqlParameter("@bairro", estacionamento.Bairro),
                                new NpgsqlParameter("@cidade", estacionamento.Cidade),
                                new NpgsqlParameter("@estado", estacionamento.Estado),
                                new NpgsqlParameter("@observacao", estacionamento.Observacao)
                            }
                        );

                        var newId = (int)await _command.ExecuteScalarAsync();
                        _retorno.Id = newId;
                    }
                }
            }
            catch (Exception ex)
            {
                return new Resposta { Objeto = _retorno, Sucesso = false, Mensagem = ex.Message };
            }

            return new Resposta { Objeto = _retorno, Sucesso = true, Mensagem = "Estacionamento salvo com sucesso." };
        }
        public async Task<Resposta<EstacionamentoDto>> ObterEstacionamentoPorId(int id)
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
                                rua,
                                numero,
                                bairro,
                                cidade,
                                estado,
                                observacao
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
                                    Rua = _reader.GetString(6),
                                    Numero = _reader.GetString(7),
                                    Bairro = _reader.GetString(8),
                                    Cidade = _reader.GetString(9),
                                    Estado = _reader.GetString(10),
                                    Observacao = _reader.GetString(11)
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return new Resposta<EstacionamentoDto>(ex);
            }

            return new Resposta<EstacionamentoDto> { Objeto = estacionamento, Sucesso = estacionamento != null };
        }
        public async Task<Resposta<List<EstacionamentoDto>>> ObterEstacionamentoPorUsuario(int idUsuario)
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
                                rua,
                                numero,
                                bairro,
                                cidade,
                                estado,
                                observacao
                            FROM 
                                public.estacionamento
                            WHERE 
                                id_usuario = @idUsuario;
                        ";

                        _command.Parameters.AddWithValue("@idUsuario", idUsuario);

                        using (var _reader = await _command.ExecuteReaderAsync())
                        {
                            while (await _reader.ReadAsync())
                            {
                                listaEstacionamentos.Add(new EstacionamentoDto
                                {
                                    Id = _reader.GetInt32(0),
                                    Nome = _reader.GetString(1),
                                    CapacidadeTotal = _reader.GetInt32(2),
                                    VagasDisponiveis = _reader.GetInt32(3),
                                    IdUsuario = _reader.GetInt32(4),
                                    DtCriacao = _reader.GetDateTime(5),
                                    Rua = _reader.GetString(6),
                                    Numero = _reader.GetString(7),
                                    Bairro = _reader.GetString(8),
                                    Cidade = _reader.GetString(9),
                                    Estado = _reader.GetString(10),
                                    Observacao = _reader.GetString(11)
                                });
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
                        // Verificar se o estacionamento existe
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
        public async Task<Resposta<Paginacao<EstacionamentoDto>>> BuscaTodosEstacionamentos(int page = 1, int pageSize = 10)
        {
            var resultados = new List<EstacionamentoDto>();

            try
            {
                var _connStr = _configuration.GetConnectionString("DefaultConnection");
                using (var _conn = new NpgsqlConnection(_connStr))
                {
                    await _conn.OpenAsync();

                    // Conta o número total de registros na tabela
                    var totalCountCommand = new NpgsqlCommand("SELECT COUNT(*) FROM public.estacionamento;", _conn);
                    var totalCount = (long)await totalCountCommand.ExecuteScalarAsync();
                    /* Não tire essa linha, sei que parece loucura mas funciona */
                    var totalCountInt = (int)totalCount; /* O retorno está vindo como System int 64, então essa conversão de long para int é necessário para n precisar mudar os padrões para system int 64 tbm */

                    // Calcula o offset para a paginação
                    var offset = (page - 1) * pageSize;

                    var query = @"
                        SELECT 
                            id,
                            nome,
                            capacidade_total,
                            vagas_disponiveis,
                            id_usuario,
                            dt_criacao,
                            rua,
                            numero,
                            bairro,
                            cidade,
                            estado,
                            observacao
                        FROM 
                            public.estacionamento
                        ORDER BY 
                            id
                        LIMIT @pageSize OFFSET @offset;
                    ";

                    using (var command = new NpgsqlCommand(query, _conn))
                    {
                        command.Parameters.AddWithValue("@pageSize", pageSize);
                        command.Parameters.AddWithValue("@offset", offset);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                resultados.Add(new EstacionamentoDto
                                {
                                    Id = reader.GetInt32(0),
                                    Nome = reader.GetString(1),
                                    CapacidadeTotal = reader.GetInt32(2),
                                    VagasDisponiveis = reader.GetInt32(3),
                                    IdUsuario = reader.GetInt32(4),
                                    DtCriacao = reader.GetDateTime(5),
                                    Rua = reader.GetString(6),
                                    Numero = reader.GetString(7),
                                    Bairro = reader.GetString(8),
                                    Cidade = reader.GetString(9),
                                    Estado = reader.GetString(10),
                                    Observacao = reader.GetString(11)
                                });
                            }
                        }
                    }

                    var totalPages = (int)Math.Ceiling(totalCountInt / (double)pageSize);

                    return new Resposta<Paginacao<EstacionamentoDto>>
                    {
                        Objeto = new Paginacao<EstacionamentoDto>
                        {
                            Itens = resultados,
                            PaginaAtual = page,
                            TamanhoPagina = pageSize,
                            TotalItens = totalCountInt,
                            TotalPaginas = totalPages
                        },
                        Sucesso = true
                    };
                }
            }
            catch (Exception ex)
            {
                return new Resposta<Paginacao<EstacionamentoDto>>(ex);
            }
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
                                rua = @rua,
                                numero = @numero,
                                bairro = @bairro,
                                cidade = @cidade,
                                estado = @estado,
                                observacao = @observacao
                            WHERE 
                                id = @id;
                        ";

                        _command.Parameters.AddWithValue("@nome", estacionamento.Nome);
                        _command.Parameters.AddWithValue("@capacidade_total", estacionamento.CapacidadeTotal);
                        _command.Parameters.AddWithValue("@vagas_disponiveis", estacionamento.VagasDisponiveis);
                        _command.Parameters.AddWithValue("@id_usuario", estacionamento.IdUsuario);
                        _command.Parameters.AddWithValue("@dt_criacao", estacionamento.DtCriacao);
                        _command.Parameters.AddWithValue("@rua", estacionamento.Rua);
                        _command.Parameters.AddWithValue("@numero", estacionamento.Numero);
                        _command.Parameters.AddWithValue("@bairro", estacionamento.Bairro);
                        _command.Parameters.AddWithValue("@cidade", estacionamento.Cidade);
                        _command.Parameters.AddWithValue("@estado", estacionamento.Estado);
                        _command.Parameters.AddWithValue("@observacao", estacionamento.Observacao);
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
            return new Resposta { Sucesso = true, Mensagem = "Estacionamento atualizado com sucesso."};
        }
        public async Task<Resposta<List<EstacionamentoDto>>> BuscarPorEndereco(
            string? rua = null,
            string? cidade = null,
            string? estado = null,
            string? observacao = null,
            string? bairro = null)
        {
            var resultados = new List<EstacionamentoDto>();

            try
            {
                var _connStr = _configuration.GetConnectionString("DefaultConnection");
                using (var _conn = new NpgsqlConnection(_connStr))
                {
                    await _conn.OpenAsync();

                    using (var _command = _conn.CreateCommand())
                    {
                        // Construção dinâmica da consulta SQL
                        var query = new StringBuilder(@"
                            SELECT 
                                id,
                                nome,
                                capacidade_total,
                                vagas_disponiveis,
                                id_usuario,
                                dt_criacao,
                                rua,
                                numero,
                                bairro,
                                cidade,
                                estado,
                                observacao
                            FROM 
                                public.estacionamento
                            WHERE 
                        ");

                        var parameters = new List<NpgsqlParameter>();

                        if (!string.IsNullOrEmpty(rua))
                        {
                            query.Append("UPPER(rua) LIKE UPPER(@rua) AND ");
                            parameters.Add(new NpgsqlParameter("@rua", $"%{rua}%"));
                        }
                        if (!string.IsNullOrEmpty(cidade))
                        {
                            query.Append("UPPER(cidade) LIKE UPPER(@cidade) AND ");
                            parameters.Add(new NpgsqlParameter("@cidade", $"%{cidade}%"));
                        }
                        if (!string.IsNullOrEmpty(estado))
                        {
                            query.Append("UPPER(estado) LIKE UPPER(@estado) AND ");
                            parameters.Add(new NpgsqlParameter("@estado", $"%{estado}%"));
                        }
                        if (!string.IsNullOrEmpty(observacao))
                        {
                            query.Append("UPPER(observacao) LIKE UPPER(@observacao) AND ");
                            parameters.Add(new NpgsqlParameter("@observacao", $"%{observacao}%"));
                        }
                        if (!string.IsNullOrEmpty(bairro))
                        {
                            query.Append("UPPER(bairro) LIKE UPPER(@bairro) AND ");
                            parameters.Add(new NpgsqlParameter("@bairro", $"%{bairro}%"));
                        }

                        // Remove o último "AND" se a cláusula WHERE não estiver vazia
                        if (parameters.Count > 0)
                        {
                            query.Length -= 5; // Remove o último " AND "
                        }
                        else
                        {
                            query.Append("1 = 1"); // Caso nenhum parâmetro seja fornecido, sempre retorna verdadeiro
                        }

                        _command.CommandText = query.ToString();
                        _command.Parameters.AddRange(parameters.ToArray());

                        using (var _reader = await _command.ExecuteReaderAsync())
                        {
                            while (await _reader.ReadAsync())
                            {
                                resultados.Add(new EstacionamentoDto
                                {
                                    Id = _reader.GetInt32(0),
                                    Nome = _reader.GetString(1),
                                    CapacidadeTotal = _reader.GetInt32(2),
                                    VagasDisponiveis = _reader.GetInt32(3),
                                    IdUsuario = _reader.GetInt32(4),
                                    DtCriacao = _reader.GetDateTime(5),
                                    Rua = _reader.GetString(6),
                                    Numero = _reader.GetString(7),
                                    Bairro = _reader.GetString(8),
                                    Cidade = _reader.GetString(9),
                                    Estado = _reader.GetString(10),
                                    Observacao = _reader.GetString(11)
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return new Resposta<List<EstacionamentoDto>>(ex);
            }

            return new Resposta<List<EstacionamentoDto>> { Objeto = resultados, Sucesso = true };
        }
    }
}
