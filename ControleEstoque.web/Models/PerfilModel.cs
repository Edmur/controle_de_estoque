using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ControleEstoque.web.Models
{
    public class PerfilModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Nome do perfil deve ser informado.")]
        public string Nome { get; set; }
        public bool Ativo { get; set; }

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
                    comando.CommandText = "select count(*) from tb_perfil";
                    ret = Convert.ToInt32(comando.ExecuteScalar());
                }
            }

            return ret;
        }

        public static List<PerfilModel> RecuperarLista(int pagina = -1, int tamPagina = -1, string filtro = "")
        {
            var ret = new List<PerfilModel>();
            using (var conexao = new MySqlConnection())
            {
                var pos = (pagina - 1) * tamPagina;
                var filtroWhere = "";
                if (!string.IsNullOrEmpty(filtro))
                {
                    filtroWhere = string.Format(" where lower(nome) like '%{0}%' ", filtro.ToLower());
                }

                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                conexao.Open();
                using (var comando = new MySqlCommand())
                {
                    comando.Connection = conexao;

                    if (pagina == -1 || tamPagina == -1)
                    {
                        comando.CommandText = "select * from tb_perfil where status = 1 order by nome";
                    }
                    else
                    {
                        comando.CommandText = string.Format(
                            "select * " +
                            "from tb_perfil " +
                            filtroWhere +
                            "order by nome " +
                            "limit {0}, {1}",
                            pos > 0 ? pos : 0, tamPagina);
                    }

                    MySqlDataReader dtreader = comando.ExecuteReader();

                    while (dtreader.Read())
                    {
                        ret.Add(new PerfilModel
                        {
                            Id = Convert.ToInt32(dtreader["id_perfil"]),
                            Nome = Convert.ToString(dtreader["nome"]),
                            Ativo = Convert.ToBoolean(dtreader["status"])
                        });
                    }
                }
            }

            return ret;
        }

        public static List<PerfilModel> RecuperarListaAtivos()
        {
            var ret = new List<PerfilModel>();
            using (var conexao = new MySqlConnection())
            {
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                conexao.Open();
                using (var comando = new MySqlCommand())
                {
                    comando.Connection = conexao;
                    comando.CommandText = string.Format("select * from tb_perfil where status = 1 order by nome");
                    MySqlDataReader dtreader = comando.ExecuteReader();

                    while (dtreader.Read())
                    {
                        ret.Add(new PerfilModel
                        {
                            Id = Convert.ToInt32(dtreader["id_perfil"]),
                            Nome = Convert.ToString(dtreader["nome"]),
                            Ativo = Convert.ToBoolean(dtreader["status"])
                        });
                    }
                }
            }

            return ret;
        }

        public static PerfilModel RecuperarPorId(int id)
        {
            PerfilModel ret = null;
            using (var conexao = new MySqlConnection())
            {
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                conexao.Open();
                using (var comando = new MySqlCommand())
                {
                    comando.Connection = conexao;
                    comando.CommandText = "select * from tb_perfil where id_perfil = @id";
                    comando.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                    MySqlDataReader dtreader = comando.ExecuteReader();

                    if (dtreader.Read())
                    {
                        ret = new PerfilModel
                        {
                            Id = Convert.ToInt32(dtreader["id_perfil"]),
                            Nome = Convert.ToString(dtreader["nome"]),
                            Ativo = Convert.ToBoolean(dtreader["status"])
                        };
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
                        comando.CommandText = "delete from tb_perfil where id_perfil = @id";
                        comando.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                        ret = (comando.ExecuteNonQuery() > 0);
                    }
                }
            }
            return ret;
        }

        public int SalvarPerfil()
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
                        comando.CommandText = "insert into tb_perfil (nome, status) values (@nome, @ativo); select max(id_perfil) as id_perfil  from tb_perfil ";
                        comando.Parameters.Add("@nome", MySqlDbType.VarChar).Value = this.Nome;
                        comando.Parameters.Add("@ativo", MySqlDbType.Bit).Value = this.Ativo ? 1 : 0;
                        ret = Convert.ToInt32(comando.ExecuteScalar());
                    }
                    else
                    {
                        comando.CommandText = "update tb_perfil set nome=@nome, status=@ativo where id_perfil = @id";
                        comando.Parameters.Add("@id", MySqlDbType.Int32).Value = this.Id;
                        comando.Parameters.Add("@nome", MySqlDbType.VarChar).Value = this.Nome;
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
    }
}