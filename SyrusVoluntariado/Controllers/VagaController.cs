﻿using Microsoft.AspNetCore.Http;
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

namespace SyrusVoluntariado.Controllers
{
    [Login]
    public class VagaController : Controller
    {

        private DatabaseContext _db;

        public VagaController(DatabaseContext db)
        {
            _db = db;
        }

        public IActionResult Index(int? page)
        {
            var pageNumber = page ?? 1;

            List<Vaga> vagas = Vaga_P1.TodasVagas();

            var resultadoPaginado = vagas.ToPagedList(pageNumber, 10);

            return View(resultadoPaginado);
        }

        [HttpGet]
        public IActionResult Cadastrar()
        {
            ViewBag.CadastrarAtualizar = "Cadastrar";

            return View(new Vaga_P1());
        }

        [HttpPost]
        public IActionResult Cadastrar([FromForm] Vaga vaga)
        {

            ViewBag.CadastrarAtualizar = "Cadastrar";
            var ValorUsuarioLogado = HttpContext.Session.GetInt32("IdUsuarioLogado");
            int IdUsuarioLogado = ValorUsuarioLogado.GetValueOrDefault();

            if (ModelState.IsValid)
            {

                Vaga_P1 vagas = new Vaga_P1();
                vagas.CompleteObject();

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
        public IActionResult Visualizar(int Id)
        {

            Vaga_P1 vaga = new Vaga_P1(Id);
            vaga.CompleteObject();

            var IdfUsuarioLogado = HttpContext.Session.GetInt32("IdUsuarioLogado").GetValueOrDefault();

            List<VagaCandidatura> TodasCandidaturasUsuario = VagaCandidaturas_P1.TodasCandidaturasUsuario(IdfUsuarioLogado);
            var CandidatosBanco = TodasCandidaturasUsuario.Where(a => a.Id_Usuario == IdfUsuarioLogado && a.Id_Vaga == Id).FirstOrDefault();

            if (CandidatosBanco != null)
            {
                ViewBag.JaVoluntariado = true;
            }
            if (vaga.IdUsuarioAdm == IdfUsuarioLogado)
            {
                ViewBag.ADMVaga = true;
            }

            return View(vaga);
        }

        [HttpGet]
        public IActionResult Editar(int Id)
        {

            //ViewBag.Nivel = niveis;
            var IdfUsuarioLogado = HttpContext.Session.GetInt32("IdUsuarioLogado").GetValueOrDefault();

            Vaga_P1 vaga = new Vaga_P1(Id);
            vaga.CompleteObject();

            if (IdfUsuarioLogado == vaga.IdUsuarioAdm)
            {
                ViewBag.CadastrarAtualizar = "Salvar";
                return View("Cadastrar", vaga);
            }

            TempData["MensagemErro"] = "Evento inacessível com seu usuário";
            return RedirectToAction("Index", "Vaga");
        }

        [HttpPost]
        public IActionResult Editar([FromForm] Vaga vaga)
        {

            ViewBag.CadastrarAtualizar = "Salvar";
            var IdfUsuarioLogado = HttpContext.Session.GetInt32("IdUsuarioLogado").GetValueOrDefault();

            if (ModelState.IsValid)
            {
                Vaga_P1 vagas = new Vaga_P1(vaga.Id_Vaga);
                vagas.CompleteObject();

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
        public IActionResult Excluir(int Id)
        {

            var IdfUsuarioLogado = HttpContext.Session.GetInt32("IdUsuarioLogado").GetValueOrDefault();

            if (Id != 0)
            {
                Vaga_P1 vaga = new Vaga_P1(Id);
                vaga.CompleteObject();

                if (IdfUsuarioLogado == vaga.IdUsuarioAdm)
                {
                    bool resultado = Vaga_P1.Delete(Id);

                }
            }
            return RedirectToAction("MinhasVagas", "Perfil");
        }

        [HttpGet]
        public IActionResult Voluntariar(int Id)
        {
            var IdfUsuarioLogado = HttpContext.Session.GetInt32("IdUsuarioLogado").GetValueOrDefault();

            Vaga_P1 vaga = new Vaga_P1(Id);
            vaga.CompleteObject();

            Usuario_P1 usuario = new Usuario_P1(IdfUsuarioLogado);
            usuario.CompleteObject();

            Usuario_P1 usuarioAdm = new Usuario_P1(vaga.IdUsuarioAdm);
            usuarioAdm.CompleteObject();

            List<VagaCandidatura> TodasCandidaturasUsuario = VagaCandidaturas_P1.TodasCandidaturasUsuario(IdfUsuarioLogado);
            var CandidatosBanco = TodasCandidaturasUsuario.Where(a => a.Id_Usuario == IdfUsuarioLogado && a.Id_Vaga == Id).FirstOrDefault();

            if (vaga.IdUsuarioAdm != IdfUsuarioLogado)
            {
                if (CandidatosBanco == null)
                {
                    VagaCandidaturas_P1 candidatarVaga = new VagaCandidaturas_P1();
                    candidatarVaga.IdUsuario = IdfUsuarioLogado;
                    candidatarVaga.IdVaga = Id;
                    candidatarVaga.DataCadastro = DateTime.Now;
                    candidatarVaga.Save();

                    ViewBag.JaVoluntariado = true;
                    EnviarEmail.EnviarMensagemContato(usuario, usuarioAdm.Email, Id);
                }
                else
                {
                    TempData["MensagemErro"] = "Você já está cadastrado neste evento!";
                    ViewBag.JaVoluntariado = true;
                }
            }

            return View("Visualizar", vaga);
        }

        [HttpGet]
        public IActionResult ListaVoluntarios(int Id)
        {
            ViewBag.FooterPrecisa = false;

            var ListaVoluntarios = _db.VagaCandidaturas.Where(a => a.Id_Vaga == Id).ToList();
            List<Usuario> CarregaVagasCandidatadas = null;

            List<Usuario> voluntarios = new List<Usuario>();
            List<int> IdfVoluntarios = new List<int>();

            for (int i = 0; i < ListaVoluntarios.Count; i++)
            {
                var idf = ListaVoluntarios[i].Id_Usuario;
                IdfVoluntarios.Add(idf);
            }

            foreach (var IdUsu in IdfVoluntarios)
            {
                CarregaVagasCandidatadas = _db.Usuarios.Where(a => a.Id == IdUsu).ToList();
                voluntarios.Add(CarregaVagasCandidatadas[0]);
            }

            var pageNumber = 1;

            var resultadoPaginado = voluntarios.ToPagedList(pageNumber, 10);

            return View(resultadoPaginado);

        }


    }
}
