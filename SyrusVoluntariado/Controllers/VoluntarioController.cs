using BeaHelper.BLL.BD;
using BeaHelper.BLL.Models;
using Microsoft.AspNetCore.Mvc;
using SyrusVoluntariado.Library.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace SyrusVoluntariado.Controllers
{
    [Login]
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
            
            List<UsuarioCompleto> UsuariosCompletos = new List<UsuarioCompleto>();
            var resultadoPaginado = UsuariosCompletos.ToPagedList(pageNumber, 10);

            return View(resultadoPaginado);
        }
    }
}
