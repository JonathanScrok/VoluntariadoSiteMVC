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
using BeaHelper.BLL.Database;

namespace BeaHelper.Controllers
{
    public class EventoController : Controller
    {
        [HttpGet]
        public IActionResult Index(int? page)
        {
            int pageNumber = page ?? 1;

            List<Evento> eventos = Evento_P2.TodosEventos();
            List<Evento> eventosFinal = new List<Evento>();
            Parallel.ForEach(eventos, evento =>
            {
                eventosFinal.Add(evento);
            });

            Parallel.ForEach(eventos, evento =>
            {
                if (evento.SemData)
                {
                    var dataMais2Meses = evento.DataPublicacao.AddMonths(2);
                    if (evento.DataPublicacao.AddMonths(2) < DateTime.Now)
                    {
                        eventosFinal.Remove(evento);
                    }
                }
            });
            eventosFinal[0].Filtros = new Filtro();
            var resultadoPaginado = eventosFinal.ToPagedList(pageNumber, 10);

            ViewBag.idusuario = GetUsuarioLogado();
            return View(resultadoPaginado);
        }

        [HttpPost]
        public IActionResult FiltrarEvento(Filtro Filtro)
        {
            List<Evento> eventos = new List<Evento>();
            IPagedList<Evento> resultadoPaginado;
            eventos = Evento_P2.FiltrarEventos(Filtro.Titulo, Filtro.Descricao, Filtro.Categoria, Filtro.Local);

            if (eventos.Count == 0)
            {
                Evento evento = new Evento();
                evento.Filtros = Filtro;
                eventos.Add(evento);
            }

            ViewBag.idusuario = GetUsuarioLogado();
            eventos[0].Filtros = Filtro;
            resultadoPaginado = eventos.ToPagedList(1, 10);
            return View("Index", resultadoPaginado);
        }

        [HttpGet]
        public IActionResult OrdenarEventos(string ordenarPor)
        {
            int pageNumber = 1;

            List<Evento> eventos = Evento_P2.TodosEventos();
            List<Evento> eventosFinal = new List<Evento>();
            Parallel.ForEach(eventos, evento =>
            {
                eventosFinal.Add(evento);
            });

            Parallel.ForEach(eventos, evento =>
            {
                if (evento.SemData)
                {
                    var dataMais2Meses = evento.DataPublicacao.AddMonths(2);
                    if (evento.DataPublicacao.AddMonths(2) < DateTime.Now)
                    {
                        eventosFinal.Remove(evento);
                    }
                }
            });
            ViewBag.idusuario = GetUsuarioLogado();
            if (ordenarPor == "DataMenor")
            {
                eventosFinal = eventosFinal.OrderBy(x => x.DataEvento).ToList();
            }
            else if (ordenarPor == "DataMaior")
            {
                eventosFinal = eventosFinal.OrderByDescending(x => x.DataEvento).ToList();
            }

            eventosFinal[0].Filtros = new Filtro();
            var resultadoPaginado = eventosFinal.ToPagedList(pageNumber, 10);

            return View("Index", resultadoPaginado);
        }

        [Login]
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
            ViewBag.LocalEsolhido = "false";
            int IdUsuarioLogado = GetUsuarioLogado();

