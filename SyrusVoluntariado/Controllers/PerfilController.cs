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

            // Minhas Eventos
            List<Evento> minhasvagas = Evento_P2.MinhasVagas(IdUsuarioLogado);
            ViewBag.VagasCandidatas = minhasvagas;
            // -------------------------------------------------

            // Eventos Candidatadas
            List<EventoCandidatura> EventosCandidatados = EventoCandidaturas_P1.TodasCandidaturasUsuario(IdUsuarioLogado);

            List<Evento_P1> MinhasCandidaturas = new List<Evento_P1>();
            List<int> Idfvagas = new List<int>();

            for (int i = 0; i < EventosCandidatados.Count; i++)
            {
                var idf = EventosCandidatados[i].Id_Evento;
                Idfvagas.Add(idf);
            }

            foreach (var Id in Idfvagas)
            {
                Evento_P1 evento = new Evento_P1(Id);
                evento.CompleteObject();
                MinhasCandidaturas.Add(evento);
            }
            ViewBag.MinhasVagas = MinhasCandidaturas;
            // --------------------------------------------------

            return View(Usuario);
        }

        [HttpGet]
        public IActionResult MeusEventos() {
            ViewBag.FooterPrecisa = false;
            int IdUsuarioLogado = GetUsuarioLogado();

            List<Evento> meuseventos = Evento_P2.MinhasVagas(IdUsuarioLogado);

            return View(meuseventos);
        }

        [HttpGet]
        public IActionResult EventosCandidatados() {
            ViewBag.FooterPrecisa = false;

            //int IdfUsuario = HttpContext.Session.GetInt32("IdUsuarioLogado").GetValueOrDefault(); //Remover ou Comentar
            int IdUsuarioLogado = GetUsuarioLogado();

            List<EventoCandidatura> EventosCandidatados = EventoCandidaturas_P1.TodasCandidaturasUsuario(IdUsuarioLogado);

            List<Evento_P1> MinhasCandidaturas = new List<Evento_P1>();
            List<int> Idfvagas = new List<int>();

            //List<Evento> vagas = Evento_P1.TodasVagas();

            for (int i = 0; i < EventosCandidatados.Count; i++)
            {
                var idf = EventosCandidatados[i].Id_Evento;
                Idfvagas.Add(idf);
            }

            foreach (var Id in Idfvagas)
            {
                Evento_P1 evento = new Evento_P1(Id);
                evento.CompleteObject();
                MinhasCandidaturas.Add(evento);
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
