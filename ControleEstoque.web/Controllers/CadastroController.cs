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
        private const int _quantMaxLinhaPorPagina = 7;

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

    }
}