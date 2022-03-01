using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BeaHelper.Library.Filters;
using BeaHelper.BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeaHelper.BLL.BD;

namespace BeaHelper.Controllers
{
    [Login]
    public class AvaliarController : Controller
    {
        [HttpGet]
        public IActionResult Index(int Id, int IdEvento)
        {
            HttpContext.Session.SetInt32("UsuarioAvaliado", Id);
            HttpContext.Session.SetInt32("VagaId", IdEvento);

            return View(new Avaliacao());
        }

        [HttpGet]
        public IActionResult PostAvaliacao(int Id)
        { //OBS: Id é a Quantidade de estrelas da avaliação!
            var QtdEstrelas = Id;
            int IdUsuarioAvaliado = HttpContext.Session.GetInt32("UsuarioAvaliado").GetValueOrDefault();
            int IdUsuarioLogado = GetUsuarioLogado();
            int IdEvento = HttpContext.Session.GetInt32("VagaId").GetValueOrDefault();

            Avaliacao_P1 avaliacao = new Avaliacao_P1();
            avaliacao.IdUsuarioAvaliado = IdUsuarioAvaliado;
            avaliacao.IdUsuarioAvaliou = IdUsuarioLogado;
            avaliacao.Nota = QtdEstrelas;
            avaliacao.DataCadastro = DateTime.Now;
            avaliacao.Save();


            return RedirectToAction("ListaVoluntarios", "Evento", new {Id = IdEvento });
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
