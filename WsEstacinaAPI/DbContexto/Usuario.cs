using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEstacionaAPI.Dto.Entidades;
using WEstacionaAPI.Dto.Valores;

namespace DBconnection
{
    public interface IUser
    {
        Task<Resposta> Salvar(UsuarioDto usuarioCadastro);
    }

    public class Usuario : IUser
    {
        private readonly IConfiguration _configuration;
        public Usuario(IConfiguration configuration) { 
            _configuration = configuration;
        }   
        public Task<Resposta> Salvar(UsuarioDto usuarioCadastro)
        {

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
                                public.usuario
                            (nome, login, senha, telefone, tipo_usuario, dt_criacao)
                            VALUES(@nome, @login, @senha, @telefone, @tipousuario, @datacriacao);
						";

                        _command.Parameters.Clear();
                        _command.Parameters.AddRange(
                            new[] {
                                new NpgsqlParameter("@nome", usuarioCadastro.Nome),
                                new NpgsqlParameter("@login", usuarioCadastro.Login),
                                new NpgsqlParameter("@senha", usuarioCadastro.Senha),
                                new NpgsqlParameter("@telefone", usuarioCadastro.Telefone),
                                new NpgsqlParameter("@tipousuario", usuarioCadastro.TipoUsuario),
                                new NpgsqlParameter("@datacriacao", usuarioCadastro.DtCriacao),
                            }
                        );

                        using (var _reader = _command.ExecuteReader())
                        {
                            while (_reader.Read())
                                _retorno.Add(_reader.ObtemDado<ObtemDado>());
                        }
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
