using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using System.Data;
using System.Configuration;
using ControleEstoque.web.Helpers;
using System.ComponentModel.DataAnnotations;

namespace ControleEstoque.web.Models
{
    public class UsuarioModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Login deve ser informado.")]
        public string Login { get; set; }
        [Required(ErrorMessage = "Senha deve ser informada.")]
        public string Senha { get; set; }
        [Required(ErrorMessage = "Nome deve ser informado.")]
        public string Nome { get; set; }
        public string Email { get; set; }
        public bool Ativo { get; set; }
        public List<PerfilModel> Perfis { get; set; }

        public UsuarioModel()
        {
            this.Perfis = new List<PerfilModel>();
        }

        public static int RecuperarQuantidadeReg()
        {
            var ret = 0;
            using (var conexao = new MySqlConnection())
            {
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                conexao.Open();
                using (var comando = new MySqlCommand())
                {
                    comando.Connection = conexao;
                    comando.CommandText = "select count(*) from tb_usuario";
                    ret = Convert.ToInt32(comando.ExecuteScalar());
                }
            }

            return ret;
        }

        public static List<UsuarioModel> RecuperarLista(int pagina = 0, int tamPagina = 0, string filtro = "")
        {
            var ret = new List<UsuarioModel>();
            using (var conexao = new MySqlConnection())
            {
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                conexao.Open();
                using (var comando = new MySqlCommand())
                {
                    var pos = (pagina - 1) * tamPagina;
                    var filtroWhere = "";
                    if (!string.IsNullOrEmpty(filtro))
                    {
                        filtroWhere = string.Format(" where lower(us.nome) like '%{0}%' ", filtro.ToLower());
                    }

                    comando.Connection = conexao;
                    comando.CommandText = string.Format("select " +
                        "us.id_usuario, " +
                        "us.usuario, " +
                        "us.senha, " +
                        "us.nome, " +
                        "us.email, " +
                        "us.status " +
                        "from tb_usuario us " +
                        filtroWhere +
                        "order by nome limit {0}, {1}",
                        pos > 0 ? pos : 0, tamPagina);
                    MySqlDataReader dtreader = comando.ExecuteReader();

                    while (dtreader.Read())
                    {
                        ret.Add(new UsuarioModel
                        {
                            Id = (int)dtreader["id_usuario"],
                            Login = (string)dtreader["usuario"],
                            Nome = (string)dtreader["nome"],
                            Email = (string)dtreader["email"],
                            Ativo = (bool)dtreader["status"]
                        });
                    }
                }
            }

            return ret;
        }

        public void CarregarPerfis()
        {
            this.Perfis.Clear();

            using (var conexao = new MySqlConnection())
            {
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                conexao.Open();
                using (var comando = new MySqlCommand())
                {
                    comando.Connection = conexao;
                    comando.CommandText = "select p.* " +
                        "from tb_perfil_usuario pu, tb_perfil p " +
                        "where(pu.id_usuario = @id_usuario) " +
                        "and(pu.id_perfil = p.id_perfil)";
                    comando.Parameters.Add("id_usuario", MySqlDbType.Int32).Value = this.Id;
                    MySqlDataReader dtreader = comando.ExecuteReader();

                    while (dtreader.Read())
                    {
                        this.Perfis.Add(new PerfilModel
                        {
                            Id = (int)dtreader["id_perfil"],
                            Nome = (string)dtreader["nome"]
                        });
                    }
                }
            }
        }

        public static UsuarioModel RecuperarPorId(int id)
        {
            UsuarioModel ret = null;
            using (var conexao = new MySqlConnection())
            {
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                conexao.Open();
                using (var comando = new MySqlCommand())
                {
                    comando.Connection = conexao;
                    comando.CommandText = "select * from tb_usuario where id_usuario = @id";
                    comando.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                    MySqlDataReader dtreader = comando.ExecuteReader();

                    if (dtreader.Read())
                    {
                        ret = new UsuarioModel
                        {
                            Id = (int)dtreader["id_usuario"],
                            Login = (string)dtreader["usuario"],
                            Nome = (string)dtreader["nome"],
                            Email = (string)dtreader["email"],
                            Ativo = (bool)dtreader["status"]
                        };
                    }
                }
            }

            return ret;
        }

