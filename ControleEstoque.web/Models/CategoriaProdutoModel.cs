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
    public class CategoriaProdutoModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Descrição deve ser informada.")]
        public string Descricao { get; set; }
        public bool Ativo { get; set; }

        public static List<CategoriaProdutoModel> RecuperarLista()
        {
            var ret = new List<CategoriaProdutoModel>();
            using (var conexao = new MySqlConnection())
            {
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                conexao.Open();
                using (var comando = new MySqlCommand())
                {
                    comando.Connection = conexao;
                    comando.CommandText = string.Format("select * from tb_categoria order by descricao");
                    MySqlDataReader dtreader = comando.ExecuteReader();

                    while (dtreader.Read())
                    {
                        ret.Add(new CategoriaProdutoModel
                        {
                            Id = Convert.ToInt32(dtreader["id_categoria"]),
                            Descricao = Convert.ToString(dtreader["descricao"]),
                            Ativo = Convert.ToBoolean(dtreader["status"])
                        });
                    }
                }
            }

            return ret;
        }

        public static CategoriaProdutoModel RecuperarPorId(int id)
        {
            CategoriaProdutoModel ret = null;
            using (var conexao = new MySqlConnection())
            {
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                conexao.Open();
                using (var comando = new MySqlCommand())
                {
                    comando.Connection = conexao;
                    comando.CommandText = "select * from tb_categoria where id_categoria = @id";
                    comando.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                    MySqlDataReader dtreader = comando.ExecuteReader();

                    if (dtreader.Read())
                    {
                        ret = new CategoriaProdutoModel
                        {
                            Id = Convert.ToInt32(dtreader["id_categoria"]),
                            Descricao = Convert.ToString(dtreader["descricao"]),
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
                        comando.CommandText = "delete from tb_categoria where id_categoria = @id";
                        comando.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                        ret = (comando.ExecuteNonQuery() > 0);
                    }
                }
            }
            return ret;
        }

        public int SalvarCategoria()
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
                        comando.CommandText = "insert into tb_categoria (descricao, status) values (@descricao, @ativo); select max(id_categoria) as id_categoria from tb_categoria ";
                        comando.Parameters.Add("@descricao", MySqlDbType.VarChar).Value = this.Descricao;
                        comando.Parameters.Add("@ativo",MySqlDbType.Bit).Value = this.Ativo ? 1 : 0;
                        ret = Convert.ToInt32(comando.ExecuteScalar());
                    }
                    else
                    {
                        comando.CommandText = "update tb_categoria set descricao=@descricao, status=@ativo where id_categoria = @id";
                        comando.Parameters.Add("@id", MySqlDbType.Int32).Value = this.Id;
                        comando.Parameters.Add("@descricao", MySqlDbType.VarChar).Value = this.Descricao;
                        comando.Parameters.Add("@ativo", MySqlDbType.Bit).Value = this.Ativo ? 1 : 0;
                        if (comando.ExecuteNonQuery()>0)
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