using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BeaHelper.BLL.BD;
using BeaHelper.BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace BeaHelper.Controllers {
    public class HomeController : Controller {

        public IActionResult Index(int? page) {
            var pageNumber = page ?? 1;
            List<Evento> eventos = Evento_P2.Top8UltimasEventos();

            var resultadoPaginado = eventos.ToPagedList(pageNumber, 8);
            return View(resultadoPaginado);
        }

        public IActionResult QuemSomosNos() {
            return View();
        }

    }
}
