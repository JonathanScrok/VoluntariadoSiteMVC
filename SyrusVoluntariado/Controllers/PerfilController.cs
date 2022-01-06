using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BeaHelper.BLL.BD;
using BeaHelper.Library.Filters;
using BeaHelper.BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeaHelper.Controllers {
    [Login]
    public class PerfilController : Controller {

        public IActionResult Index() {

            //int IdfUsuario = HttpContext.Session.GetInt32("IdUsuarioLogado").GetValueOrDefault(); //Remover ou Comentar
            int IdUsuarioLogado = GetUsuarioLogado();

            Usuario_P1 Usuario = new Usuario_P1(IdUsuarioLogado);
            Usuario.CompleteObject();

            if (Usuario.Sexo == 1)
            {
                ViewBag.UsuarioSexo = "Masculino";
            }
            else if (Usuario.Sexo == 2)
            {
                ViewBag.UsuarioSexo = "Feminino";
            }
            else
            {
                ViewBag.UsuarioSexo = "Prefiro não declarar";
            }

            // Minhas Vagas
            List<Vaga> minhasvagas = Vaga_P2.MinhasVagas(IdUsuarioLogado);
            ViewBag.VagasCandidatas = minhasvagas;
            // -------------------------------------------------

            // Vagas Candidatadas
            List<VagaCandidatura> VagasCandidatadas = VagaCandidaturas_P1.TodasCandidaturasUsuario(IdUsuarioLogado);

            List<Vaga_P1> MinhasCandidaturas = new List<Vaga_P1>();
            List<int> Idfvagas = new List<int>();

            for (int i = 0; i < VagasCandidatadas.Count; i++)
            {
                var idf = VagasCandidatadas[i].Id_Vaga;
                Idfvagas.Add(idf);
            }

            foreach (var Id in Idfvagas)
            {
                Vaga_P1 vaga = new Vaga_P1(Id);
                vaga.CompleteObject();
                MinhasCandidaturas.Add(vaga);
            }
            ViewBag.MinhasVagas = MinhasCandidaturas;
            // --------------------------------------------------

            return View(Usuario);
        }

        [HttpGet]
        public IActionResult MinhasVagas() {
            ViewBag.FooterPrecisa = false;
            int IdUsuarioLogado = GetUsuarioLogado();

            List<Vaga> minhasvagas = Vaga_P2.MinhasVagas(IdUsuarioLogado);

            return View(minhasvagas);
        }

        [HttpGet]
        public IActionResult VagasCandidatadas() {
            ViewBag.FooterPrecisa = false;

            //int IdfUsuario = HttpContext.Session.GetInt32("IdUsuarioLogado").GetValueOrDefault(); //Remover ou Comentar
            int IdUsuarioLogado = GetUsuarioLogado();

            List<VagaCandidatura> VagasCandidatadas = VagaCandidaturas_P1.TodasCandidaturasUsuario(IdUsuarioLogado);

            List<Vaga_P1> MinhasCandidaturas = new List<Vaga_P1>();
            List<int> Idfvagas = new List<int>();

            //List<Vaga> vagas = Vaga_P1.TodasVagas();

            for (int i = 0; i < VagasCandidatadas.Count; i++)
            {
                var idf = VagasCandidatadas[i].Id_Vaga;
                Idfvagas.Add(idf);
            }

            foreach (var Id in Idfvagas)
            {
                Vaga_P1 vaga = new Vaga_P1(Id);
                vaga.CompleteObject();
                MinhasCandidaturas.Add(vaga);
            }

            return View(MinhasCandidaturas);
        }

        public int GetUsuarioLogado()
        {
            int IdUsuarioLogado = 0;
            try
            {
                IdUsuarioLogado = Int32.Parse(HttpContext.Request.Cookies["IdUsuarioLogado"]);
            }
            catch (Exception)
            {
                IdUsuarioLogado = HttpContext.Session.GetInt32("IdUsuarioLogado").GetValueOrDefault();
            }

            return IdUsuarioLogado;
        }

    }
}
