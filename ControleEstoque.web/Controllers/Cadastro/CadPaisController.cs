﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ControleEstoque.web.Models;

namespace ControleEstoque.web.Controllers
{
    [Authorize(Roles = "Gerente,Administratito,Operador")]
    public class CadPaisController : Controller
    {
        private const int _quantMaxLinhaPorPagina = 7;

        public ActionResult Index()
        {
            ViewBag.ListaTamPag = new SelectList(new int[] { _quantMaxLinhaPorPagina, 14, 21, 28 }, _quantMaxLinhaPorPagina);
            ViewBag.QuantMaxLinhaPorPagina = _quantMaxLinhaPorPagina;
            ViewBag.PaginaAtual = 1;

            var lista = PaisModel.RecuperarLista(ViewBag.PaginaAtual, _quantMaxLinhaPorPagina);
            var quant = PaisModel.RecuperarQuantidadeReg();

            var difQuantPaginas = (quant % ViewBag.QuantMaxLinhaPorPagina) > 0 ? 1 : 0;
            ViewBag.QuantPaginas = (quant / ViewBag.QuantMaxLinhaPorPagina + difQuantPaginas);

            return View(lista);
        }

        [HttpPost]
        public JsonResult PaisPagina(int pagina, int tamPag, string filtro)
        {
            var lista = PaisModel.RecuperarLista(pagina, tamPag, filtro);

            var difQuantPaginas = (lista.Count % ViewBag.QuantMaxLinhaPorPagina) > 0 ? 1 : 0;
            ViewBag.QuantPaginas = (lista.Count / ViewBag.QuantMaxLinhaPorPagina + difQuantPaginas);

            return Json(lista);
        }

        [HttpPost]
        public ActionResult RecuperarPais(int id)
        {
            return Json(PaisModel.RecuperarPorId(id));
        }

        [HttpPost]
        public ActionResult SalvarPais(PaisModel model)
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
                    var id = model.SalvarPais();
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
        public ActionResult ExcluirPais(int id)
        {
            return Json(PaisModel.ExcluirPorId(id));
        }

    }
}