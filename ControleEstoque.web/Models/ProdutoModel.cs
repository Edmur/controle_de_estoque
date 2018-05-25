using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using System.Web;

namespace ControleEstoque.web.Models
{
    public class ProdutoModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Categoria deve ser informada.")]
        public int Id_Categoria { get; set; }
        [Required(ErrorMessage = "Fornecedor deve ser informado.")]
        public int Id_Fornecedor { get; set; }
        [Required(ErrorMessage = "Código EAN deve ser informado.")]
        public string Ean { get; set; }
        [Required(ErrorMessage = "Descrição deve ser informada.")]
        public string Descricao { get; set; }
        [Required(ErrorMessage = "Preço de custo deve ser informado.")]
        public decimal PrecoCusto { get; set; }
        [Required(ErrorMessage = "Preço de venda deve ser informado.")]
        public decimal PrecoVenda { get; set; }
        [Required(ErrorMessage = "Unidade de medida deve ser informada.")]
        public int Id_UnidadeMedida { get; set; }
        [Required(ErrorMessage = "Qtde da embalagem deve ser informada.")]
        public int Qt_UnidadeMedida { get; set; }
        public bool Ativo { get; set; }

        public static List<ProdutoModel> RecuperarLista()
        {
            var ret = new List<ProdutoModel>();
            using (var conexao = new MySqlConnection())
            {
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                conexao.Open();
                using (var comando = new MySqlCommand())
                {
                    comando.Connection = conexao;
                    comando.CommandText = string.Format("select * from tb_produto order by descricao");
                    MySqlDataReader dtreader = comando.ExecuteReader();

                    while (dtreader.Read())
                    {
                        ret.Add(new ProdutoModel
                        {
                            Id = Convert.ToInt32(dtreader["id_produto"]),
                            Id_Categoria = Convert.ToInt32(dtreader["id_categoria"]),
                            Id_Fornecedor = Convert.ToInt32(dtreader["id_fornecedor"]),
                            Ean = Convert.ToString(dtreader["ean"]),
                            Descricao = Convert.ToString(dtreader["descricao"]),
                            PrecoCusto = Convert.ToDecimal(dtreader["preco_custo"]),
                            PrecoVenda = Convert.ToDecimal(dtreader["preco_venda"]),
                            Id_UnidadeMedida = Convert.ToInt32(dtreader["id_unidade_medida"]),
                            Qt_UnidadeMedida = Convert.ToInt32(dtreader["qt_unidade"]),
                            Ativo = Convert.ToBoolean(dtreader["status"])
                        });
                    }
                }
            }

            return ret;
        }

        public static ProdutoModel RecuperarPorId(int id)
        {
            ProdutoModel ret = null;
            using (var conexao = new MySqlConnection())
            {
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                conexao.Open();
                using (var comando = new MySqlCommand())
                {
                    comando.Connection = conexao;
                    comando.CommandText = "select * from tb_produto where id_produto = @id";
                    comando.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                    MySqlDataReader dtreader = comando.ExecuteReader();

                    if (dtreader.Read())
                    {
                        ret = new ProdutoModel
                        {
                            Id = Convert.ToInt32(dtreader["id_produto"]),
                            Id_Categoria = Convert.ToInt32(dtreader["id_categoria"]),
                            Id_Fornecedor = Convert.ToInt32(dtreader["id_fornecedor"]),
                            Ean = Convert.ToString(dtreader["ean"]),
                            Descricao = Convert.ToString(dtreader["descricao"]),
                            PrecoCusto = Convert.ToDecimal(dtreader["preco_custo"]),
                            PrecoVenda = Convert.ToDecimal(dtreader["preco_venda"]),
                            Id_UnidadeMedida = Convert.ToInt32(dtreader["id_unidade_medida"]),
                            Qt_UnidadeMedida = Convert.ToInt32(dtreader["qt_unidade"]),
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
                        comando.CommandText = "delete from tb_produto where id_produto = @id";
                        comando.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                        ret = (comando.ExecuteNonQuery() > 0);
                    }
                }
            }
            return ret;
        }

        public int SalvarProduto()
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
                        comando.CommandText = "insert into tb_produto (id_categoria, id_fornecedor, ean, descricao, preco_custo, preco_venda, id_unidade_medida, qt_unidade, status) values (@id_categoria, @id_fornecedor, @ean, @descricao, @preco_custo, @preco_venda, @id_unidade_medida, @qt_unidade, @ativo); select max(id_produto) as id_produto from tb_produto;";
                        comando.Parameters.Add("@id_categoria", MySqlDbType.VarChar).Value = this.Id_Categoria;
                        comando.Parameters.Add("@id_fornecedor", MySqlDbType.VarChar).Value = this.Id_Fornecedor;
                        comando.Parameters.Add("@ean", MySqlDbType.VarChar).Value = this.Ean;
                        comando.Parameters.Add("@descricao", MySqlDbType.VarChar).Value = this.Descricao;
                        comando.Parameters.Add("@preco_custo", MySqlDbType.VarChar).Value = this.PrecoCusto;
                        comando.Parameters.Add("@preco_venda", MySqlDbType.VarChar).Value = this.PrecoVenda;
                        comando.Parameters.Add("@id_unidade_medida", MySqlDbType.VarChar).Value = this.Id_UnidadeMedida;
                        comando.Parameters.Add("@qt_unidade", MySqlDbType.VarChar).Value = this.Qt_UnidadeMedida;
                        comando.Parameters.Add("@ativo", MySqlDbType.Bit).Value = this.Ativo ? 1 : 0;
                        ret = Convert.ToInt32(comando.ExecuteScalar());
                    }
                    else
                    {
                        comando.CommandText = "update tb_produto set id_categoria=@id_categoria, id_fornecedor=@id_fornecedor, ean=@ean, descricao=@descricao, preco_custo=@preco_custo, preco_venda=@preco_venda, id_unidade_medida=@id_unidade_medida, qt_unidade=@qt_unidade, status=@ativo where id_produto = @id";
                        comando.Parameters.Add("@id", MySqlDbType.VarChar).Value = this.Id;
                        comando.Parameters.Add("@id_categoria", MySqlDbType.VarChar).Value = this.Id_Categoria;
                        comando.Parameters.Add("@id_fornecedor", MySqlDbType.VarChar).Value = this.Id_Fornecedor;
                        comando.Parameters.Add("@ean", MySqlDbType.VarChar).Value = this.Ean;
                        comando.Parameters.Add("@descricao", MySqlDbType.VarChar).Value = this.Descricao;
                        comando.Parameters.Add("@preco_custo", MySqlDbType.VarChar).Value = this.PrecoCusto;
                        comando.Parameters.Add("@preco_venda", MySqlDbType.VarChar).Value = this.PrecoVenda;
                        comando.Parameters.Add("@id_unidade_medida", MySqlDbType.VarChar).Value = this.Id_UnidadeMedida;
                        comando.Parameters.Add("@qt_unidade", MySqlDbType.VarChar).Value = this.Qt_UnidadeMedida;
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