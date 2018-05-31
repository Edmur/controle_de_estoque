using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ControleEstoque.web.Models;

namespace ControleEstoque.web.Controllers
{
    [Authorize(Roles = "Gerente")]
    public class CadUsuarioController : Controller
    {
        private const int _quantMaxLinhaPorPagina = 7;
        private const string _senhaPadrao = "{@127;@188}";

        public ActionResult Index()
        {
            ViewBag.ListaPerfil = PerfilModel.RecuperarLista();
            ViewBag.SenhaPadrao = _senhaPadrao;
            ViewBag.ListaTamPag = new SelectList(new int[] { _quantMaxLinhaPorPagina, 14, 21, 28 }, _quantMaxLinhaPorPagina);
            ViewBag.QuantMaxLinhaPorPagina = _quantMaxLinhaPorPagina;
            ViewBag.PaginaAtual = 1;

            var lista = UsuarioModel.RecuperarLista(ViewBag.PaginaAtual, _quantMaxLinhaPorPagina);
            var quant = UsuarioModel.RecuperarQuantidadeReg();

            var difQuantPaginas = (quant % ViewBag.QuantMaxLinhaPorPagina) > 0 ? 1 : 0;
            ViewBag.QuantPaginas = (quant / ViewBag.QuantMaxLinhaPorPagina + difQuantPaginas);

            return View(lista);
        }

        [HttpPost]
        public JsonResult UsuarioPagina(int pagina, int tamPag)
        {
            var lista = UsuarioModel.RecuperarLista(pagina, tamPag);

            var difQuantPaginas = (lista.Count % ViewBag.QuantMaxLinhaPorPagina) > 0 ? 1 : 0;
            ViewBag.QuantPaginas = (lista.Count / ViewBag.QuantMaxLinhaPorPagina + difQuantPaginas);

            return Json(lista);
        }

        [HttpPost]
        public ActionResult RecuperarUsuario(int id)
        {
            var ret = UsuarioModel.RecuperarPorId(id);
            ret.Senha = _senhaPadrao;
            ret.CarregarPerfis();
            return Json(ret);
        }

        [HttpPost]
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
                    if (model.Senha == _senhaPadrao)
                    {
                        model.Senha = "";
                    }

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
        public ActionResult ExcluirUsuario(int id)
        {
            return Json(UsuarioModel.ExcluirPorId(id));
        }

    }
}