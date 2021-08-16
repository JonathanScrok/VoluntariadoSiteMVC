using Microsoft.AspNetCore.Mvc;
using SyrusVoluntariado.Library.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SyrusVoluntariado.Controllers {
    [Login]
    public class CadastrovagaController : Controller {
        public IActionResult Index() {
            return View();
        }
    }
}
