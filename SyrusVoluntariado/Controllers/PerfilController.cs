using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SyrusVoluntariado.BLL;
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

            int IdfUsuario = HttpContext.Session.GetInt32("IdUsuarioLogado").GetValueOrDefault();

            Usuario_P1 Usuario = new Usuario_P1(IdfUsuario);
            Usuario.CompleteObject();

            return View(Usuario);
        }

        public IActionResult MinhasVagas() {
            ViewBag.FooterPrecisa = false;
            int IdfUsuario = HttpContext.Session.GetInt32("IdUsuarioLogado").GetValueOrDefault();

            List<Vaga> vagas = Vaga_P1.TodasVagas();
            var VagasUsuario = vagas.Where(a => a.Id_Usuario_Adm == IdfUsuario).ToList();

            return View(VagasUsuario);
        }

        public IActionResult VagasCandidatadas() {
            ViewBag.FooterPrecisa = false;
            int IdfUsuario = HttpContext.Session.GetInt32("IdUsuarioLogado").GetValueOrDefault();
            var IdVagasCandidatadas = _db.VagaCandidaturas.Where(a => a.Idf_Usuario_Candidatado == IdfUsuario).ToList();

            List<Vaga> CarregaVagasCandidatadas = null;
            List<Vaga> MinhasCandidaturas = new List<Vaga>();
            List<int> Idfvagas = new List<int>();
            List<Vaga> vagas = Vaga_P1.TodasVagas();

            for (int i = 0; i < IdVagasCandidatadas.Count; i++) {
                var idf = IdVagasCandidatadas[i].Idf_Vaga;
                Idfvagas.Add(idf);
            }

            foreach (var Id in Idfvagas) {
                CarregaVagasCandidatadas = vagas.Where(a => a.Id_Vaga == Id).ToList();
                MinhasCandidaturas.Add(CarregaVagasCandidatadas[0]);
            }
            
            return View(MinhasCandidaturas);
        }
    }
}
