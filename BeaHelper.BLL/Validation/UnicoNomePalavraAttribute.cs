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

            Vaga vaga = validationContext.ObjectInstance as Vaga;
            if (vaga.Titulo != null)
            {
                List<Vaga> vagas = Vaga_P2.BuscaTitulo(vaga.Titulo);
                if (vagas.Count == 0)
                {
                    return ValidationResult.Success;
                }
                else
                {
                    return new ValidationResult("O evento com titulo '" + vaga.Titulo + "' já está cadastrada!");
                }
            }
            else
            {
                return new ValidationResult("O Campo é Obrigatório!");
            }
            

            
        }
    }
}
