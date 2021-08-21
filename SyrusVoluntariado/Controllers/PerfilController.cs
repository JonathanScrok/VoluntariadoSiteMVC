using Microsoft.AspNetCore.Http;
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
    public class PerfilController : Controller {

        private DatabaseContext _db;

        public PerfilController(DatabaseContext db) {
            _db = db;
        }

        public IActionResult Index() {
            ViewBag.FooterPrecisa = false;
            int IdfUsuario = HttpContext.Session.GetInt32("IdUsuarioLogado").GetValueOrDefault();
            Usuario usuario = _db.Usuarios.Find(IdfUsuario);

            return View(usuario);
        }

        public IActionResult MinhasVagas() {
            ViewBag.FooterPrecisa = false;
            int IdfUsuario = HttpContext.Session.GetInt32("IdUsuarioLogado").GetValueOrDefault();
            var VagasUsuario = _db.Vagas.Where(a => a.Idf_Usuario_Adm == IdfUsuario).ToList();

            return View(VagasUsuario);
        }

        public IActionResult VagasCandidatadas() {
            ViewBag.FooterPrecisa = false;
            int IdfUsuario = HttpContext.Session.GetInt32("IdUsuarioLogado").GetValueOrDefault();
            var IdVagasCandidatadas = _db.VagaCandidaturas.Where(a => a.Idf_Usuario_Candidatado == IdfUsuario).ToList();
            List<Vaga> CarregaVagasCandidatadas = null;
            List<Vaga> MinhasCandidaturas = new List<Vaga>();
            List<int> Idfvagas = new List<int>();

            for (int i = 0; i < IdVagasCandidatadas.Count; i++) {
                var idf = IdVagasCandidatadas[i].Idf_Vaga;
                Idfvagas.Add(idf);
            }

            foreach (var Id in Idfvagas) {
                CarregaVagasCandidatadas = _db.Vagas.Where(a => a.Id == Id).ToList();
                MinhasCandidaturas.Add(CarregaVagasCandidatadas[0]);
            }
            
            return View(MinhasCandidaturas);
        }
    }
}
