using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SyrusVoluntariado.BLL;
using SyrusVoluntariado.Database;
using SyrusVoluntariado.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace SyrusVoluntariado.Controllers {
    public class HomeController : Controller {

        public IActionResult Index(int? page) {
            var pageNumber = page ?? 1;

            List<Vaga> vagas = Vaga_P1.TodasVagas();

            var resultadoPaginado = vagas.ToPagedList(pageNumber, 6);
            return View(resultadoPaginado);
        }

        public IActionResult QuemSomosNos() {
            return View();
        }


    }
}
