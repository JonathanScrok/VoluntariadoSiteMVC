using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SyrusVoluntariado.BLL;
using SyrusVoluntariado.Database;
using SyrusVoluntariado.Library.Filters;
using SyrusVoluntariado.Library.Mail;
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

            //var vagas = _db.Vagas.ToList();
            List<Vaga> vagas = Vaga_P1.TodasVagas();

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

            ViewBag.CadastrarAtualizar = "Cadastrar";
            var ValorUsuarioLogado = HttpContext.Session.GetInt32("IdUsuarioLogado");
            int IdUsuarioLogado = ValorUsuarioLogado.GetValueOrDefault();

            Vaga_P1 vagas = new Vaga_P1();
            vagas.CompleteObject();

            if (ModelState.IsValid) {

                vagas.DataVaga = DateTime.Now;
                vagas.IdUsuarioAdm = IdUsuarioLogado;
                vagas.Titulo = vaga.Titulo;
                vagas.Categoria = vaga.Categoria;
                vagas.Descricao = vaga.Descricao;
                vagas.CidadeEstado = vaga.Cidade_Estado;

                vagas.Save();

                TempData["Mensagem"] = "O evento foi cadastrada com sucesso!";

                return RedirectToAction("Index");
            }

            return View(vaga);
        }

        [HttpGet]
        public IActionResult Visualizar(int Id) {

            Vaga_P1 vaga = new Vaga_P1(Id);
            vaga.CompleteObject();

            var IdfUsuarioLogado = HttpContext.Session.GetInt32("IdUsuarioLogado").GetValueOrDefault();

            //Vaga vaga = _db.Vagas.Find(Id);
            VagaCandidatura vagaCandidatura = _db.VagaCandidaturas.Find(Id);
            var CandidatosBanco = _db.VagaCandidaturas.Where(a => a.Idf_Usuario_Candidatado == IdfUsuarioLogado && a.Idf_Vaga == Id).FirstOrDefault();

            if (CandidatosBanco != null) {
                ViewBag.JaVoluntariado = true;
            }
            if (vaga.IdUsuarioAdm == IdfUsuarioLogado) {
                ViewBag.ADMVaga = true;
            }

            return View(vaga);
        }


        [HttpGet]
        public IActionResult Editar(int Id) {

            //ViewBag.Nivel = niveis;
            var IdfUsuarioLogado = HttpContext.Session.GetInt32("IdUsuarioLogado").GetValueOrDefault();

            Vaga_P1 vaga = new Vaga_P1(Id);
            vaga.CompleteObject();

            if (IdfUsuarioLogado == vaga.IdUsuarioAdm) {
                ViewBag.CadastrarAtualizar = "Salvar";
                return View("Cadastrar", vaga);
            }

            TempData["MensagemErro"] = "Evento inacessível com seu usuário";
            return RedirectToAction("Index", "Vaga");
        }

        [HttpPost]
        public IActionResult Editar([FromForm] Vaga vaga) {

            ViewBag.CadastrarAtualizar = "Salvar";
            var IdfUsuarioLogado = HttpContext.Session.GetInt32("IdUsuarioLogado").GetValueOrDefault();

            Vaga_P1 vagas = new Vaga_P1(vaga.Id_Vaga);
            vagas.CompleteObject();

            if (ModelState.IsValid) {

                vagas.DataVaga = DateTime.Now;
                vagas.IdUsuarioAdm = IdfUsuarioLogado;
                vagas.Titulo = vaga.Titulo;
                vagas.Categoria = vaga.Categoria;
                vagas.Descricao = vaga.Descricao;
                vagas.CidadeEstado = vaga.Cidade_Estado;

                vagas.Save();

                TempData["Mensagem"] = "O evento foi atualizado com sucesso!";
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
            Usuario usuario = _db.Usuarios.Find(IdfUsuarioLogado);
            Usuario usuarioAdm = _db.Usuarios.Find(vaga.Idf_Usuario_Adm);

            var CandidatosBanco = _db.VagaCandidaturas.Where(a => a.Idf_Usuario_Candidatado == IdfUsuarioLogado && a.Idf_Vaga == Id).FirstOrDefault();

            if (vaga.Idf_Usuario_Adm != IdfUsuarioLogado) {
                if (CandidatosBanco == null) {
                    VagaCandidatura VagaCandidatada = new VagaCandidatura();
                    VagaCandidatada.Idf_Vaga = Id;
                    VagaCandidatada.Idf_Usuario_Candidatado = IdfUsuarioLogado;

                    _db.VagaCandidaturas.Add(VagaCandidatada);
                    _db.SaveChanges();

                    EnviarEmail.EnviarMensagemContato(usuario, usuarioAdm.Email, Id);
                } else {
                    TempData["MensagemErro"] = "Você já está cadastrado neste evento!";
                    ViewBag.JaVoluntariado = true;
                }
            }

            return RedirectToAction("Visualizar", "Vaga", vaga);
        }

        [HttpGet]
        public IActionResult ListaVoluntarios(int Id) {
            ViewBag.FooterPrecisa = false;

            var ListaVoluntarios = _db.VagaCandidaturas.Where(a => a.Idf_Vaga == Id).ToList();
            List<Usuario> CarregaVagasCandidatadas = null;

            List<Usuario> voluntarios = new List<Usuario>();
            List<int> IdfVoluntarios = new List<int>();

            for (int i = 0; i < ListaVoluntarios.Count; i++) {
                var idf = ListaVoluntarios[i].Idf_Usuario_Candidatado;
                IdfVoluntarios.Add(idf);
            }

            foreach (var IdUsu in IdfVoluntarios) {
                CarregaVagasCandidatadas = _db.Usuarios.Where(a => a.Id == IdUsu).ToList();
                voluntarios.Add(CarregaVagasCandidatadas[0]);
            }

            var pageNumber = 1;

            var resultadoPaginado = voluntarios.ToPagedList(pageNumber, 10);

            return View(resultadoPaginado);

        }


    }
}
