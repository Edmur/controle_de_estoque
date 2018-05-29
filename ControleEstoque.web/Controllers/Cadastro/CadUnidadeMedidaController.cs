using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ControleEstoque.web.Models;

namespace ControleEstoque.web.Controllers
{
    public class CadUnidadeMedidaController : Controller
    {
        private const int _quantMaxLinhaPorPagina = 7;

        [Authorize]
        public ActionResult Index()
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

    }
}