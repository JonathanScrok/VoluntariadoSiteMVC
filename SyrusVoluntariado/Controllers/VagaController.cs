using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SyrusVoluntariado.BLL;
using SyrusVoluntariado.Database;
using SyrusVoluntariado.Library.Filters;
using SyrusVoluntariado.Library.Mail;
using SyrusVoluntariado.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace SyrusVoluntariado.Controllers
{
    [Login]
    public class VagaController : Controller
    {

        public IActionResult Index(int? page)
        {
            var pageNumber = page ?? 1;

            List<Vaga> vagas = Vaga_P1.TodasVagas();

            var resultadoPaginado = vagas.ToPagedList(pageNumber, 10);

            return View(resultadoPaginado);
        }

        [HttpGet]
        public IActionResult Cadastrar()
        {
            ViewBag.CadastrarAtualizar = "Cadastrar";

            return View(new Vaga());
        }

        [HttpPost]
        public IActionResult Cadastrar([FromForm] Vaga vaga)
        {

            ViewBag.CadastrarAtualizar = "Cadastrar";
            var ValorUsuarioLogado = HttpContext.Session.GetInt32("IdUsuarioLogado");
            int IdUsuarioLogado = ValorUsuarioLogado.GetValueOrDefault();

            
            if (vaga.DataEvento.ToString("dd/MM/yyyy") == "01/01/0001")
            {
                TempData["DataEvento"] = "Data Obrigatória! Caso não deseje informar o horário campo deve ser colocado como 00/00/0000 00:00";
            }

            if (ModelState.IsValid)
            {

                Vaga_P1 vagaCadastrar = new Vaga_P1();
                vagaCadastrar.CompleteObject();

                vagaCadastrar.DataPublicacao = DateTime.Now;
                vagaCadastrar.DataEvento = vaga.DataEvento;
                vagaCadastrar.IdUsuarioAdm = IdUsuarioLogado;
                vagaCadastrar.Titulo = vaga.Titulo;
                vagaCadastrar.Categoria = vaga.Categoria;
                vagaCadastrar.Descricao = vaga.Descricao;
                vagaCadastrar.CidadeEstado = vaga.Cidade_Estado;

                vagaCadastrar.Save();

                TempData["Mensagem"] = "O evento foi cadastrada com sucesso!";

                return RedirectToAction("Index");
            }

            return View(vaga);
        }

        [HttpGet]
        public IActionResult Visualizar(int Id)
        {

            Vaga_P1 vaga = new Vaga_P1(Id);
            vaga.CompleteObject();

            var IdfUsuarioLogado = HttpContext.Session.GetInt32("IdUsuarioLogado").GetValueOrDefault();

            List<VagaCandidatura> TodasCandidaturasUsuario = VagaCandidaturas_P1.TodasCandidaturasUsuario(IdfUsuarioLogado);
            var CandidatosBanco = TodasCandidaturasUsuario.Where(a => a.Id_Usuario == IdfUsuarioLogado && a.Id_Vaga == Id).FirstOrDefault();

            if (CandidatosBanco != null)
            {
                ViewBag.JaVoluntariado = true;
            }
            if (vaga.IdUsuarioAdm == IdfUsuarioLogado)
            {
                ViewBag.ADMVaga = true;
            }

            return View(vaga);
        }

        [HttpGet]
        public IActionResult Editar(int Id)
        {

            //ViewBag.Nivel = niveis;
            var IdfUsuarioLogado = HttpContext.Session.GetInt32("IdUsuarioLogado").GetValueOrDefault();

            Vaga_P1 vaga = new Vaga_P1(Id);
            vaga.CompleteObject();

            if (IdfUsuarioLogado == vaga.IdUsuarioAdm)
            {
                Mapper.CreateMap<Vaga_P1, Vaga>();
                Vaga VagaEdidar = Mapper.Map<Vaga>(vaga);

                ViewBag.CadastrarAtualizar = "Salvar";
                return View("Cadastrar", VagaEdidar);
            }

            TempData["MensagemErro"] = "Evento inacessível com seu usuário";
            return RedirectToAction("Index", "Vaga");
        }

        [HttpPost]
        public IActionResult Editar([FromForm] Vaga vaga)
        {

            ViewBag.CadastrarAtualizar = "Salvar";
            var IdfUsuarioLogado = HttpContext.Session.GetInt32("IdUsuarioLogado").GetValueOrDefault();

            if (ModelState.IsValid)
            {
                Vaga_P1 vagas = new Vaga_P1(vaga.Id_Vaga);
                vagas.CompleteObject();

                vagas.DataPublicacao = DateTime.Now;
                vagas.DataEvento = vaga.DataEvento;
                vagas.IdUsuarioAdm = IdfUsuarioLogado;
                vagas.Titulo = vaga.Titulo;
                vagas.Categoria = vaga.Categoria;
                vagas.Descricao = vaga.Descricao;
                vagas.CidadeEstado = vaga.Cidade_Estado;

                vagas.Save();

                TempData["Mensagem"] = "O evento foi atualizado com sucesso!";
                return RedirectToAction("MinhasVagas", "Perfil");
            }
            return View("Cadastrar", vaga);
        }

        [HttpGet]
        public IActionResult Excluir(int Id)
        {

            var IdfUsuarioLogado = HttpContext.Session.GetInt32("IdUsuarioLogado").GetValueOrDefault();

            if (Id != 0)
            {
                Vaga_P1 vaga = new Vaga_P1(Id);
                vaga.CompleteObject();

                if (IdfUsuarioLogado == vaga.IdUsuarioAdm)
                {
                    bool resultado = Vaga_P1.Delete(Id);

                }
            }
            return RedirectToAction("MinhasVagas", "Perfil");
        }

        [HttpGet]
        public IActionResult Voluntariar(int Id)
        {
            var IdfUsuarioLogado = HttpContext.Session.GetInt32("IdUsuarioLogado").GetValueOrDefault();

            Vaga_P1 vaga = new Vaga_P1(Id);
            vaga.CompleteObject();

            Usuario_P1 usuario = new Usuario_P1(IdfUsuarioLogado);
            usuario.CompleteObject();

            Usuario_P1 usuarioAdm = new Usuario_P1(vaga.IdUsuarioAdm);
            usuarioAdm.CompleteObject();

            List<VagaCandidatura> TodasCandidaturasUsuario = VagaCandidaturas_P1.TodasCandidaturasUsuario(IdfUsuarioLogado);
            var CandidatosBanco = TodasCandidaturasUsuario.Where(a => a.Id_Usuario == IdfUsuarioLogado && a.Id_Vaga == Id).FirstOrDefault();

            if (vaga.IdUsuarioAdm != IdfUsuarioLogado)
            {
                if (CandidatosBanco == null)
                {
                    VagaCandidaturas_P1 candidatarVaga = new VagaCandidaturas_P1();
                    candidatarVaga.IdUsuario = IdfUsuarioLogado;
                    candidatarVaga.IdVaga = Id;
                    candidatarVaga.DataCadastro = DateTime.Now;
                    candidatarVaga.Save();

                    ViewBag.JaVoluntariado = true;

                    if (usuario.Sexo == 1)
                    {
                        ViewBag.UsuarioSexo = "Masculino";
                    }
                    else if (usuario.Sexo == 2)
                    {
                        ViewBag.UsuarioSexo = "Feminino";
                    }
                    else
                    {
                        ViewBag.UsuarioSexo = "Prefiro não declarar";
                    }

                    EnviarEmail.EnviarMensagemContato(usuario, usuarioAdm.Email, Id);
                }
                else
                {
                    TempData["MensagemErro"] = "Você já está voluntariádo neste evento!";
                    ViewBag.JaVoluntariado = true;
                }
            }

            return View("Visualizar", vaga);
        }

        [HttpGet]
        public IActionResult ListaVoluntarios(int Id)
        {
            ViewBag.FooterPrecisa = false;

            List<VagaCandidatura> ListaUsuariosVoluntariados = VagaCandidaturas_P1.TodasUsuarioCandidatadosVaga(Id);

            List<Usuario_P1> voluntarios = new List<Usuario_P1>();
            List<int> IdfVoluntarios = new List<int>();

            //Salva todos ID dos usuários candidatados
            for (int i = 0; i < ListaUsuariosVoluntariados.Count; i++)
            {
                var idf = ListaUsuariosVoluntariados[i].Id_Usuario;
                IdfVoluntarios.Add(idf);
            }

            foreach (var IdUsu in IdfVoluntarios)
            {
                Usuario_P1 Usuario = new Usuario_P1(IdUsu);
                Usuario.CompleteObject();

                voluntarios.Add(Usuario);
            }

            var pageNumber = 1;

            var resultadoPaginado = voluntarios.ToPagedList(pageNumber, 10);

            return View(resultadoPaginado);

        }


    }
}
