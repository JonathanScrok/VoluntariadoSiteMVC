using Microsoft.AspNetCore.Mvc;
using SyrusVoluntariado.Database;
using SyrusVoluntariado.Library.Filters;
using SyrusVoluntariado.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace SyrusVoluntariado.Controllers {
    [Login]
    public class VagaController : Controller {

        private DatabaseContext _db;

        public VagaController(DatabaseContext db) {
            _db = db;
        }

        public IActionResult Index(int? page) {
            var pageNumber = page ?? 1;

            var palavras = _db.Vagas.ToList();
            var resultadoPaginado = palavras.ToPagedList(pageNumber, 10);

            return View(resultadoPaginado);
        }

        [HttpGet]
        public IActionResult Cadastrar() {
            //ViewBag.Nivel = niveis;
            return View(new Vaga());
        }

        [HttpPost]
        public IActionResult Cadastrar([FromForm] Vaga vaga) {

            //ViewBag.Nivel = niveis;

            if (ModelState.IsValid) {
                vaga.DataVaga = DateTime.Now;
                _db.Vagas.Add(vaga);
                _db.SaveChanges();

                TempData["Mensagem"] = "A vaga foi cadastrada com sucesso!";

                return RedirectToAction("Index");
            }

            return View(vaga);
        }

        [HttpGet]
        public IActionResult Visualizar(int Id) {

            Vaga vaga = _db.Vagas.Find(Id);

            return View(vaga);
        }

    }
}
