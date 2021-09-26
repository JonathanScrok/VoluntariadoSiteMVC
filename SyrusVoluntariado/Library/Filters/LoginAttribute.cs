using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SyrusVoluntariado.Library.Filters {
    public class LoginAttribute : ActionFilterAttribute {

        public override void OnActionExecuting(ActionExecutingContext context) {

            var ValoresRedirecionamento = context.ActionDescriptor.RouteValues.Values.ToList();
            var ValorArgumento = context.ActionArguments.Values.ToList();

            string ActionRedi = ValoresRedirecionamento[0];
            string ControlerRedi = ValoresRedirecionamento[1];

            var login = context.HttpContext.Session.GetString("Login"); //Remover ou Comentar
            var Logado = context.HttpContext.Request.Cookies["Logado"];

            if (Logado == null) {

                if (context.Controller != null) {
                    Controller controlador = context.Controller as Controller;
                    string argumento;
                    if (ValorArgumento.Count > 0) {
                        argumento = ValorArgumento[0].ToString();
                    } else {
                        argumento = "null";
                    }

                    controlador.TempData["URLRedirectController"] = ControlerRedi;
                    controlador.TempData["URLRedirectAction"] = ActionRedi;
                    controlador.TempData["URLRedirectArgumento"] = argumento;
                    controlador.TempData["MensagemErro"] = "Faça o Login para acessar esta página!";
                }

                context.Result = new RedirectToActionResult("Index", "Login", null);
            }
        }
    }
}
