﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SyrusVoluntariado.BLL;
using SyrusVoluntariado.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace SyrusVoluntariado.Controllers {
    public class LoginController : Controller {

        [HttpGet]
        public IActionResult Index() {
            ViewBag.FooterPrecisa = false;
            return View();
        }

        [HttpPost]
        public ActionResult Index([FromForm] Usuario usuario) {

            var Logins = Login_P1.TodosLogins();

            var ValidaDados = Logins.Where(a => a.Email == usuario.Email && a.Senha == usuario.Senha).FirstOrDefault();

            if (ValidaDados != null) {

                /*
                * Add Session
                * HttpContext.Session.SetString("Login", "true");
                * HttpContext.Session.SetInt32("UserID", 32);
                * HttpContext.Session.SetString("Login", Serializar Object > String);

                * Ler Session
                * string login = HttpContext.Session.GetString("Login");
                */
                Usuario_P1 Usuario = new Usuario_P1(ValidaDados.Id_Usuario);
                Usuario.CompleteObject();

                HttpContext.Session.SetString("Login", "true");
                HttpContext.Session.SetString("UsuarioLogado", Usuario.Nome);

                HttpContext.Session.SetInt32("IdUsuarioLogado", ValidaDados.Id_Usuario);
                string UrlAction;
                string UrlControler;
                string Id;

                try {
                    UrlAction = TempData["URLRedirectAction"].ToString();
                    UrlControler = TempData["URLRedirectController"].ToString();
                    TempData["URLRedirectAction"] = "Index";
                    TempData["URLRedirectController"] = "Home";
                    Id = TempData["URLRedirectArgumento"].ToString();
                    if (Id != "null") {
                        return Redirect("/" + UrlControler + "/" + UrlAction + "/" + Id);
                    } else {
                        return RedirectToAction(UrlAction, UrlControler);
                    }
                } catch (NullReferenceException) {
                    UrlAction = "Index";
                    UrlControler = "Home";
                    return RedirectToAction(UrlAction, UrlControler);
                }
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

                Usuario_P1 usu = new Usuario_P1();

                usu.Nome = usuario.Nome;
                usu.Email = usuario.Email;
                if (usuario.Sexo == "Masculino")
                {
                    usu.Sexo = 1;
                }
                else if (usuario.Sexo == "Feminino")
                {
                    usu.Sexo = 2;
                }
                else
                {
                    usu.Sexo = 3;
                }
                //usu.Sexo = usuario.Sexo;

                usu.DataCadastro = DateTime.Now;
                usu.Save();

                usu.CompleteObject(usuario.Email);
                 
                Login_P1 login = new Login_P1();
                login.IdUsuario = usu.IdUsuario;
                login.Email = usuario.Email;
                login.Senha = usuario.Senha;
                login.DataCadastro = DateTime.Now;
                login.Save();

                HttpContext.Session.SetString("Login", "true");

                HttpContext.Session.SetString("UsuarioLogado", usuario.Nome);
                HttpContext.Session.SetInt32("IdUsuarioLogado", usu.IdUsuario);

                return RedirectToAction("Index", "Home");
            }
            return View(usuario);
        }

        public bool ExisteEmail(Usuario usuario) {

            int? emails = Login_P1.BuscaLogin_Email(usuario.Email).Count;

            if (emails == 0 || emails == null) {
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
