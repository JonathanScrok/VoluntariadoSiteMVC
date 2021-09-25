using Microsoft.AspNetCore.Mvc;
using SyrusVoluntariado.Library.Filters;
using SyrusVoluntariado.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SyrusVoluntariado.Controllers
{
    [Login]
    public class AvaliarController : Controller
    {
        [HttpGet]
        public IActionResult Index(int Id)
        {
            ViewBag.IdUsuAvaliar = Id;
            return View(new Avaliacao());
        }

        [HttpGet]
        public IActionResult PostAvaliacao(int Id)
        { //OBS: Id é a Quantidade de estrelas da avaliação!

            var QtdEstrelas = Id;

            return RedirectToAction("ListaVoluntarios", "Vaga");
        }
    }
}
