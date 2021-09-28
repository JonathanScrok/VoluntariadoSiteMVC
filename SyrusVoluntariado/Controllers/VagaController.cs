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

            //var ValorUsuarioLogado = HttpContext.Session.GetInt32("IdUsuarioLogado"); //Remover ou Comentar
            //int IdUsuarioLog = ValorUsuarioLogado.GetValueOrDefault(); //Remover ou Comentar
            int IdUsuarioLogado = GetUsuarioLogado();

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

            //var IdfUsuarioLogado = HttpContext.Session.GetInt32("IdUsuarioLogado").GetValueOrDefault(); //Remover ou Comentar
            int IdUsuarioLogado = GetUsuarioLogado();

            List<VagaCandidatura> TodasCandidaturasUsuario = VagaCandidaturas_P1.TodasCandidaturasUsuario(IdUsuarioLogado);
            var CandidatosBanco = TodasCandidaturasUsuario.Where(a => a.Id_Usuario == IdUsuarioLogado && a.Id_Vaga == Id).FirstOrDefault();

            if (CandidatosBanco != null)
            {
                ViewBag.JaVoluntariado = true;
            }
            if (vaga.IdUsuarioAdm == IdUsuarioLogado)
            {
                ViewBag.ADMVaga = true;
            }

            return View(vaga);
        }

        [HttpGet]
        public IActionResult Editar(int Id)
        {

            //ViewBag.Nivel = niveis;
            //var IdfUsuarioLogado = HttpContext.Session.GetInt32("IdUsuarioLogado").GetValueOrDefault(); //Remover ou Comentar
            int IdUsuarioLogado = GetUsuarioLogado();

            Vaga_P1 vaga = new Vaga_P1(Id);
            vaga.CompleteObject();

            if (IdUsuarioLogado == vaga.IdUsuarioAdm)
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
            //var IdfUsuarioLogado = HttpContext.Session.GetInt32("IdUsuarioLogado").GetValueOrDefault(); //Remover ou Comentar
            int IdUsuarioLogado = GetUsuarioLogado();

            if (ModelState.IsValid)
            {
                Vaga_P1 vagas = new Vaga_P1(vaga.Id_Vaga);
                vagas.CompleteObject();

                vagas.DataPublicacao = DateTime.Now;
                vagas.DataEvento = vaga.DataEvento;
                vagas.IdUsuarioAdm = IdUsuarioLogado;
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
            //var IdfUsuarioLogado = HttpContext.Session.GetInt32("IdUsuarioLogado").GetValueOrDefault(); //Remover ou Comentar
            int IdUsuarioLogado = GetUsuarioLogado();

            if (Id != 0)
            {
                Vaga_P1 vaga = new Vaga_P1(Id);
                vaga.CompleteObject();

                if (IdUsuarioLogado == vaga.IdUsuarioAdm)
                {
                    bool resultado = Vaga_P1.Delete(Id);

                }
            }
            return RedirectToAction("MinhasVagas", "Perfil");
        }

        [HttpGet]
        public IActionResult Voluntariar(int Id)
        {
            //var IdfUsuarioLogado = HttpContext.Session.GetInt32("IdUsuarioLogado").GetValueOrDefault(); //Remover ou Comentar
            int IdUsuarioLogado = GetUsuarioLogado();

            Vaga_P1 vaga = new Vaga_P1(Id);
            vaga.CompleteObject();

            List<VagaCandidatura> TodasCandidaturasUsuario = VagaCandidaturas_P1.TodasCandidaturasUsuario(IdUsuarioLogado);
            var CandidatosBanco = TodasCandidaturasUsuario.Where(a => a.Id_Usuario == IdUsuarioLogado && a.Id_Vaga == Id).FirstOrDefault();

            if (vaga.IdUsuarioAdm != IdUsuarioLogado)
            {
                if (CandidatosBanco == null)
                {
                    VagaCandidaturas_P1 candidatarVaga = new VagaCandidaturas_P1();
                    candidatarVaga.IdUsuario = IdUsuarioLogado;
                    candidatarVaga.IdVaga = Id;
                    candidatarVaga.DataCadastro = DateTime.Now;
                    candidatarVaga.Save();

                    ViewBag.JaVoluntariado = true;

                    Usuario_P1 usuario = new Usuario_P1(IdUsuarioLogado);
                    usuario.CompleteObject();

                    //bool EmailEnviado = EnviarCandidatoParaDonoVaga(IdUsuarioLogado, vaga, usuario);
                }
                else
                {
                    TempData["MensagemErro"] = "Você já está voluntariádo neste evento!";
                    ViewBag.JaVoluntariado = true;
                }
            }

            return View("Visualizar", vaga);
        }

        public bool EnviarCandidatoParaDonoVaga(int IdUsuarioLogado, Vaga_P1 vaga, Usuario_P1 usuario)
        {
            try
            {
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

                Usuario_P1 usuarioAdm = new Usuario_P1(vaga.IdUsuarioAdm);
                usuarioAdm.CompleteObject();

                EnviarEmail.EnviarMensagemContato(usuario, usuarioAdm.Email, vaga.IdVaga);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        [HttpGet]
        public IActionResult ListaVoluntarios(int Id)
        {
            ViewBag.FooterPrecisa = false;
            ViewBag.IdVaga = Id;
            int IdUsuarioLogado = GetUsuarioLogado();

            List<VagaCandidatura> ListaUsuariosVoluntariados = VagaCandidaturas_P1.TodasUsuarioCandidatadosVaga(Id);

            List<UsuarioCompleto> voluntariosCompleto = new List<UsuarioCompleto>();
            List<int> IdfVoluntarios = new List<int>();

            //Lista todos ID dos usuários candidatados
            for (int i = 0; i < ListaUsuariosVoluntariados.Count; i++)
            {
                var idf = ListaUsuariosVoluntariados[i].Id_Usuario;
                IdfVoluntarios.Add(idf);
            }

            foreach (var IdUsu in IdfVoluntarios)
            {
                UsuarioCompleto UsuarioCompleto = new UsuarioCompleto();

                Usuario_P1 Usuario = new Usuario_P1(IdUsu);
                Usuario.CompleteObject();

                var JaAvaliado = Avaliacao_P1.BuscaIdUsuario_AvaliouEAvaliado(IdUsu, IdUsuarioLogado);
                
                var Avaliacao = Avaliacao_P1.TodasAvaliacoesUsuario(IdUsu);
                UsuarioCompleto.Id = Usuario.IdUsuario;
                UsuarioCompleto.Email = Usuario.Email;
                UsuarioCompleto.Nome = Usuario.Nome;
                UsuarioCompleto.Sexo = Usuario.Sexo;

                if (JaAvaliado.Count > 0)
                {
                    UsuarioCompleto.UsuarioLogadoAvaliou = true;
                }
                else
                {
                    UsuarioCompleto.UsuarioLogadoAvaliou = false;
                }

                if (Avaliacao.Count > 0)
                {
                    double NotaSomadas = 0;
                    for (int i = 0; i < Avaliacao.Count; i++)
                    {
                        NotaSomadas += Avaliacao[i].Nota;
                    }

                    UsuarioCompleto.NotaMedia = NotaSomadas / Avaliacao.Count;
                    UsuarioCompleto.NuncaAvaliado = false;
                }
                else
                {
                    UsuarioCompleto.NuncaAvaliado = true;
                }

                voluntariosCompleto.Add(UsuarioCompleto);
            }

            var pageNumber = 1;

            var resultadoPaginado = voluntariosCompleto.ToPagedList(pageNumber, 10);

            return View(resultadoPaginado);

        }

        public int GetUsuarioLogado()
        {
            int IdUsuarioLogado = Int32.Parse(HttpContext.Request.Cookies["IdUsuarioLogado"]);

            return IdUsuarioLogado;
        }


    }
}
