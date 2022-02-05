using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BeaHelper.BLL.BD;
using BeaHelper.Library.Filters;
using BeaHelper.Library.Mail;
using BeaHelper.BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;
using System.Text.RegularExpressions;

namespace BeaHelper.Controllers
{
    [Login]
    public class EventoController : Controller
    {

        public IActionResult Index(int? page)
        {
            var pageNumber = page ?? 1;

            List<Evento> eventos = Evento_P2.TodosEventos();

            var resultadoPaginado = eventos.ToPagedList(pageNumber, 10);

            return View(resultadoPaginado);
        }

        [HttpGet]
        public IActionResult Cadastrar()
        {
            ViewBag.CadastrarAtualizar = "Cadastrar";
            return View(new Evento());
        }

        [HttpPost]
        public IActionResult Cadastrar([FromForm] Evento evento)
        {

            ViewBag.CadastrarAtualizar = "Cadastrar";

            //var ValorUsuarioLogado = HttpContext.Session.GetInt32("IdUsuarioLogado"); //Remover ou Comentar
            //int IdUsuarioLog = ValorUsuarioLogado.GetValueOrDefault(); //Remover ou Comentar
            int IdUsuarioLogado = GetUsuarioLogado();

            if (ModelState.IsValid)
            {

                Evento_P1 eventoCadastrar = new Evento_P1();

                eventoCadastrar.DataPublicacao = DateTime.Now;
                if (evento.SemData == true)
                {
                    eventoCadastrar.DataEvento = null;
                }
                else
                {
                    eventoCadastrar.DataEvento = evento.DataEvento;
                }
                
                eventoCadastrar.IdUsuarioAdm = IdUsuarioLogado;
                eventoCadastrar.Titulo = evento.Titulo;
                eventoCadastrar.Categoria = evento.Categoria;
                eventoCadastrar.Descricao = evento.Descricao;
                eventoCadastrar.CidadeEstado = evento.Cidade_Estado;
                eventoCadastrar.SemData = evento.SemData;
                eventoCadastrar.EventoRecorrente = evento.EventoRecorrente;

                eventoCadastrar.Save();

                TempData["Mensagem"] = "O evento foi cadastrado com sucesso!";

                return RedirectToAction("Index");
            }

            return View(evento);
        }

        [HttpGet]
        public IActionResult Visualizar(int Id)
        {

            Evento_P1 evento = new Evento_P1(Id);
            evento.CompleteObject();

            //var IdfUsuarioLogado = HttpContext.Session.GetInt32("IdUsuarioLogado").GetValueOrDefault(); //Remover ou Comentar
            int IdUsuarioLogado = GetUsuarioLogado();

            List<EventoCandidatura> TodasCandidaturasUsuario = EventoCandidaturas_P1.TodasCandidaturasUsuario(IdUsuarioLogado);
            var CandidatosBanco = TodasCandidaturasUsuario.Where(a => a.Id_Usuario == IdUsuarioLogado && a.Id_Evento == Id).FirstOrDefault();

            if (CandidatosBanco != null)
            {
                ViewBag.JaVoluntariado = true;
            }
            if (evento.IdUsuarioAdm == IdUsuarioLogado)
            {
                ViewBag.ADMEvento = true;
            }

            List<Evento> eventos = Evento_P2.EventosParecidosLocal(evento.CidadeEstado, evento.IdEvento);

            List<Evento> eventosParecidas = eventos.Where(x => x.DataEvento != null).ToList();

            List<Evento> eventosParecidosSemData = eventos.Where(x => x.DataEvento == null).ToList();

            foreach (var eventoNoDate in eventosParecidosSemData)
            {
                eventosParecidas.Add(eventoNoDate);
            }

            ViewBag.EventosParecidos = eventosParecidas;
            ViewBag.Local = evento.CidadeEstado.ToUpper();
            return View(evento);
        }

