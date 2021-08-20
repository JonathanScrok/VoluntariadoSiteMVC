using Microsoft.AspNetCore.Http;
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
            ViewBag.CadastrarAtualizar = "Cadastrar";

            return View(new Vaga());
        }

        [HttpPost]
        public IActionResult Cadastrar([FromForm] Vaga vaga) {

            //ViewBag.Nivel = niveis;
            var ValorUsuarioLogado = HttpContext.Session.GetInt32("IdUsuarioLogado");
            int IdUsuarioLogado = ValorUsuarioLogado.GetValueOrDefault();

            if (ModelState.IsValid) {

                vaga.DataVaga = DateTime.Now;
                vaga.Idf_Usuario_Adm = IdUsuarioLogado;

                _db.Vagas.Add(vaga);
                _db.SaveChanges();

                TempData["Mensagem"] = "O evento foi cadastrada com sucesso!";

                return RedirectToAction("Index");
            }

            return View(vaga);
        }

        [HttpGet]
        public IActionResult Visualizar(int Id) {

            var IdfUsuarioLogado = HttpContext.Session.GetInt32("IdUsuarioLogado").GetValueOrDefault();
            Vaga vaga = _db.Vagas.Find(Id);
            VagaCandidatura vagaCandidatura = _db.VagaCandidaturas.Find(Id);

            var CandidatosBanco = _db.VagaCandidaturas.Where(a => a.Idf_Usuario_Candidatado == IdfUsuarioLogado && a.Idf_Vaga == Id).FirstOrDefault();

            if (CandidatosBanco != null) {
                ViewBag.JaVoluntariado = true;
            }
            if (vaga.Idf_Usuario_Adm == IdfUsuarioLogado) {
                ViewBag.ADMVaga = true;
            }

            return View(vaga);
        }


        [HttpGet]
        public IActionResult Editar(int Id) {

            //ViewBag.Nivel = niveis;
            var IdfUsuarioLogado = HttpContext.Session.GetInt32("IdUsuarioLogado").GetValueOrDefault();

            Vaga vaga = _db.Vagas.Find(Id);

            if (IdfUsuarioLogado == vaga.Idf_Usuario_Adm) {
                ViewBag.CadastrarAtualizar = "Salvar";
                return View("Cadastrar", vaga);
            }

            TempData["MensagemErro"] = "Evento inacessível com seu usuário";
            return RedirectToAction("Index", "Vaga");
        }

        [HttpPost]
        public IActionResult Editar([FromForm] Vaga vaga) {

            //ViewBag.Nivel = niveis;
            var IdfUsuarioLogado = HttpContext.Session.GetInt32("IdUsuarioLogado").GetValueOrDefault();

            if (ModelState.IsValid) {

                vaga.DataVaga = DateTime.Now;
                vaga.Idf_Usuario_Adm = IdfUsuarioLogado;
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

            if (IdfUsuarioLogado == vaga.Idf_Usuario_Adm) {
                _db.Vagas.Remove(vaga);
                _db.SaveChanges();
            }

            return RedirectToAction("MinhasVagas", "Perfil");
        }

        [HttpGet]
        public IActionResult Voluntariar(int Id) {
            var IdfUsuarioLogado = HttpContext.Session.GetInt32("IdUsuarioLogado").GetValueOrDefault();

            Vaga vaga = _db.Vagas.Find(Id);
            var CandidatosBanco = _db.VagaCandidaturas.Where(a => a.Idf_Usuario_Candidatado == IdfUsuarioLogado && a.Idf_Vaga == Id).FirstOrDefault();

            if (vaga.Idf_Usuario_Adm != IdfUsuarioLogado) {
                if (CandidatosBanco == null) {
                    VagaCandidatura VagaCandidatada = new VagaCandidatura();
                    VagaCandidatada.Idf_Vaga = Id;
                    VagaCandidatada.Idf_Usuario_Candidatado = IdfUsuarioLogado;

                    _db.VagaCandidaturas.Add(VagaCandidatada);
                    _db.SaveChanges();
                } else {
                    // Avisar em um DataTemp que já esta cadastrado!
                    TempData["MensagemErro"] = "Você já está cadastrado nesta vaga!";
                    ViewBag.JaVoluntariado = true;
                }
            }

            return RedirectToAction("Visualizar", "Vaga", vaga);
        }
    }
}
