using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SyrusVoluntariado.Database;
using SyrusVoluntariado.Library.Filters;
using SyrusVoluntariado.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SyrusVoluntariado.Controllers {
    [Login]
    public class PerfilController : Controller {

        private DatabaseContext _db;

        public PerfilController(DatabaseContext db) {
            _db = db;
        }

        public IActionResult Index() {

            int IdfUsuario = HttpContext.Session.GetInt32("IdUsuarioLogado").GetValueOrDefault();
            Usuario usuario = _db.Usuarios.Find(IdfUsuario);

            return View(usuario);
        }

        public IActionResult MinhasVagas() {

            int IdfUsuario = HttpContext.Session.GetInt32("IdUsuarioLogado").GetValueOrDefault();
            var VagasUsuario = _db.Vagas.Where(a => a.Idf_Usuario_Adm == IdfUsuario).ToList();

            return View(VagasUsuario);
        }
    }
}
