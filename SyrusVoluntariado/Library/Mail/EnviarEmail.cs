using BeaHelper.BLL.BD;
using BeaHelper.BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace BeaHelper.Library.Mail {
    public class EnviarEmail {

        public static void EnviarMensagemContato(Usuario_P1 usuario, string emailAdm, int idEvento) {

            string hrefListaEventos = "https://beahelper.herokuapp.com/evento/listavoluntarios/" + idEvento;
            string Sexo;

            if (usuario.Sexo == 1)
            {
                Sexo = "Masculino";
            }
            else if (usuario.Sexo == 2)
            {
                Sexo = "Feminino";
            }
            else
            {
                Sexo = "Prefiro não declarar";
            }

            string conteudo = string.Format("<p>Nome: {0}<br/> Email: {1}<br/> Sexo: {2}</p><p><a href='{3}'>Ver todos voluntários</a></p>", usuario.Nome, usuario.Email, Sexo, hrefListaEventos);

            //Configurar Servidor SMTP
            SmtpClient smtp = new SmtpClient(Constants.ServidorSMTP, Constants.PortaSMTP);
            smtp.EnableSsl = true;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential(Constants.Usuario, Constants.Senha);
            smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;

            //Mensagem de Email
            MailMessage mensagem = new MailMessage();
            mensagem.From = new MailAddress("tutorialaprendizdedev@gmail.com"); //Remetente - Quem está enviando
            mensagem.To.Add(emailAdm); //Destinatário - Quem recebe a mensagem
            mensagem.Subject = "Novos Voluntários para seu Evento!"; //Assunto do email
            mensagem.IsBodyHtml = true; //O corpo do email é um HTML - Verdadeiro
            mensagem.Body = "<h1>Você tem um novo voluntário para seu Evento:</h1>" + conteudo; //Corpo do email em HTML, neste caso!

            smtp.Send(mensagem);
        }

        public static void EnviarRecuperacaoSenha(Usuario_P1 usuario)
        {
            //string link = "";
            string link = "https://localhost:44394/login/novasenha/"+ usuario.IdUsuario;

            string conteudo = string.Format("<p>Olá Senhor(a): {0}<br/>Verificamos que solicitou a recuperação de senha em nosso site. <br/>Para recuperar sua senha clique no link a baixo. <br/>" + link, usuario.Nome);

            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress("tutorialaprendizdedev@gmail.com");
                mail.To.Add(usuario.Email);
                mail.Subject = "Recuperação de senha!";
                mail.Body = "<h1>Solicitação de Recuperação de Senha.</h1>" + conteudo;
                mail.IsBodyHtml = true;
                //mail.Attachments.Add(new Attachment("C:\\file.zip"));

                using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtp.Credentials = new NetworkCredential(Constants.Usuario, Constants.Senha);
                    smtp.EnableSsl = true;
                    smtp.Send(mail);
                }
            }
        }
    }
}
