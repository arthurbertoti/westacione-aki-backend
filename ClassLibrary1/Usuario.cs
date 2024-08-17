using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBconnection
{
    public interface IUser
    {
        Task<Resposta> Cadastro(usuarioDto UsuarioCadastro);
    }

    public class Usuario : IUser
    {
        public Task<Resposta> Cadastro(usuarioDto U)
        {
            var _retorno = new List<EEVolumeDto>();
            try
            {
                var _connStr = origem.ToConnectionString();
                using (var _conn = new NpgsqlConnection(_connStr))
                {
                    _conn.Open();
                    using (var _command = _conn.CreateCommand())
                    {
                        _command.CommandText = $@"
						    
						";

                        _command.Parameters.Clear();
                        _command.Parameters.AddRange(
                            new[] {
                                new NpgsqlParameter("@codbarras", codBarras),
                                new NpgsqlParameter("@tipo", tipo)
                            }
                        );

                        using (var _reader = _command.ExecuteReader())
                        {
                            while (_reader.Read())
                                _retorno.Add(_reader.ObtemDado<EEVolumeDto>());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return Task.FromResult(new Resposta<IEnumerable<EEVolumeDto>> { Objeto = _retorno, Sucesso = false, Mensagem = ex.Message });
            }
            return Task.FromResult(new Resposta<IEnumerable<EEVolumeDto>> { Objeto = _retorno, Sucesso = true });
        }
    }
}
