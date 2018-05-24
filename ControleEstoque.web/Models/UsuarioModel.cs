using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using System.Data;
using System.Configuration;
using ControleEstoque.web.Helpers;

namespace ControleEstoque.web.Models
{
    public class UsuarioModel
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Senha { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public bool Ativo { get; set; }

        public static List<UsuarioModel> RecuperarLista()
        {
            var ret = new List<UsuarioModel>();
            using (var conexao = new MySqlConnection())
            {
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                conexao.Open();
                using (var comando = new MySqlCommand())
                {
                    comando.Connection = conexao;
                    comando.CommandText = string.Format("select * from tb_usuario order by nome");
                    MySqlDataReader dtreader = comando.ExecuteReader();

                    while (dtreader.Read())
                    {
                        ret.Add(new UsuarioModel
                        {
                            Id = (int)dtreader["id_usuario"],
                            Login = (string)dtreader["usuario"],
                            Senha = (string)dtreader["senha"],
                            Nome = (string)dtreader["nome"],
                            Email = (string)dtreader["email"],
                            Ativo = (bool)dtreader["status"]
                        });
                    }
                }
            }

            return ret;
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

        public int SalvarUsuario()
        {
            var ret = 0;
            var model = RecuperarPorId(this.Id);

            using (var conexao = new MySqlConnection())
            {
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                conexao.Open();
                using (var comando = new MySqlCommand())
                {
                    comando.Connection = conexao;
                    if (model == null)
                    {
                        comando.CommandText = "insert into tb_usuario (senha, usuario, nome, email, status) values (@senha, @login, @nome, @email, @ativo); select max(id_usuario) as id_usuario from tb_usuario ";
                        comando.Parameters.Add("@senha", MySqlDbType.VarChar).Value = this.Senha;
                        comando.Parameters.Add("@login", MySqlDbType.VarChar).Value = this.Login;
                        comando.Parameters.Add("@nome", MySqlDbType.VarChar).Value = this.Nome;
                        comando.Parameters.Add("@email", MySqlDbType.VarChar).Value = this.Email;
                        comando.Parameters.Add("@ativo", MySqlDbType.Bit).Value = this.Ativo ? 1 : 0;
                        ret = Convert.ToInt32(comando.ExecuteScalar());
                    }
                    else
                    {
                        comando.CommandText = "update tb_usuario set "+
                            (!string.IsNullOrEmpty(this.Senha) ? " senha=@senha, " : "")+
                            "usuario=@login, nome=@nome, email=@email, status=@ativo where id_usuario = @id";
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
    }
}