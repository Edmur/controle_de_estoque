using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using System.Web;

namespace ControleEstoque.web.Models
{
    public class FornecedorModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Nome deve ser informado.")]
        public string Nome { get; set; }
        [Required(ErrorMessage = "CPF / CNPJ deve ser informado.")]
        public string CpfCnpj { get; set; }
        public string TelefoneFixo { get; set; }
        [Required(ErrorMessage = "Celular 1 deve ser informado.")]
        public string TelefoneCelular1 { get; set; }
        public string TelefoneCelular2 { get; set; }
        [Required(ErrorMessage = "E-mail deve ser informado.")]
        public string Email { get; set; }
        public string Endereco { get; set; }
        public string Numero { get; set; }
        public string Complemento { get; set; }
        public string Bairro { get; set; }
        public string Cep { get; set; }
        [Required(ErrorMessage = "Cidade deve ser informado.")]
        public string Cidade { get; set; }
        [Required(ErrorMessage = "UF deve ser informada.")]
        public string UF { get; set; }
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
                    comando.CommandText = "select count(*) from tb_fornecedor";
                    ret = Convert.ToInt32(comando.ExecuteScalar());
                }
            }

            return ret;
        }

        public static List<FornecedorModel> RecuperarLista(int pagina, int tamPagina, string filtro = "")
        {
            var ret = new List<FornecedorModel>();
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
                        "from tb_fornecedor " +
                        filtroWhere +
                        "order by nome " +
                        "limit {0}, {1}",
                        pos > 0 ? pos : 0, tamPagina );
                    MySqlDataReader dtreader = comando.ExecuteReader();

                    while (dtreader.Read())
                    {
                        ret.Add(new FornecedorModel
                        {
                            Id = Convert.ToInt32(dtreader["id_fornecedor"]),
                            Nome = Convert.ToString(dtreader["nome"]),
                            CpfCnpj = Convert.ToString(dtreader["cpf_cnpj"]),
                            TelefoneFixo = Convert.ToString(dtreader["tel_fixo"]),
                            TelefoneCelular1 = Convert.ToString(dtreader["tel_celular_1"]),
                            TelefoneCelular2 = Convert.ToString(dtreader["tel_celular_2"]),
                            Email = Convert.ToString(dtreader["email"]),
                            Endereco = Convert.ToString(dtreader["endereco"]),
                            Numero = Convert.ToString(dtreader["numero"]),
                            Complemento = Convert.ToString(dtreader["complemento"]),
                            Bairro = Convert.ToString(dtreader["bairro"]),
                            Cep = Convert.ToString(dtreader["cep"]),
                            Cidade = Convert.ToString(dtreader["cidade"]),
                            UF = Convert.ToString(dtreader["uf"]),
                            Ativo = Convert.ToBoolean(dtreader["status"])
                        });
                    }
                }
            }

            return ret;
        }

        public static FornecedorModel RecuperarPorId(int id)
        {
            FornecedorModel ret = null;
            using (var conexao = new MySqlConnection())
            {
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                conexao.Open();
                using (var comando = new MySqlCommand())
                {
                    comando.Connection = conexao;
                    comando.CommandText = "select * from tb_fornecedor where id_fornecedor = @id";
                    comando.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                    MySqlDataReader dtreader = comando.ExecuteReader();

                    if (dtreader.Read())
                    {
                        ret = new FornecedorModel
                        {
                            Id = Convert.ToInt32(dtreader["id_fornecedor"]),
                            Nome = Convert.ToString(dtreader["nome"]),
                            CpfCnpj = Convert.ToString(dtreader["cpf_cnpj"]),
                            TelefoneFixo = Convert.ToString(dtreader["tel_fixo"]),
                            TelefoneCelular1 = Convert.ToString(dtreader["tel_celular_1"]),
                            TelefoneCelular2 = Convert.ToString(dtreader["tel_celular_2"]),
                            Email = Convert.ToString(dtreader["email"]),
                            Endereco = Convert.ToString(dtreader["endereco"]),
                            Numero = Convert.ToString(dtreader["numero"]),
                            Complemento = Convert.ToString(dtreader["complemento"]),
                            Bairro = Convert.ToString(dtreader["bairro"]),
                            Cep = Convert.ToString(dtreader["cep"]),
                            Cidade = Convert.ToString(dtreader["cidade"]),
                            UF = Convert.ToString(dtreader["uf"]),
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
                        comando.CommandText = "delete from tb_fornecedor where id_fornecedor = @id";
                        comando.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                        ret = (comando.ExecuteNonQuery() > 0);
                    }
                }
            }
            return ret;
        }

        public int SalvarFornecedor()
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
                        comando.CommandText = "insert into tb_fornecedor " +
                            "(nome, cpf_cnpj, tel_fixo, tel_celular_1, tel_celular_2, email, endereco, numero, complemento, bairro, cidade, estado, status) " +
                            "values (@nome, @cpf_cnpj, @tel_fixo, @tel_celular_1, @tel_celular_2, @email, @endereco, @numero,, @complemento, @bairro, @cidade, @uf, @ativo); " +
                            "select max(id_fornecedor) as id_fornecedor from tb_fornecedor;";
                        comando.Parameters.Add("@nome", MySqlDbType.VarChar).Value = this.Nome;
                        comando.Parameters.Add("@cpf_cnpj", MySqlDbType.VarChar).Value = this.CpfCnpj;
                        comando.Parameters.Add("@tel_fixo", MySqlDbType.VarChar).Value = this.TelefoneFixo;
                        comando.Parameters.Add("@tel_celular_1", MySqlDbType.VarChar).Value = this.TelefoneCelular1;
                        comando.Parameters.Add("@tel_celular_2", MySqlDbType.VarChar).Value = this.TelefoneCelular2;
                        comando.Parameters.Add("@email", MySqlDbType.VarChar).Value = this.Email;
                        comando.Parameters.Add("@endereco", MySqlDbType.VarChar).Value = this.Endereco;
                        comando.Parameters.Add("@numero", MySqlDbType.VarChar).Value = this.Numero;
                        comando.Parameters.Add("@complemento", MySqlDbType.VarChar).Value = this.Complemento;
                        comando.Parameters.Add("@bairro", MySqlDbType.VarChar).Value = this.Bairro;
                        comando.Parameters.Add("@cep", MySqlDbType.VarChar).Value = this.Cep;
                        comando.Parameters.Add("@cidade", MySqlDbType.VarChar).Value = this.Cidade;
                        comando.Parameters.Add("@uf", MySqlDbType.VarChar).Value = this.UF;
                        comando.Parameters.Add("@ativo", MySqlDbType.Bit).Value = this.Ativo ? 1 : 0;
                        ret = Convert.ToInt32(comando.ExecuteScalar());
                    }
                    else
                    {
                        comando.CommandText = "update tb_fornecedor set " +
                            "nome=@nome, " +
                            "cpf_cnpj=@cpf_cnpj, " +
                            "tel_fixo=@tel_fixo, " +
                            "tel_celular_1=@tel_celular_1, " +
                            "tel_celular_2=@tel_celular_2, " +
                            "email=@email, " +
                            "endereco=@endereco, " +
                            "numero=@numero, " +
                            "complemento=@complemento, " +
                            "bairro=@bairro, " +
                            "cep=@cep, " +
                            "cidade=@cidade, " +
                            "uf=@uf, " +
                            "status=@ativo where id_fornecedor = @id";
                        comando.Parameters.Add("@id", MySqlDbType.VarChar).Value = this.Id;
                        comando.Parameters.Add("@nome", MySqlDbType.VarChar).Value = this.Nome;
                        comando.Parameters.Add("@cpf_cnpj", MySqlDbType.VarChar).Value = this.CpfCnpj;
                        comando.Parameters.Add("@tel_fixo", MySqlDbType.VarChar).Value = this.TelefoneFixo;
                        comando.Parameters.Add("@tel_celular_1", MySqlDbType.VarChar).Value = this.TelefoneCelular1;
                        comando.Parameters.Add("@tel_celular_2", MySqlDbType.VarChar).Value = this.TelefoneCelular2;
                        comando.Parameters.Add("@email", MySqlDbType.VarChar).Value = this.Email;
                        comando.Parameters.Add("@endereco", MySqlDbType.VarChar).Value = this.Endereco;
                        comando.Parameters.Add("@numero", MySqlDbType.VarChar).Value = this.Numero;
                        comando.Parameters.Add("@complemento", MySqlDbType.VarChar).Value = this.Complemento;
                        comando.Parameters.Add("@bairro", MySqlDbType.VarChar).Value = this.Bairro;
                        comando.Parameters.Add("@cep", MySqlDbType.VarChar).Value = this.Cep;
                        comando.Parameters.Add("@cidade", MySqlDbType.VarChar).Value = this.Cidade;
                        comando.Parameters.Add("@uf", MySqlDbType.VarChar).Value = this.UF;
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