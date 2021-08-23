﻿using SyrusVoluntariado.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace SyrusVoluntariado.Library.Mail {
    public class EnviarEmail {

        public static void EnviarMensagemContato(Usuario usuario, string emailAdm, int idVaga) {

            string hrefListaVagas = "https://localhost:44394/vaga/listavoluntarios/" + idVaga;

            string conteudo = string.Format("<p>Nome: {0}<br/> Email: {1}<br/> Sexo: {2}</p><p><a href='{3}'>Ver todos voluntários</a></p>", usuario.Nome, usuario.Email, usuario.Sexo, hrefListaVagas);

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
    }
}
