using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using System.Web;

namespace ControleEstoque.web.Models
{
    public class UnidadeMedidaModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Sigla deve ser informada.")]
        public string Sigla { get; set; }
        [Required(ErrorMessage = "Descrição deve ser informada.")]
        public string Descricao { get; set; }
        public bool Ativo { get; set; }

        public static List<UnidadeMedidaModel> RecuperarLista()
        {
            var ret = new List<UnidadeMedidaModel>();
            using (var conexao = new MySqlConnection())
            {
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                conexao.Open();
                using (var comando = new MySqlCommand())
                {
                    comando.Connection = conexao;
                    comando.CommandText = string.Format("select * from tb_unidade_medida order by sigla");
                    MySqlDataReader dtreader = comando.ExecuteReader();

                    while (dtreader.Read())
                    {
                        ret.Add(new UnidadeMedidaModel
                        {
                            Id = Convert.ToInt32(dtreader["id_unidade_medida"]),
                            Sigla = Convert.ToString(dtreader["sigla"]),
                            Descricao = Convert.ToString(dtreader["descricao"]),
                            Ativo = Convert.ToBoolean(dtreader["status"])
                        });
                    }
                }
            }

            return ret;
        }

        public static UnidadeMedidaModel RecuperarPorId(int id)
        {
            UnidadeMedidaModel ret = null;
            using (var conexao = new MySqlConnection())
            {
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                conexao.Open();
                using (var comando = new MySqlCommand())
                {
                    comando.Connection = conexao;
                    comando.CommandText = "select * from tb_unidade_medida where id_unidade_medida = @id";
                    comando.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                    MySqlDataReader dtreader = comando.ExecuteReader();

                    if (dtreader.Read())
                    {
                        ret = new UnidadeMedidaModel
                        {
                            Id = Convert.ToInt32(dtreader["id_unidade_medida"]),
                            Sigla = Convert.ToString(dtreader["sigla"]),
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
                        comando.CommandText = "delete from tb_unidade_medida where id_unidade_medida = @id";
                        comando.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                        ret = (comando.ExecuteNonQuery() > 0);
                    }
                }
            }
            return ret;
        }

        public int SalvarUnidadeMedida()
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
                        comando.CommandText = "insert into tb_unidade_medida (sigla, descricao, status) values (@sigla, @descricao, @ativo); select max(id_unidade_medida) as id_unidade_medida from tb_unidade_medida";
                        comando.Parameters.Add("@sigla", MySqlDbType.VarChar).Value = this.Sigla;
                        comando.Parameters.Add("@descricao", MySqlDbType.VarChar).Value = this.Descricao;
                        comando.Parameters.Add("@ativo", MySqlDbType.Bit).Value = this.Ativo ? 1 : 0;
                        ret = Convert.ToInt32(comando.ExecuteScalar());
                    }
                    else
                    {
                        comando.CommandText = "update tb_unidade_medida set sigla=@sigla, descricao=@descricao, status=@ativo where id_unidade_medida = @id";
                        comando.Parameters.Add("@id", MySqlDbType.VarChar).Value = this.Id;
                        comando.Parameters.Add("@sigla", MySqlDbType.VarChar).Value = this.Sigla;
                        comando.Parameters.Add("@descricao", MySqlDbType.VarChar).Value = this.Descricao;
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