using BeaHelper.BLL.BD;
using BeaHelper.BLL.Models;
using Microsoft.AspNetCore.Mvc;
using BeaHelper.Library.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;
using Microsoft.AspNetCore.Http;

namespace BeaHelper.Controllers
{
    [Login]
    public class VoluntarioController : Controller
    {
        public IActionResult Index()
        {
            int IdUsuarioLogado = GetUsuarioLogado();
            var voluntarios = Usuario_P2.TodosUsuarios(IdUsuarioLogado);

            List<UsuarioCompleto> voluntariosCompleto = new List<UsuarioCompleto>();

            foreach (var voluntario in voluntarios)
            {
                UsuarioCompleto voluntarioCompleto = new UsuarioCompleto();

                var AvaliacaoUsuario = Avaliacao_P1.TodasAvaliacoesUsuario(voluntario.Id_Usuario);
                double countNota = 0;

                if (AvaliacaoUsuario != null && AvaliacaoUsuario.Count > 0)
                {
                    foreach (var avaliacao in AvaliacaoUsuario)
                    {
                        countNota += avaliacao.Nota;
                    }
                    voluntario.Senha = "0";

                    if (voluntario.NumeroCelular == null)
                    {
                        voluntario.NumeroCelular = "Não Declarado";
                    }

                    voluntarioCompleto.Usuario = voluntario;
                    var media = countNota / AvaliacaoUsuario.Count;
                    media = Math.Round(media, 1);
                    voluntarioCompleto.NotaMedia = media;
                    voluntarioCompleto.NuncaAvaliado = false;
                }
                else
                {
                    if (voluntario.NumeroCelular == null)
                    {
                        voluntario.NumeroCelular = "Não Declarado";
                    }

                    voluntarioCompleto.Usuario = voluntario;
                    voluntarioCompleto.NuncaAvaliado = true;
                }
                var notificacoes = Notificacao_P1.BuscaIdUsuario_NotificouENotificado(voluntario.Id_Usuario, IdUsuarioLogado);
                if (notificacoes.Count > 0)
                    voluntarioCompleto.JaConvidado = true;
                else
                    voluntarioCompleto.JaConvidado = false;
                voluntariosCompleto.Add(voluntarioCompleto);
            }

            var pageNumber = 1;
            List<UsuarioCompleto> voluntariosOrdenados = voluntariosCompleto.OrderByDescending(x => x.NotaMedia).ToList();
            
            var resultadoPaginado = voluntariosOrdenados.ToPagedList(pageNumber, 10);

            return View(resultadoPaginado);
        }

        [HttpGet]
        public IActionResult Convidar(int Id)
        {
            return RedirectToAction("MeusEventos", "Perfil", new { Convidando = true, IdVoluntarioConvidado = Id});
        }

        [HttpGet]
        public IActionResult PostConvidar(int Id, int IdEvento)
        {
            Notificacao_P1 notificacao = new Notificacao_P1();
            notificacao.IdUsuarioNotificado = Id;
            notificacao.IdUsuarioNotificou = GetUsuarioLogado();
            notificacao.Descricao = "Alguém te convidou para participar de um evento! Clique aqui e saiba mais!";
            notificacao.NotificacaoAtiva = true;
            notificacao.UrlNotificacao = "https://localhost:44394/evento/visualizar/" + IdEvento;
            notificacao.DataCadastro = DateTime.Now;
            notificacao.Save();

            return RedirectToAction("Index", "Voluntario");
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
