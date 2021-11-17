using BeaHelper.BLL.BD;
using BeaHelper.BLL.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SyrusVoluntariado.Controllers
{
    public class VoluntarioController : Controller
    {
        public IActionResult Index()
        {
            var voluntarios = Usuario_P2.TodosUsuarios();

            List<UsuarioCompleto> voluntariosCompleto = new List<UsuarioCompleto>();
            foreach (var voluntario in voluntarios)
            {
                var AvaliacaoUsuario = Avaliacao_P1.TodasAvaliacoesUsuario(voluntario.Id);
            }

            var pageNumber = 1;
            //var resultadoPaginado = voluntariosCompleto.ToPagedList(pageNumber, 10);

            UsuarioCompleto UsuarioCompleto = new UsuarioCompleto();
            return View(UsuarioCompleto);
        }
    }
}
