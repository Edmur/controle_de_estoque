using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ControleEstoque.web.Models;

namespace ControleEstoque.web.Controllers
{
    [Authorize(Roles = "Gerente,Administratito,Operador")]
    public class CadEstadoController : Controller
    {
        private const int _quantMaxLinhaPorPagina = 7;

        public ActionResult Index()
        {
            ViewBag.ListaTamPag = new SelectList(new int[] { _quantMaxLinhaPorPagina, 14, 21, 28 }, _quantMaxLinhaPorPagina);
            ViewBag.QuantMaxLinhaPorPagina = _quantMaxLinhaPorPagina;
            ViewBag.PaginaAtual = 1;

            var lista = EstadoModel.RecuperarLista(ViewBag.PaginaAtual, _quantMaxLinhaPorPagina);
            var quant = EstadoModel.RecuperarQuantidadeReg();

            var difQuantPaginas = (quant % ViewBag.QuantMaxLinhaPorPagina) > 0 ? 1 : 0;
            ViewBag.QuantPaginas = (quant / ViewBag.QuantMaxLinhaPorPagina + difQuantPaginas);
            ViewBag.Paises = PaisModel.RecuperarLista(1, 9999);

            return View(lista);
        }

        [HttpPost]
        public JsonResult EstadoPagina(int pagina, int tamPag, string filtro)
        {
            var lista = EstadoModel.RecuperarLista(pagina, tamPag, filtro);

            var difQuantPaginas = (lista.Count % ViewBag.QuantMaxLinhaPorPagina) > 0 ? 1 : 0;
            ViewBag.QuantPaginas = (lista.Count / ViewBag.QuantMaxLinhaPorPagina + difQuantPaginas);

            return Json(lista);
        }

        [HttpPost]
        public JsonResult RecuperarEstadosDoPais(int idPais)
        {

            var lista = EstadoModel.RecuperarLista(1, 9999, idPais: idPais);
            return Json(lista);
        }

        [HttpPost]
        public ActionResult RecuperarEstado(int id)
        {
            return Json(EstadoModel.RecuperarPorId(id));
        }

        [HttpPost]
        public ActionResult SalvarEstado(EstadoModel model)
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
                    var id = model.SalvarEstado();
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
        [Authorize(Roles = "Gerente,Administratito")]
        public ActionResult ExcluirEstado(int id)
        {
            return Json(EstadoModel.ExcluirPorId(id));
        }

    }
}