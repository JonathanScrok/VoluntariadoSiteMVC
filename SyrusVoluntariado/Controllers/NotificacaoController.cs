using BeaHelper.BLL.BD;
using BeaHelper.BLL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeaHelper.Controllers
{
    public class NotificacaoController : Controller
    {
        public IActionResult Index()
        {
            int IdUsuarioLogado = GetUsuarioLogado();
            List<Notificacao> notificacoes = new List<Notificacao>();
            if (IdUsuarioLogado != 0)
            {
                notificacoes = Notificacao_P1.TodasNotificacoesUsuarioAtiva(IdUsuarioLogado);
            }

            ViewBag.TotalNotificacoes = notificacoes.Count;

            return View(notificacoes);
        }

        [HttpGet]
        public int QtdNotificacao()
        {
            int IdUsuarioLogado = GetUsuarioLogado();
            if (IdUsuarioLogado > 0)
                return Notificacao_P1.CountTodasNotificacoesUsuarioAtiva(IdUsuarioLogado);
            else
                return 0;
        }

        [HttpPost]
        public string NotificacaoVisualizada(int idNotificacao)
        {
            Notificacao_P1 notificacao = new Notificacao_P1(idNotificacao);
            notificacao.CompleteObject();
            notificacao.Flg_Visualizado = true;
            notificacao.Save();
            return notificacao.UrlNotificacao;
        }

        [HttpGet]
        public async Task<bool> NotificacoesRecentes()
        {
            int IdUsuarioLogado = GetUsuarioLogado();
            if (IdUsuarioLogado > 0)
            {
                var notificacoesRecentes = await Notificacao_P1.NotificacoesRecentes(IdUsuarioLogado);
                var UmaSemanaAtras = DateTime.Now.AddDays(-7);

                var listaNotifiRecentes = notificacoesRecentes.Where(x => x.DataCadastro >= UmaSemanaAtras).ToList();
                ViewBag.ListaNotificacoesRecentes = listaNotifiRecentes;
                return true;
            }
            else
            {
                return false;
            }
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
