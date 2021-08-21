using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SyrusVoluntariado.Database;
using SyrusVoluntariado.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace SyrusVoluntariado.Controllers {
    public class LoginController : Controller {
        private DatabaseContext _db;

        public LoginController(DatabaseContext db) {
            _db = db;
        }

        [HttpGet]
        public IActionResult Index() {
            ViewBag.FooterPrecisa = false;
            return View();
        }

        [HttpPost]
        public ActionResult Index([FromForm] Usuario usuario) {

            var ValidaDados = _db.Usuarios.Where(a => a.Email == usuario.Email && a.Senha == usuario.Senha).FirstOrDefault();


            if (ValidaDados != null) {

                /*
                * Add Session
                * HttpContext.Session.SetString("Login", "true");
                * HttpContext.Session.SetInt32("UserID", 32);
                * HttpContext.Session.SetString("Login", Serializar Object > String);

                * Ler Session
                * string login = HttpContext.Session.GetString("Login");
                */

                HttpContext.Session.SetString("Login", "true");
                HttpContext.Session.SetString("UsuarioLogado", ValidaDados.Nome);

                HttpContext.Session.SetInt32("IdUsuarioLogado", ValidaDados.Id);

                return RedirectToAction("Index", "Home");

            } else {

                TempData["MensagemErro"] = "Email ou senha estão incorretos!";
                return View();
            }


        }

        [HttpGet]
        public IActionResult CadastrarUsuario() {
            ViewBag.FooterPrecisa = false;
            return View(new Usuario());
        }

        [HttpPost]
        public IActionResult CadastrarUsuario([FromForm] Usuario usuario) {

            var ExistenciaEmail = ExisteEmail(usuario);

            if (ExistenciaEmail == true) {
                TempData["MensagemErro"] = "O email " + usuario.Email + " já está cadastrado!";
            }

            if (ModelState.IsValid && ExistenciaEmail == false) {

                _db.Usuarios.Add(usuario);
                _db.SaveChanges();

                HttpContext.Session.SetString("Login", "true");

                HttpContext.Session.SetString("UsuarioLogado", usuario.Nome);
                HttpContext.Session.SetInt32("IdUsuarioLogado", usuario.Id);

                return RedirectToAction("Index", "Home");
            }
            return View(usuario);
        }

        public bool ExisteEmail(Usuario usuario) {

            var UsuariosCadastrados = _db.Usuarios.Where(a => a.Email == usuario.Email && a.Id != usuario.Id).FirstOrDefault();

            if (UsuariosCadastrados == null) {
                return false;
            } else {
                return true;
            }
        }

        public ActionResult Logout() {
            HttpContext.Session.Clear();

            return RedirectToAction("Index", "Home");
        }
    }
}