        public int SalvarUsuario()
        {
            var ret = 0;
            var model = RecuperarPorId(this.Id);

            using (var conexao = new MySqlConnection())
            {
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                conexao.Open();

                using (var transacao = conexao.BeginTransaction())
                {
                    using (var comando = new MySqlCommand())
                    {
                        comando.Connection = conexao;
                        comando.Transaction = transacao;

                        if (model == null)
                        {
                            comando.CommandText = "insert into tb_usuario (senha, usuario, nome, email, status) " +
                                "values (@senha, @login, @nome, @email, @ativo); select max(id_usuario) as id_usuario from tb_usuario ";
                            comando.Parameters.Add("@senha", MySqlDbType.VarChar).Value = CriptoHelper.HashMD5(Senha);
                            comando.Parameters.Add("@login", MySqlDbType.VarChar).Value = this.Login;
                            comando.Parameters.Add("@nome", MySqlDbType.VarChar).Value = this.Nome;
                            comando.Parameters.Add("@email", MySqlDbType.VarChar).Value = this.Email;
                            comando.Parameters.Add("@ativo", MySqlDbType.Bit).Value = this.Ativo ? 1 : 0;
                            ret = Convert.ToInt32(comando.ExecuteScalar());
                        }
                        else
                        {
                            comando.CommandText = "update tb_usuario set " +
                                "usuario=@login, nome=@nome, email=@email, status=@ativo " +
                                (!string.IsNullOrEmpty(this.Senha) ? ", senha=@senha " : "") +
                                "where id_usuario = @id";
                            comando.Parameters.Add("@id", MySqlDbType.Int32).Value = this.Id;
                            if (!string.IsNullOrEmpty(this.Senha))
                            {
                                comando.Parameters.Add("@senha", MySqlDbType.VarChar).Value = CriptoHelper.HashMD5(Senha);
                            }
                            comando.Parameters.Add("@login", MySqlDbType.VarChar).Value = this.Login;
                            comando.Parameters.Add("@nome", MySqlDbType.VarChar).Value = this.Nome;
                            comando.Parameters.Add("@email", MySqlDbType.VarChar).Value = this.Email;
                            comando.Parameters.Add("@ativo", MySqlDbType.Bit).Value = this.Ativo ? 1 : 0;
                            if (comando.ExecuteNonQuery() > 0)
                            {
                                ret = this.Id;
                            }
                        }
                    }

                    if (this.Perfis != null && this.Perfis.Count > 0)
                    {
                        using (var comandoExclusaoPerfilUsuario = new MySqlCommand())
                        {
                            comandoExclusaoPerfilUsuario.Connection = conexao;
                            comandoExclusaoPerfilUsuario.Transaction = transacao;
                            comandoExclusaoPerfilUsuario.CommandText = "delete from tb_perfil_usuario where (id_usuario=@id_usuario)";
                            comandoExclusaoPerfilUsuario.Parameters.Add("@id_usuario", MySqlDbType.Int32).Value = this.Id;
                            comandoExclusaoPerfilUsuario.ExecuteScalar();
                        }

                        if (this.Perfis[0].Id != -1)
                        {
                            foreach (var perfil in this.Perfis)
                            {
                                using (var comandoInclusaoPerfilUsuario = new MySqlCommand())
                                {
                                    comandoInclusaoPerfilUsuario.Connection = conexao;
                                    comandoInclusaoPerfilUsuario.Transaction = transacao;
                                    comandoInclusaoPerfilUsuario.CommandText = "insert into tb_perfil_usuario (id_perfil, id_usuario) values (@id_perfil, @id_usuario)";
                                    comandoInclusaoPerfilUsuario.Parameters.Add("@id_perfil", MySqlDbType.Int32).Value = perfil.Id;
                                    comandoInclusaoPerfilUsuario.Parameters.Add("@id_usuario", MySqlDbType.Int32).Value = (this.Id == 0 ? ret : this.Id);
                                    comandoInclusaoPerfilUsuario.ExecuteScalar();
                                }
                            }
                        }
                    }

                    transacao.Commit();

                }
            }
            return ret;
        }

        public static bool ExcluirPorId(int id)
        {
            var ret = false;

            if (RecuperarPorId(id) != null)
            {
                using (var conexao = new MySqlConnection())
                {
                    conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                    conexao.Open();
                    using (var comando = new MySqlCommand())
                    {
                        comando.Connection = conexao;
                        comando.CommandText = "delete from tb_usuario where id_usuario = @id";
                        comando.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                        ret = (comando.ExecuteNonQuery() > 0);
                    }
                }
            }
            return ret;
        }

        public static UsuarioModel ValidarUsuario(string login, string senha)
        {
            UsuarioModel ret = null;
            using (var conexao = new MySqlConnection())
            {
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                conexao.Open();
                using (var comando = new MySqlCommand())
                {
                    comando.Connection = conexao;
                    comando.CommandText = "select * from tb_usuario where usuario=@login and senha=@senha";
                    comando.Parameters.Add("@login", MySqlDbType.VarChar).Value = login;
                    comando.Parameters.Add("@senha", MySqlDbType.VarChar).Value = CriptoHelper.HashMD5(senha);
                    MySqlDataReader dtreader = comando.ExecuteReader();
                    if (dtreader.Read())
                    {
                        ret = new UsuarioModel
                        {
                            Id = (int)dtreader["id_usuario"],
                            Login = (string)dtreader["usuario"],
                            Senha = (string)dtreader["senha"],
                            Nome = (string)dtreader["nome"],
                            Email = (string)dtreader["email"],
                            Ativo = (bool)dtreader["status"]
                        };
                    }
                }
            }

            return ret;
        }

        public string RecuperarStringNomePerfis()
        {
            var ret = string.Empty;

            using (var conexao = new MySqlConnection())
            {
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                conexao.Open();
                using (var comando = new MySqlCommand())
                {
                    comando.Connection = conexao;
                    comando.CommandText = string.Format(
                        "select " +
                        "p.nome " +
                        "from tb_perfil_usuario pu, tb_perfil p " +
                        "where pu.id_usuario = @id_usuario " +
                        "and (pu.id_perfil = p.id_perfil) " +
                        "and (p.status = 1) ");

                    comando.Parameters.Add("@id_usuario", MySqlDbType.Int32).Value = this.Id;

                    MySqlDataReader dtreader = comando.ExecuteReader();

                    while (dtreader.Read())
                    {
                        ret += (ret != string.Empty ? ";" : string.Empty) + (string)dtreader["nome"];
                    }
                }
            }

            return ret;

        }

    }
}