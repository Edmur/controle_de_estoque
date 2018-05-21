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
        public static bool ValidarUsuario(string login, string senha)
        {
            bool ret = false;
            using (var conexao = new MySqlConnection())
            {
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                conexao.Open();
                using (var comando = new MySqlCommand())
                {
                    comando.Connection = conexao;
                    comando.CommandText = "select count(*) as retorno from tb_usuario where usuario=@login and senha=@senha";
                    comando.Parameters.Add("@login", MySqlDbType.VarChar).Value = login;
                    comando.Parameters.Add("@senha", MySqlDbType.VarChar).Value = CriptoHelper.HashMD5(senha);
                    ret = (Convert.ToInt32(comando.ExecuteScalar())>0);
                }
            }

            return ret;
        }
    }
}