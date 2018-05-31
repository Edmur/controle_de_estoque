using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ControleEstoque.web.Models;

namespace ControleEstoque.web.Controllers
{
    public class CadProdutoController : Controller
    {
        private const int _quantMaxLinhaPorPagina = 7;

        [Authorize]
        public ActionResult Index()
        {
            ViewBag.ListaTamPag = new SelectList(new int[] { _quantMaxLinhaPorPagina, 14, 21, 28 }, _quantMaxLinhaPorPagina);
            ViewBag.QuantMaxLinhaPorPagina = _quantMaxLinhaPorPagina;
            ViewBag.PaginaAtual = 1;

            var lista = ProdutoModel.RecuperarLista(ViewBag.PaginaAtual, _quantMaxLinhaPorPagina);
            var quant = ProdutoModel.RecuperarQuantidadeReg();

            var difQuantPaginas = (quant % ViewBag.QuantMaxLinhaPorPagina) > 0 ? 1 : 0;
            ViewBag.QuantPaginas = (quant / ViewBag.QuantMaxLinhaPorPagina + difQuantPaginas);
            ViewBag.UnidadesMedida = UnidadeMedidaModel.RecuperarLista(1, 9999);
            ViewBag.CategoriasProduto = CategoriaProdutoModel.RecuperarLista(1, 9999);
            ViewBag.Fornecedores = FornecedorModel.RecuperarLista(1, 9999);

            return View(lista);
        }

        [HttpPost]
        [Authorize]
        public JsonResult ProdutoPagina(int pagina, int tamPag)
        {
            var lista = ProdutoModel.RecuperarLista(pagina, tamPag);

            var difQuantPaginas = (lista.Count % ViewBag.QuantMaxLinhaPorPagina) > 0 ? 1 : 0;
            ViewBag.QuantPaginas = (lista.Count / ViewBag.QuantMaxLinhaPorPagina + difQuantPaginas);

            return Json(lista);
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

    }
}