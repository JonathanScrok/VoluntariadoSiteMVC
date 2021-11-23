using BeaHelper.BLL.BD;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using BeaHelper.BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeaHelper.BLL.Services;

namespace SyrusVoluntariado.Controllers
{
    public class LoginController : Controller
    {

        [HttpGet]
        public IActionResult Index()
        {
            string LoginSession = HttpContext.Session.GetString("Login"); //Remover ou Comentar
            var LogadoCookie = HttpContext.Request.Cookies["Logado"];

            if (LoginSession == "true" || LogadoCookie == "true")
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.FooterPrecisa = false;
                return View(new Usuario());
            }

        }

        [HttpPost]
        public ActionResult Index([FromForm] Usuario usuario)
        {
            if (usuario.Email != null && usuario.Senha != null)
            {
                string senhaEncoding = _encodeSenha.HashValue(usuario.Senha);
                var LoginExitente = Login_P1.BuscaLogin_EmailSenha(usuario.Email, senhaEncoding);

                if (LoginExitente.Count > 0)
                {
                    Usuario_P1 Usuario = new Usuario_P1(LoginExitente[0].Id_Usuario);
                    Usuario.CompleteObject();

                    CookieOptions option = new CookieOptions();
                    option.Expires = DateTime.Now.AddDays(7);

                    string[] NomeCompleto = Usuario.Nome.Split(" ");
                    string primeiroNome = NomeCompleto[0].ToString();

                    if (usuario.ManterConectado)
                    {
                        Response.Cookies.Append("Logado", "true", option); //Cookie
                        Response.Cookies.Append("UsuarioLogado", primeiroNome, option); //Cookie
                        Response.Cookies.Append("IdUsuarioLogado", LoginExitente[0].Id_Usuario.ToString(), option); //Cookie
                    }
                    else
                    {
                        HttpContext.Session.SetString("Login", "true"); //Session
                        HttpContext.Session.SetString("UsuarioLogado", primeiroNome); //Session
                        HttpContext.Session.SetInt32("IdUsuarioLogado", LoginExitente[0].Id_Usuario); //Session
                    }

                    string UrlAction;
                    string UrlControler;
                    string Id;

                    try
                    {
                        UrlAction = TempData["URLRedirectAction"].ToString();
                        UrlControler = TempData["URLRedirectController"].ToString();
                        TempData["URLRedirectAction"] = "Index";
                        TempData["URLRedirectController"] = "Home";
                        Id = TempData["URLRedirectArgumento"].ToString();
                        if (Id != "null")
                        {
                            return Redirect("/" + UrlControler + "/" + UrlAction + "/" + Id);
                        }
                        else
                        {
                            return RedirectToAction(UrlAction, UrlControler);
                        }
                    }
                    catch (NullReferenceException)
                    {
                        UrlAction = "Index";
                        UrlControler = "Home";
                        return RedirectToAction(UrlAction, UrlControler);
                    }
                }
                else
                {
                    TempData["MensagemErro"] = "Email ou senha estão incorretos!";
                    return View(usuario);
                }
            }
            else
            {
                return View(usuario);
            }
        }

        [HttpGet]
        public IActionResult CadastrarUsuario()
        {
            string LoginSession = HttpContext.Session.GetString("Login"); //Remover ou Comentar
            var LogadoCookie = HttpContext.Request.Cookies["Logado"];

            if (LoginSession == "true" || LogadoCookie == "true")
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.FooterPrecisa = false;
                return View(new Usuario());
            }
        }

        [HttpPost]
        public IActionResult CadastrarUsuario([FromForm] Usuario usuario)
        {
            bool ExistenciaEmail = false;

            if (usuario.Email != null)
            {
                ExistenciaEmail = ExisteEmail(usuario);

                if (ExistenciaEmail == true)
                {
                    TempData["MensagemErro"] = "O email " + usuario.Email + " já está cadastrado!";
                }
            }


            if (ModelState.IsValid && ExistenciaEmail == false)
            {
                string senhaEncoding = _encodeSenha.HashValue(usuario.Senha);

                Usuario_P1 usu = new Usuario_P1();

                usu.Nome = usuario.Nome;
                usu.Email = usuario.Email;
                usu.NumeroCelular = usuario.NumeroCelular;
                usu.Sexo = usuario.Sexo;

                usu.DataCadastro = DateTime.Now;
                usu.Save();

                usu.CompleteObject(usuario.Email);

                Login_P1 login = new Login_P1();
                login.IdUsuario = usu.IdUsuario;
                login.Email = usuario.Email;
                login.Senha = senhaEncoding;
                login.DataCadastro = DateTime.Now;
                login.Save();

                CookieOptions option = new CookieOptions();
                option.Expires = DateTime.Now.AddDays(7);

                //HttpContext.Session.SetString("Login", "true"); //Remover ou Comentar
                Response.Cookies.Append("Logado", "true", option);

                string[] NomeCompleto = usuario.Nome.Split(" ");
                string primeiroNome = NomeCompleto[0].ToString();

                //HttpContext.Session.SetString("UsuarioLogado", primeiroNome); //Remover ou Comentar
                Response.Cookies.Append("UsuarioLogado", primeiroNome, option);

                //HttpContext.Session.SetInt32("IdUsuarioLogado", usu.IdUsuario); //Remover ou Comentar
                Response.Cookies.Append("IdUsuarioLogado", usu.IdUsuario.ToString(), option);

                return RedirectToAction("Index", "Home");
            }
            return View(usuario);
        }

        public bool ExisteEmail(Usuario usuario)
        {

            int? emails = Login_P1.BuscaLogin_Email(usuario.Email).Count;

            if (emails == 0 || emails == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public ActionResult Logout()
        {
            HttpContext.Session.Clear();

            foreach (var cookie in Request.Cookies.Keys)
            {
                if (cookie != "AceitaCookies")
                {
                    Response.Cookies.Delete(cookie);
                }
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