        [HttpGet]
        public IActionResult Editar(int Id)
        {

            //ViewBag.Nivel = niveis;
            //var IdfUsuarioLogado = HttpContext.Session.GetInt32("IdUsuarioLogado").GetValueOrDefault(); //Remover ou Comentar
            int IdUsuarioLogado = GetUsuarioLogado();

            Evento_P1 evento = new Evento_P1(Id);
            evento.CompleteObject();

            if (IdUsuarioLogado == evento.IdUsuarioAdm)
            {
                Mapper.CreateMap<Evento_P1, Evento>();
                Evento EventoEdidar = Mapper.Map<Evento>(evento);

                ViewBag.CadastrarAtualizar = "Salvar";
                return View("Cadastrar", EventoEdidar);
            }

            TempData["MensagemErro"] = "Evento inacessível com seu usuário";
            return RedirectToAction("Index", "Evento");
        }

        [HttpPost]
        public IActionResult Editar([FromForm] Evento evento)
        {

            ViewBag.CadastrarAtualizar = "Salvar";
            //var IdfUsuarioLogado = HttpContext.Session.GetInt32("IdUsuarioLogado").GetValueOrDefault(); //Remover ou Comentar
            int IdUsuarioLogado = GetUsuarioLogado();

            if (ModelState.IsValid)
            {
                Evento_P1 eventos = new Evento_P1(evento.Id_Evento);
                eventos.CompleteObject();

                eventos.DataPublicacao = DateTime.Now;
                eventos.DataEvento = evento.DataEvento;
                eventos.IdUsuarioAdm = IdUsuarioLogado;
                eventos.Titulo = evento.Titulo;
                eventos.Categoria = evento.Categoria;
                eventos.Descricao = evento.Descricao;
                eventos.CidadeEstado = evento.Cidade_Estado;
                eventos.SemData = evento.SemData;
                eventos.EventoRecorrente = evento.EventoRecorrente;

                eventos.Save();

                TempData["Mensagem"] = "O evento foi atualizado com sucesso!";
                return RedirectToAction("MeusEventos", "Perfil");
            }
            return View("Cadastrar", evento);
        }

        [HttpGet]
        public IActionResult Excluir(int Id)
        {
            //var IdfUsuarioLogado = HttpContext.Session.GetInt32("IdUsuarioLogado").GetValueOrDefault(); //Remover ou Comentar
            int IdUsuarioLogado = GetUsuarioLogado();

            if (Id != 0)
            {
                Evento_P1 evento = new Evento_P1(Id);
                evento.CompleteObject();

                if (IdUsuarioLogado == evento.IdUsuarioAdm)
                {
                    bool resultado = Evento_P1.Delete(Id);

                }
            }
            return RedirectToAction("MeusEventos", "Perfil");
        }

        [HttpGet]
        public IActionResult Voluntariar(int Id)
        {
            //var IdfUsuarioLogado = HttpContext.Session.GetInt32("IdUsuarioLogado").GetValueOrDefault(); //Remover ou Comentar
            int IdUsuarioLogado = GetUsuarioLogado();

            Evento_P1 evento = new Evento_P1(Id);
            evento.CompleteObject();

            List<EventoCandidatura> TodasCandidaturasUsuario = EventoCandidaturas_P1.TodasCandidaturasUsuario(IdUsuarioLogado);
            var CandidatosBanco = TodasCandidaturasUsuario.Where(a => a.Id_Usuario == IdUsuarioLogado && a.Id_Evento == Id).FirstOrDefault();

            if (evento.IdUsuarioAdm != IdUsuarioLogado)
            {
                if (CandidatosBanco == null)
                {
                    EventoCandidaturas_P1 candidatarEvento = new EventoCandidaturas_P1();
                    candidatarEvento.IdUsuario = IdUsuarioLogado;
                    candidatarEvento.IdEvento = Id;
                    candidatarEvento.DataCadastro = DateTime.Now;
                    candidatarEvento.Save();

                    ViewBag.JaVoluntariado = true;

                    //Usuario_P1 usuario = new Usuario_P1(IdUsuarioLogado);
                    //usuario.CompleteObject();

                    //bool EmailEnviado = EnviarCandidatoParaDonoEvento(IdUsuarioLogado, evento, usuario);
                }
                else
                {
                    TempData["MensagemErro"] = "Você já está voluntariádo neste evento!";
                    ViewBag.JaVoluntariado = true;
                }
            }
            return Json(Ok());
        }

