using BeaHelper.BLL.BD;
using BeaHelper.BLL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BeaHelper.BLL.Validation {
    public class UnicoNomePalavraAttribute : ValidationAttribute {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext) {

            Evento evento = validationContext.ObjectInstance as Evento;
            if (evento.Titulo != null)
            {
                List<Evento> eventos = Evento_P2.BuscaTitulo(evento.Titulo);        
                var TitulosCadastrados = eventos.Where(a => a.Id_Evento != evento.Id_Evento).FirstOrDefault();

                if (TitulosCadastrados == null)
                {
                    return ValidationResult.Success;
                }
                else
                {
                    return new ValidationResult("O evento com titulo '" + evento.Titulo + "' já está cadastrado!");
                }
            }
            else
            {
                return new ValidationResult("O Campo é Obrigatório!");
            }
            

            
        }
    }
}