            if (ModelState.IsValid)
            {

                Evento_P1 eventoCadastrar = new Evento_P1();

                eventoCadastrar.DataPublicacao = DateTime.Now;
                eventoCadastrar.IdUsuarioAdm = IdUsuarioLogado;
                eventoCadastrar.Titulo = evento.Titulo;
                eventoCadastrar.Categoria = evento.Categoria;
                eventoCadastrar.Descricao = evento.Descricao;
                eventoCadastrar.CidadeEstado = evento.Cidade_Estado;
                eventoCadastrar.EventoRecorrente = evento.EventoRecorrente;
                eventoCadastrar.SemData = evento.SemData;
                eventoCadastrar.Privado = evento.Privado;

                if (evento.SemData)
                {
                    eventoCadastrar.DataEvento = null;
                }
                else
                {
                    if (evento.DataEvento == null)
                    {
                        ModelState.AddModelError("DataEvento", "Data do evento necessária!");
                        return View("Cadastrar", evento);
                    }
                    else
                    {
                        eventoCadastrar.DataEvento = evento.DataEvento;
                    }
                }

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

        [Login]
        [HttpGet]
        public IActionResult Editar(int Id, bool reativando = false)
        {
            int IdUsuarioLogado = GetUsuarioLogado();

            Evento_P1 evento = new Evento_P1(Id);
            evento.CompleteObject();

            if (IdUsuarioLogado == evento.IdUsuarioAdm)
            {
                Mapper.CreateMap<Evento_P1, Evento>();
                Evento EventoEdidar = Mapper.Map<Evento>(evento);
                ViewBag.CadastrarAtualizar = "Salvar";
                ViewBag.LocalEsolhido = "true";
                if (reativando)
                {
                    EventoEdidar.DataEvento = null;
                    ModelState.AddModelError("DataEvento", "Informe a nova data do evento!");
                }

                return View("Cadastrar", EventoEdidar);
            }

            TempData["MensagemErro"] = "Evento inacessível com seu usuário";
            return RedirectToAction("Index", "Evento");
        }

        [HttpPost]
        public IActionResult Editar([FromForm] Evento evento)
        {

            ViewBag.CadastrarAtualizar = "Salvar";
            int IdUsuarioLogado = GetUsuarioLogado();

            if (ModelState.IsValid)
            {
                Evento_P1 eventos = new Evento_P1(evento.Id_Evento);
                eventos.CompleteObject();

                eventos.DataPublicacao = DateTime.Now;
                eventos.IdUsuarioAdm = IdUsuarioLogado;
                eventos.Titulo = evento.Titulo;
                eventos.Categoria = evento.Categoria;
                eventos.Descricao = evento.Descricao;
                eventos.CidadeEstado = evento.Cidade_Estado;
                eventos.SemData = evento.SemData;
                eventos.Privado = evento.Privado;
                eventos.EventoRecorrente = evento.EventoRecorrente;

                if (evento.SemData)
                {
                    eventos.DataEvento = null;
                }
                else
                {
                    if (evento.DataEvento == null)
                    {
                        ModelState.AddModelError("DataEvento", "Data do evento necessária!");
                        return View("Cadastrar", evento);
                    }
                    else
                    {
                        eventos.DataEvento = evento.DataEvento;
                    }
                }

                eventos.Save();

                TempData["Mensagem"] = "O evento foi atualizado com sucesso!";
                return RedirectToAction("MeusEventos", "Perfil");
            }
            return View("Cadastrar", evento);
        }

        [Login]
        [HttpGet]
        public IActionResult Excluir(int Id)
        {
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

        [Login]
        [HttpGet]
        public IActionResult Voluntariar(int Id)
        {
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

                    Notificacao_P1 notificacao = new Notificacao_P1();
                    notificacao.IdUsuarioNotificado = evento.IdUsuarioAdm;
                    notificacao.IdUsuarioNotificou = IdUsuarioLogado;
                    notificacao.Descricao = "Alguém voluntariou-se em um evento seu! Clique aqui e saiba mais!";
                    notificacao.NotificacaoAtiva = true;
                    notificacao.UrlNotificacao = DbAcess.GetUrlOrigem() + "evento/visualizar/" + Id;
                    notificacao.DataCadastro = DateTime.Now;
                    notificacao.Save();

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

        [Login]
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

                Regex regex = new Regex(@"\D");
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
            List<UsuarioCompleto> voluntariosOrdenados = voluntariosCompleto.OrderByDescending(x => x.NotaMedia).ToList();
            var resultadoPaginado = voluntariosOrdenados.ToPagedList(pageNumber, 10);

            return View(resultadoPaginado);

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
