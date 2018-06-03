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
    public class PaisModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Nome deve ser informado.")]
        [MaxLength(30, ErrorMessage = "Nome deve ter no máximo 30 caracteres.")]
        public string Nome { get; set; }
        [Required(ErrorMessage = "Código internacional deve ser informado.")]
        [MaxLength(3, ErrorMessage = "Código internacional deve ter no máximo 3 caracteres.")]
        public string Codigo { get; set; }
        [Required(ErrorMessage = "Código ISO deve ser informado.")]
        [MaxLength(3, ErrorMessage = "Código ISO deve ter no máximo 3 caracteres.")]
        public string Iso { get; set; }
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
                    comando.CommandText = "select count(*) from tb_pais";
                    ret = Convert.ToInt32(comando.ExecuteScalar());
                }
            }

            return ret;
        }

        public static List<PaisModel> RecuperarLista(int pagina, int tamPagina, string filtro = "")
        {
            var ret = new List<PaisModel>();
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
                    comando.CommandText = string.Format("select * " +
                        "from tb_pais " +
                        filtroWhere +
                        "order by nome " +
                        "limit {0}, {1}",
                        pos > 0 ? pos : 0, tamPagina);
                    MySqlDataReader dtreader = comando.ExecuteReader();

                    while (dtreader.Read())
                    {
                        ret.Add(new PaisModel
                        {
                            Id = Convert.ToInt32(dtreader["id_pais"]),
                            Nome = Convert.ToString(dtreader["nome"]),
                            Codigo = Convert.ToString(dtreader["codigo"]),
                            Iso = Convert.ToString(dtreader["iso"]),
                            Ativo = Convert.ToBoolean(dtreader["status"])
                        });
                    }
                }
            }

            return ret;
        }

        public static PaisModel RecuperarPorId(int id)
        {
            PaisModel ret = null;
            using (var conexao = new MySqlConnection())
            {
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                conexao.Open();
                using (var comando = new MySqlCommand())
                {
                    comando.Connection = conexao;
                    comando.CommandText = "select * from tb_pais where id_pais = @id";
                    comando.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                    MySqlDataReader dtreader = comando.ExecuteReader();

                    if (dtreader.Read())
                    {
                        ret = new PaisModel
                        {
                            Id = Convert.ToInt32(dtreader["id_pais"]),
                            Nome = Convert.ToString(dtreader["nome"]),
                            Codigo = Convert.ToString(dtreader["codigo"]),
                            Iso = Convert.ToString(dtreader["iso"]),
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
                        comando.CommandText = "delete from tb_pais where id_pais = @id";
                        comando.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                        ret = (comando.ExecuteNonQuery() > 0);
                    }
                }
            }
            return ret;
        }

        public int SalvarPais()
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
                        comando.CommandText = "insert into tb_pais (nome, codigo, iso, status) values (@nome, @codigo, @iso, @ativo); select max(id_pais) as id_pais from tb_pais ";
                        comando.Parameters.Add("@nome", MySqlDbType.VarChar).Value = this.Nome;
                        comando.Parameters.Add("@codigo", MySqlDbType.VarChar).Value = this.Codigo;
                        comando.Parameters.Add("@iso", MySqlDbType.VarChar).Value = this.Iso;
                        comando.Parameters.Add("@ativo", MySqlDbType.Bit).Value = this.Ativo ? 1 : 0;
                        ret = Convert.ToInt32(comando.ExecuteScalar());
                    }
                    else
                    {
                        comando.CommandText = "update tb_pais set nome=@nome, codigo=@codigo, iso=@iso, status=@ativo where id_pais = @id";
                        comando.Parameters.Add("@id", MySqlDbType.Int32).Value = this.Id;
                        comando.Parameters.Add("@nome", MySqlDbType.VarChar).Value = this.Nome;
                        comando.Parameters.Add("@codigo", MySqlDbType.VarChar).Value = this.Codigo;
                        comando.Parameters.Add("@iso", MySqlDbType.VarChar).Value = this.Iso;
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