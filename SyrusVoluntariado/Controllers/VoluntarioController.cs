using BeaHelper.BLL.BD;
using BeaHelper.BLL.Models;
using Microsoft.AspNetCore.Mvc;
using BeaHelper.Library.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace BeaHelper.Controllers
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
                UsuarioCompleto voluntarioCompleto = new UsuarioCompleto();

                var AvaliacaoUsuario = Avaliacao_P1.TodasAvaliacoesUsuario(voluntario.Id_Usuario);
                int countNota = 0;

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
                    voluntarioCompleto.NotaMedia = countNota / AvaliacaoUsuario.Count;
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
                voluntariosCompleto.Add(voluntarioCompleto);
            }

            var pageNumber = 1;

            var resultadoPaginado = voluntariosCompleto.ToPagedList(pageNumber, 10);

            return View(resultadoPaginado);
        }
    }
}
