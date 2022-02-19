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

        public int QtdNotificacao()
        {
            int IdUsuarioLogado = GetUsuarioLogado();
            return Notificacao_P1.CountTodasNotificacoesUsuarioAtiva(IdUsuarioLogado);
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
