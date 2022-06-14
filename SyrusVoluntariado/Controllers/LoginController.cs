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
using BeaHelper.Library.Mail;
using AutoMapper;

namespace BeaHelper.Controllers
{
    public class LoginController : Controller
    {

        [HttpGet]
        public IActionResult Index()
        {
            string LoginSession = HttpContext.Session.GetString("Login");
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

                    string UrlAction = string.Empty;
                    string UrlControler = string.Empty;
                    string Id;

                    if (TempData["URLRedirectAction"] != null && TempData["URLRedirectController"] != null && TempData["URLRedirectArgumento"] != null)
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
                    else
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
            string LoginSession = HttpContext.Session.GetString("Login");
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

        [HttpGet]
        public IActionResult RecuperarSenha()
        {
            ViewBag.EmailEnviado = false;
            return View(new Usuario());
        }

        [HttpPost]
        public IActionResult RecuperarSenha([FromForm] Usuario usuario)
        {

            //var config = new MapperConfiguration(cfg =>
            //{
            //    cfg.CreateMap<Usuario_P1, Usuario>();
            //});
            //IMapper _mapper = config.CreateMapper();
            //var usuarioEmail = _mapper.Map<Usuario_P1>(usuario);

            var result = EnviarEmailRecuperacaoSenha(usuario);
            if (result == false)
            {
                ViewBag.EmailEnviado = false;
                return View(usuario);
            }
            else
            {
                ViewBag.EmailEnviado = true;
                return View();
            }
        }

        [HttpGet]
        public IActionResult NovaSenha(int id)
        {
            AlterarSenha usuarioSenha = new AlterarSenha();
            usuarioSenha.IdUsuario = id;
            return View(usuarioSenha);
        }

        [HttpPost]
        public IActionResult NovaSenha([FromForm] AlterarSenha novasenha)
        {
            if (ModelState.IsValid)
            {
                if (novasenha.Senha == novasenha.ConfirmacaoSenha)
                {
                    string senhaEncoding = _encodeSenha.HashValue(novasenha.Senha);
                    Login_P1 login = new Login_P1(novasenha.IdUsuario);
                    login.CompleteObject();
                    login.IdUsuario = novasenha.IdUsuario;
                    login.Senha = senhaEncoding;
                    login.Save();

                    TempData["MensagemSucesso"] = "Sua senha foi alterada com sucesso!";
                    return RedirectToAction("Index", "Login");
                }
                else
                {
                    TempData["MensagemErro"] = "As senhas devem coincidirem";
                    return View(novasenha);
                }

            }
            return View(novasenha);
        }

        [HttpGet]
        public IActionResult EmailEnviado()
        {
            return View();
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

        public bool EnviarEmailRecuperacaoSenha(Usuario usuario)
        {
            try
            {
                Usuario_P1 usuarioAdm = new Usuario_P1();
                usuarioAdm.CompleteObject(usuario.Email);

                if (usuarioAdm.Email != null && usuarioAdm.Nome != null)
                {
                    EnviarEmail.EnviarRecuperacaoSenha(usuarioAdm);
                }
                else
                {
                    TempData["MensagemErro"] = "Este e-mail não está cadastrado em nosso sistema!";
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
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
