﻿using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using System.Web;

namespace ControleEstoque.web.Models
{
    public class CidadeModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Estado deve ser informado.")]
        public int Id_Estado { get; set; }
        [Required(ErrorMessage = "País deve ser informado.")]
        public int Id_Pais { get; set; }
        [Required(ErrorMessage = "Nome deve ser informado.")]
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
                    comando.CommandText = "select count(*) from tb_cidade";
                    ret = Convert.ToInt32(comando.ExecuteScalar());
                }
            }

            return ret;
        }

        public static List<CidadeModel> RecuperarLista(int pagina, int tamPagina, string filtro = "")
        {
            var ret = new List<CidadeModel>();
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
                        "from tb_cidade " +
                        filtroWhere +
                        "order by nome " +
                        "limit {0}, {1}",
                        pos > 0 ? pos : 0, tamPagina);
                    MySqlDataReader dtreader = comando.ExecuteReader();

                    while (dtreader.Read())
                    {
                        ret.Add(new CidadeModel
                        {
                            Id = Convert.ToInt32(dtreader["id_cidade"]),
                            Id_Estado = Convert.ToInt32(dtreader["id_estado"]),
                            Nome = Convert.ToString(dtreader["nome"]),
                            Ativo = Convert.ToBoolean(dtreader["status"])
                        });
                    }
                }
            }

            return ret;
        }

        public static CidadeModel RecuperarPorId(int id)
        {
            CidadeModel ret = null;
            using (var conexao = new MySqlConnection())
            {
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                conexao.Open();
                using (var comando = new MySqlCommand())
                {
                    comando.Connection = conexao;
                    comando.CommandText =
                        "select tc.id_cidade, " +
                        "       te.id_pais, " +
                        "       tc.id_estado, " +
                        "       tc.nome, " +
                        "       tc.status " +
                        "from tb_cidade tc, tb_estado te " +
                        "where tc.id_estado = te.id_estado " +
                        "and id_cidade = @id";
                    comando.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                    MySqlDataReader dtreader = comando.ExecuteReader();

                    if (dtreader.Read())
                    {
                        ret = new CidadeModel
                        {
                            Id = Convert.ToInt32(dtreader["id_cidade"]),
                            Id_Pais = Convert.ToInt32(dtreader["id_pais"]),
                            Id_Estado = Convert.ToInt32(dtreader["id_estado"]),
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
                        comando.CommandText = "delete from tb_cidade where id_cidade = @id";
                        comando.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                        ret = (comando.ExecuteNonQuery() > 0);
                    }
                }
            }
            return ret;
        }

        public int SalvarCidade()
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
                        comando.CommandText = "insert into tb_cidade (id_estado, nome, status) values (@id_estado, @nome, @ativo); select max(id_cidade) as id_cidade from tb_cidade ";
                        comando.Parameters.Add("@id_estado", MySqlDbType.VarChar).Value = this.Id_Estado;
                        comando.Parameters.Add("@nome", MySqlDbType.VarChar).Value = this.Nome;
                        comando.Parameters.Add("@ativo", MySqlDbType.Bit).Value = this.Ativo ? 1 : 0;
                        ret = Convert.ToInt32(comando.ExecuteScalar());
                    }
                    else
                    {
                        comando.CommandText = "update tb_cidade set id_estado=@id_estado, nome=@nome, status=@ativo where id_cidade = @id";
                        comando.Parameters.Add("@id", MySqlDbType.Int32).Value = this.Id;
                        comando.Parameters.Add("@id_estado", MySqlDbType.VarChar).Value = this.Id_Estado;
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