        public bool EnviarCandidatoParaDonoEvento(int IdUsuarioLogado, Evento_P1 evento, Usuario_P1 usuario)
        {
            try
            {
                if (usuario.Sexo == 1)
                {
                    ViewBag.UsuarioSexo = "Masculino";
                }
                else if (usuario.Sexo == 2)
                {
                    ViewBag.UsuarioSexo = "Feminino";
                }
                else
                {
                    ViewBag.UsuarioSexo = "Prefiro não declarar";
                }

                Usuario_P1 usuarioAdm = new Usuario_P1(evento.IdUsuarioAdm);
                usuarioAdm.CompleteObject();

                EnviarEmail.EnviarMensagemContato(usuario, usuarioAdm.Email, evento.IdEvento);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        [HttpGet]
        public IActionResult ListaVoluntarios(int Id)
        {
            ViewBag.FooterPrecisa = false;
            ViewBag.IdEvento = Id;
            int IdUsuarioLogado = GetUsuarioLogado();

            List<EventoCandidatura> ListaUsuariosVoluntariados = EventoCandidaturas_P1.TodasUsuarioCandidatadosEvento(Id);

            List<UsuarioCompleto> voluntariosCompleto = new List<UsuarioCompleto>();
            List<int> IdfVoluntarios = new List<int>();

            //Lista todos ID dos usuários candidatados
            for (int i = 0; i < ListaUsuariosVoluntariados.Count; i++)
            {
                var idf = ListaUsuariosVoluntariados[i].Id_Usuario;
                IdfVoluntarios.Add(idf);
            }

            foreach (var IdUsu in IdfVoluntarios)
            {
                UsuarioCompleto UsuarioCompleto = new UsuarioCompleto();

                Usuario_P1 Usuario = new Usuario_P1(IdUsu);
                Usuario.CompleteObject();

                var JaAvaliado = Avaliacao_P1.BuscaIdUsuario_AvaliouEAvaliado(IdUsu, IdUsuarioLogado);
                
                var Avaliacao = Avaliacao_P1.TodasAvaliacoesUsuario(IdUsu);
                UsuarioCompleto.Id = Usuario.IdUsuario;
                UsuarioCompleto.Email = Usuario.Email;

                Regex regex = new Regex(@"\D"); //Regex para remover tudo oque não for número
                string numeroWhatapp = regex.Replace(Usuario.NumeroCelular, @"");

                UsuarioCompleto.NumeroCelular = numeroWhatapp;

                UsuarioCompleto.Nome = Usuario.Nome;
                UsuarioCompleto.Sexo = Usuario.Sexo;

                if (JaAvaliado.Count > 0)
                {
                    UsuarioCompleto.UsuarioLogadoAvaliou = true;
                }
                else
                {
                    UsuarioCompleto.UsuarioLogadoAvaliou = false;
                }

                if (Avaliacao.Count > 0)
                {
                    double NotaSomadas = 0;
                    for (int i = 0; i < Avaliacao.Count; i++)
                    {
                        NotaSomadas += Avaliacao[i].Nota;
                    }
                    var media = NotaSomadas / Avaliacao.Count;
                    media = Math.Round(media, 1);
                    UsuarioCompleto.NotaMedia = media;

                    UsuarioCompleto.NuncaAvaliado = false;
                }
                else
                {
                    UsuarioCompleto.NuncaAvaliado = true;
                }

                voluntariosCompleto.Add(UsuarioCompleto);
            }

            var pageNumber = 1;

            var resultadoPaginado = voluntariosCompleto.ToPagedList(pageNumber, 10);

            return View(resultadoPaginado);

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
