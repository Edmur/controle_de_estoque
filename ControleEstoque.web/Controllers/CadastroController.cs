using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ControleEstoque.web.Models;

namespace ControleEstoque.web.Controllers
{
    public class CadastroController : Controller
    {

        #region Usuários

        [Authorize]
        public ActionResult Usuario()
        {
            return View(UsuarioModel.RecuperarLista());
        }

        [HttpPost]
        [Authorize]
        public ActionResult RecuperarUsuario(int id)
        {
            return Json(UsuarioModel.RecuperarPorId(id));
        }

        [HttpPost]
        [Authorize]
        public ActionResult SalvarUsuario(UsuarioModel model)
        {
            var resultado = "OK";
            var mensagens = new List<string>();
            var idSalvo = string.Empty;

            if (!ModelState.IsValid)
            {
                resultado = "AVISO";
                mensagens = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList();
            }
            else
            {
                try
                {
                    var id = model.SalvarUsuario();
                    if (id > 0)
                    {
                        idSalvo = id.ToString();
                    }
                    else
                    {
                        resultado = "ERRO";
                    }
                }
                catch (Exception ex)
                {
                    resultado = "ERRO";
                }

            }

            return Json(new { Resultado = resultado, Mensagens = mensagens, IdSalvo = idSalvo });
        }

        [HttpPost]
        [Authorize]
        public ActionResult ExcluirUsuario(int id)
        {
            return Json(UsuarioModel.ExcluirPorId(id));
        }

        #endregion
        
        #region Categorias de Produtos

        [Authorize]
        public ActionResult CategoriaProduto()
        {
            return View(CategoriaProdutoModel.RecuperarLista());
        }

        [HttpPost]
        [Authorize]
        public ActionResult RecuperarCategoriaProduto(int id)
        {
            return Json(CategoriaProdutoModel.RecuperarPorId(id));
        }

        [HttpPost]
        [Authorize]
        public ActionResult SalvarCategoriaProduto(CategoriaProdutoModel model)
        {
            var resultado = "OK";
            var mensagens = new List<string>();
            var idSalvo = string.Empty;

            if (!ModelState.IsValid)
            {
                resultado = "AVISO";
                mensagens = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList();
            }
            else
            {
                try
                {
                    var id = model.SalvarCategoria();
                    if (id > 0)
                    {
                        idSalvo = id.ToString();
                    }
                    else
                    {
                        resultado = "ERRO";
                    }
                }
                catch (Exception ex)
                {
                    resultado = "ERRO";
                }

            }

            return Json(new { Resultado = resultado, Mensagens = mensagens, IdSalvo = idSalvo });
        }

        [HttpPost]
        [Authorize]
        public ActionResult ExcluirCategoriaProduto(int id)
        {
            return Json(CategoriaProdutoModel.ExcluirPorId(id));
        }

        #endregion

        #region Marcas de Produtos

        [Authorize]
        public ActionResult MarcaProduto()
        {
            return View();
        }

        #endregion

        #region Locais de Produtos

        [Authorize]
        public ActionResult LocalProduto()
        {
            return View();
        }

        #endregion

        #region Unidades de Medidas

        [Authorize]
        public ActionResult UnidadeMedida()
        {
            return View(UnidadeMedidaModel.RecuperarLista());
        }

        [HttpPost]
        [Authorize]
        public ActionResult RecuperarUnidadeMedida(int id)
        {
            return Json(UnidadeMedidaModel.RecuperarPorId(id));
        }

        [HttpPost]
        [Authorize]
        public ActionResult SalvarUnidadeMedida(UnidadeMedidaModel model)
        {
            var resultado = "OK";
            var mensagens = new List<string>();
            var idSalvo = string.Empty;

            if (!ModelState.IsValid)
            {
                resultado = "AVISO";
                mensagens = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList();
            }
            else
            {
                try
                {
                    var id = model.SalvarUnidadeMedida();
                    if (id > 0)
                    {
                        idSalvo = id.ToString();
                    }
                    else
                    {
                        resultado = "ERRO";
                    }
                }
                catch (Exception ex)
                {
                    resultado = "ERRO";
                }

            }

            return Json(new { Resultado = resultado, Mensagens = mensagens, IdSalvo = idSalvo });
        }

        [HttpPost]
        [Authorize]
        public ActionResult ExcluirUnidadeMedida(int id)
        {
            return Json(UnidadeMedidaModel.ExcluirPorId(id));
        }

        #endregion

        #region Produtos

        [Authorize]
        public ActionResult Produto()
        {
            return View(ProdutoModel.RecuperarLista());
        }

        [HttpPost]
        [Authorize]
        public ActionResult RecuperarProduto(int id)
        {
            return Json(ProdutoModel.RecuperarPorId(id));
        }

        [HttpPost]
        [Authorize]
        public ActionResult SalvarProduto(ProdutoModel model)
        {
            var resultado = "OK";
            var mensagens = new List<string>();
            var idSalvo = string.Empty;

            if (!ModelState.IsValid)
            {
                resultado = "AVISO";
                mensagens = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList();
            }
            else
            {
                try
                {
                    var id = model.SalvarProduto();
                    if (id > 0)
                    {
                        idSalvo = id.ToString();
                    }
                    else
                    {
                        resultado = "ERRO";
                    }
                }
                catch (Exception ex)
                {
                    resultado = "ERRO";
                }

            }

            return Json(new { Resultado = resultado, Mensagens = mensagens, IdSalvo = idSalvo });
        }

        [HttpPost]
        [Authorize]
        public ActionResult ExcluirProduto(int id)
        {
            return Json(ProdutoModel.ExcluirPorId(id));
        }

        #endregion

        #region Paises

        [Authorize]
        public ActionResult Pais()
        {
            return View();
        }

        #endregion

        #region Estados

        [Authorize]
        public ActionResult Estado()
        {
            return View();
        }

        #endregion

        #region Cidades

        [Authorize]
        public ActionResult Cidade()
        {
            return View();
        }

        #endregion

        #region Fornecedores

        [Authorize]
        public ActionResult Fornecedor()
        {
            return View();
        }

        #endregion

        #region Perfis de Usuarios

        [Authorize]
        public ActionResult PerfilUsuario()
        {
            return View();
        }

        #endregion

    }
}