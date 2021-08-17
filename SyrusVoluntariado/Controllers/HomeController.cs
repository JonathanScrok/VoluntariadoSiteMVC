﻿using Microsoft.AspNetCore.Mvc;
using SyrusVoluntariado.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace SyrusVoluntariado.Controllers {
    public class HomeController : Controller {

        private DatabaseContext _db;

        public HomeController(DatabaseContext db) {
            _db = db;
        }
        public IActionResult Index(int? page) {
            var pageNumber = page ?? 1;

            var palavras = _db.Vagas.ToList();
            var resultadoPaginado = palavras.ToPagedList(pageNumber, 5);

            return View(resultadoPaginado);
        }
    }
}
