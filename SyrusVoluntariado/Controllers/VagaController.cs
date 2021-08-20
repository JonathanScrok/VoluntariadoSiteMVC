﻿using Microsoft.AspNetCore.Http;
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

            var vagas = _db.Vagas.ToList();

            var resultadoPaginado = vagas.ToPagedList(pageNumber, 10);

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
            var ValorUsuarioLogado = HttpContext.Session.GetInt32("IdUsuarioLogado");
            int IdUsuarioLogado = ValorUsuarioLogado.GetValueOrDefault();

            if (ModelState.IsValid) {

                vaga.DataVaga = DateTime.Now;
                vaga.IdfUsuario = IdUsuarioLogado;

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


        [HttpGet]
        public IActionResult Editar(int Id) {

            //ViewBag.Nivel = niveis;
            var IdfUsuarioLogado = HttpContext.Session.GetInt32("IdUsuarioLogado").GetValueOrDefault();
            
            Vaga vaga = _db.Vagas.Find(Id);

            if (IdfUsuarioLogado == vaga.IdfUsuario) {
                return View("Cadastrar", vaga);
            }

            TempData["MensagemErro"] = "Vaga inacessível com seu usuário";
            return RedirectToAction("Index", "Vaga");
        }

        [HttpPost]
        public IActionResult Editar([FromForm] Vaga vaga) {

            //ViewBag.Nivel = niveis;
            var IdfUsuarioLogado = HttpContext.Session.GetInt32("IdUsuarioLogado").GetValueOrDefault();

            if (ModelState.IsValid) {

                vaga.DataVaga = DateTime.Now;
                vaga.IdfUsuario = IdfUsuarioLogado;
                _db.Vagas.Update(vaga);
                _db.SaveChanges();

                return RedirectToAction("MinhasVagas", "Perfil");
            }
            return View("Cadastrar", vaga);
        }

        [HttpGet]
        public IActionResult Excluir(int Id) {

            var IdfUsuarioLogado = HttpContext.Session.GetInt32("IdUsuarioLogado").GetValueOrDefault();

            Vaga vaga = _db.Vagas.Find(Id);

            if (IdfUsuarioLogado == vaga.IdfUsuario) {
                _db.Vagas.Remove(vaga);
                _db.SaveChanges();

                TempData["Mensagem"] = "A palavra foi excluida com sucesso!";
            }

            return RedirectToAction("MinhasVagas", "Perfil");
        }
    }
}
