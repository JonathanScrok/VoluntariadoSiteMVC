using Microsoft.AspNetCore.Mvc;
using SyrusVoluntariado.BLL;
using SyrusVoluntariado.Library.Filters;
using SyrusVoluntariado.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SyrusVoluntariado.Controllers
{
    [Login]
    public class AvaliarController : Controller
    {
        [HttpGet]
        public IActionResult Index(int Id)
        {
            ViewBag.IdUsuAvaliar = Id;
            return View(new Avaliacao());
        }

        [HttpGet]
        public IActionResult PostAvaliacao(int Id)
        { //OBS: Id é a Quantidade de estrelas da avaliação!
            var QtdEstrelas = Id;
            int IdUsuarioAvaliado = ViewBag.IdUsuAvaliar;
            int IdUsuarioLogado = GetUsuarioLogado();

            Avaliacao_P1 avaliacao = new Avaliacao_P1();
            avaliacao.IdUsuario = IdUsuarioAvaliado;
            avaliacao.Nota = QtdEstrelas;
            avaliacao.DataCadastro = DateTime.Now;
            avaliacao.Save();


            return RedirectToAction("ListaVoluntarios", "Vaga");
        }

        public int GetUsuarioLogado()
        {
            int IdUsuarioLogado = Int32.Parse(HttpContext.Request.Cookies["IdUsuarioLogado"]);

            return IdUsuarioLogado;
        }
    }
}
