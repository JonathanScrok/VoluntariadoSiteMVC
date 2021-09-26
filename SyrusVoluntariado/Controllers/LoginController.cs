using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using RestSharp;
using SyrusVoluntariado.BLL;
using SyrusVoluntariado.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace SyrusVoluntariado.Controllers
{
    public class LoginController : Controller
    {
        private IMemoryCache _cache; 

        public LoginController(IMemoryCache memoryCache)
        {
            _cache = memoryCache;
        }

        [HttpGet]
        public IActionResult Index()
        {
            //string login = HttpContext.Session.GetString("Login"); //Remover ou Comentar
            var Logado = HttpContext.Request.Cookies["Logado"];
            
            if (Logado == "true")
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.FooterPrecisa = false;
                return View();
            }

        }

        [HttpPost]
        public ActionResult Index([FromForm] Usuario usuario)
        {
            //Buscar no banco usuarios com este email e senha:
            //usuario.Email
            //usuario.Senha

            //Se existir deixa passar..

            //var Logins = Login_P1.TodosLogins();

            //var ValidaDados = Logins.Where(a => a.Email == usuario.Email && a.Senha == usuario.Senha).FirstOrDefault();

            if (true)
            {

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


                CookieOptions option = new CookieOptions();
                option.Expires = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, "E. South America Standard Time").AddDays(7);

                //HttpContext.Session.SetString("Login", "true"); //Remover ou Comentar
                Response.Cookies.Append("Logado", "true", option);

                string[] NomeCompleto = Usuario.Nome.Split(" ");

                string primeiroNome = NomeCompleto[0].ToString();

                //HttpContext.Session.SetString("UsuarioLogado", primeiroNome); //Remover ou Comentar
                Response.Cookies.Append("UsuarioLogado", primeiroNome, option);

                //HttpContext.Session.SetInt32("IdUsuarioLogado", ValidaDados.Id_Usuario); //Remover ou Comentar
                Response.Cookies.Append("IdUsuarioLogado", ValidaDados.Id_Usuario.ToString(), option);

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
                return View();
            }
        }

        [HttpGet]
        public IActionResult CadastrarUsuario()
        {
            //string login = HttpContext.Session.GetString("Login"); //Remover ou Comentar
            var Logado = HttpContext.Request.Cookies["Logado"];

            if (Logado == "true")
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

                Usuario_P1 usu = new Usuario_P1();

                usu.Nome = usuario.Nome;
                usu.Email = usuario.Email;
                usu.Sexo = usuario.Sexo;

                usu.DataCadastro = DateTime.Now;
                usu.Save();

                usu.CompleteObject(usuario.Email);

                Login_P1 login = new Login_P1();
                login.IdUsuario = usu.IdUsuario;
                login.Email = usuario.Email;
                login.Senha = usuario.Senha;
                login.DataCadastro = DateTime.Now;
                login.Save();

                CookieOptions option = new CookieOptions();
                option.Expires = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, "E. South America Standard Time").AddDays(7);

                HttpContext.Session.SetString("Login", "true");
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
                Response.Cookies.Delete(cookie);
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
