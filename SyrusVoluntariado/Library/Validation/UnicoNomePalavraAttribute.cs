using BeaHelper.BLL.BD;
using BeaHelper.BLL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BeaHelper.Library.Validation {
    public class UnicoNomePalavraAttribute : ValidationAttribute {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext) {

            Evento evento = validationContext.ObjectInstance as Evento;
            //var _db = (DatabaseContext)validationContext.GetService(typeof(DatabaseContext));
            List<Evento> vagas = Evento_P2.BuscaTitulo(evento.Titulo);
            var TitulosCadastrados = vagas.Where(a => a.Id_Evento != evento.Id_Evento).FirstOrDefault();

            //Já existe no banco 1 registro:
            // - Verificar se o nome existe
            // - Verificar se o Id é o mesmo do registro no banco.

            if (TitulosCadastrados == null) {
                return ValidationResult.Success;
            } else {
                return new ValidationResult("O evento com titulo '"+ evento.Titulo+"' já está cadastrado!");
            }
        }
    }
}
