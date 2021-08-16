using SyrusVoluntariado.Database;
using SyrusVoluntariado.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SyrusVoluntariado.Library.Validation {
    public class UnicoNomePalavraAttribute : ValidationAttribute {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext) {

            Vaga vaga = validationContext.ObjectInstance as Vaga;
            var _db = (DatabaseContext)validationContext.GetService(typeof(DatabaseContext));

            //Já existe no banco 1 registro:
            // - Verificar se o nome existe
            // - Verificar se o Id é o mesmo do registro no banco.

            var palavraBanco =_db.Vagas.Where(a => a.Titulo == vaga.Titulo && a.Id != vaga.Id).FirstOrDefault();

            if (palavraBanco == null) {
                return ValidationResult.Success;
            } else {
                return new ValidationResult("A vaga com titulo '"+ vaga.Titulo+"' já está cadastrada!");
            }
        }
    }
}
