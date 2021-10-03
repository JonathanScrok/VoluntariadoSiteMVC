using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SyrusVoluntariado.Library.Filters;
using BeaHelper.BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeaHelper.BLL.BD;

namespace SyrusVoluntariado.Controllers
{
    [Login]
    public class AvaliarController : Controller
    {
        [HttpGet]
        public IActionResult Index(int Id, int IdVaga)
        {
            HttpContext.Session.SetInt32("UsuarioAvaliado", Id);
            HttpContext.Session.SetInt32("VagaId", IdVaga);

            return View(new Avaliacao());
        }

        [HttpGet]
        public IActionResult PostAvaliacao(int Id)
        { //OBS: Id é a Quantidade de estrelas da avaliação!
            var QtdEstrelas = Id;
            int IdUsuarioAvaliado = HttpContext.Session.GetInt32("UsuarioAvaliado").GetValueOrDefault();
            int IdUsuarioLogado = GetUsuarioLogado();
            int IdVaga = HttpContext.Session.GetInt32("VagaId").GetValueOrDefault();

            Avaliacao_P1 avaliacao = new Avaliacao_P1();
            avaliacao.IdUsuarioAvaliado = IdUsuarioAvaliado;
            avaliacao.IdUsuarioAvaliou = IdUsuarioLogado;
            avaliacao.Nota = QtdEstrelas;
            avaliacao.DataCadastro = DateTime.Now;
            avaliacao.Save();


            return RedirectToAction("ListaVoluntarios", "Vaga", new {Id = IdVaga });
        }

        public int GetUsuarioLogado()
        {
            int IdUsuarioLogado = Int32.Parse(HttpContext.Request.Cookies["IdUsuarioLogado"]);

            return IdUsuarioLogado;
        }
    }
}
