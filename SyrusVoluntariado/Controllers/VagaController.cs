using Microsoft.AspNetCore.Mvc;
using SyrusVoluntariado.Database;
using SyrusVoluntariado.Library.Filters;
using SyrusVoluntariado.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SyrusVoluntariado.Controllers {
    [Login]
    public class VagaController : Controller {

        private DatabaseContext _db;

        public VagaController(DatabaseContext db) {
            _db = db;
        }

        [HttpGet]
        public IActionResult Index() {
            //ViewBag.Nivel = niveis;
            return View(new Vaga());
        }

        public IActionResult Cadastrar() {
            //ViewBag.Nivel = niveis;
            return View(new Vaga());
        }

        [HttpPost]
        public IActionResult Cadastrar([FromForm] Vaga vaga) {

            //ViewBag.Nivel = niveis;

            if (ModelState.IsValid) {
                _db.Vagas.Add(vaga);
                _db.SaveChanges();

                TempData["Mensagem"] = "A vaga foi cadastrada com sucesso!";

                return RedirectToAction("Index");
            }

            return View(vaga);
        }

    }
}
