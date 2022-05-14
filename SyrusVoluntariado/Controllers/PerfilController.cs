﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BeaHelper.BLL.BD;
using BeaHelper.Library.Filters;
using BeaHelper.BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeaHelper.Controllers
{
    [Login]
    public class PerfilController : Controller
    {

        public IActionResult Index()
        {

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

            // Meus Eventos
            List<Evento> meuseventos = Evento_P2.MeusEventos(IdUsuarioLogado);
            ViewBag.MeusEventosCriados = meuseventos;
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
            ViewBag.Filtro = new Filtro();
            ViewBag.EventosCandidatados = MinhasCandidaturas;
            // --------------------------------------------------

            return View(Usuario);
        }

        [HttpGet]
        public IActionResult MeusEventos(bool Convidando = false, int IdVoluntarioConvidado = 0)
        {

            ViewBag.ConvidarAtivado = Convidando;
            ViewBag.IdVoluntarioConvidado = IdVoluntarioConvidado;

            ViewBag.FooterPrecisa = false;
            int IdUsuarioLogado = GetUsuarioLogado();

            List<Evento> meuseventos = Evento_P2.MeusEventos(IdUsuarioLogado);

            return View(meuseventos);
        }

        [HttpGet]
        public IActionResult EventosCandidatados()
        {
            ViewBag.FooterPrecisa = false;

            List<Evento_P1> MinhasCandidaturas = CarregaEventosCandidatados();

            ViewBag.Filtro = new Filtro();
            return View(MinhasCandidaturas);
        }

        private List<Evento_P1> CarregaEventosCandidatados()
        {
            int IdUsuarioLogado = GetUsuarioLogado();
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

            return MinhasCandidaturas;
        }

        [HttpPost]
        public IActionResult FiltroEventosCandidatados(Filtro filtros)
        {
            List<Evento_P1> MinhasCandidaturas = new List<Evento_P1>();
            List<Evento_P1> FiltroMinhasCandidaturas = new List<Evento_P1>();

            MinhasCandidaturas = CarregaEventosCandidatados();

            FiltroMinhasCandidaturas = FiltroCadidaturas(MinhasCandidaturas, filtros);

            ViewBag.Filtro = filtros;
            return View("EventosCandidatados", FiltroMinhasCandidaturas);
        }

        private List<Evento_P1> FiltroCadidaturas(List<Evento_P1> MinhasCandidaturas, Filtro filtros)
        {
            List<Evento_P1> CandidatosBanco = new List<Evento_P1>();

            if (filtros.Titulo != null)
            {
                MinhasCandidaturas = MinhasCandidaturas.Where(a => a.Titulo.Contains(filtros.Titulo)).ToList();
            }
            if (filtros.Descricao != null)
            {
                MinhasCandidaturas = MinhasCandidaturas.Where(a => a.Descricao.Contains(filtros.Descricao)).ToList();
            }
            if (filtros.Categoria != null)
            {
                MinhasCandidaturas = MinhasCandidaturas.Where(a => a.Categoria.Contains(filtros.Categoria)).ToList();
            }
            if (filtros.Local != null)
            {
                MinhasCandidaturas = MinhasCandidaturas.Where(a => a.CidadeEstado.Contains(filtros.Local)).ToList();
            }

            return MinhasCandidaturas;
        }

        public int GetUsuarioLogado()
        {
            int IdUsuarioLogado = 0;

            try
            {
                if (HttpContext.Request.Cookies["IdUsuarioLogado"] != null)
                {
                    IdUsuarioLogado = Int32.Parse(HttpContext.Request.Cookies["IdUsuarioLogado"]);
                }
                else if (HttpContext.Session.GetInt32("IdUsuarioLogado") != null)
                {
                    IdUsuarioLogado = HttpContext.Session.GetInt32("IdUsuarioLogado").GetValueOrDefault();
                }
                else
                {
                    IdUsuarioLogado = 0;
                }
            }
            catch (Exception)
            {
                IdUsuarioLogado = 0;
            }

            return IdUsuarioLogado;
        }

    }
}
