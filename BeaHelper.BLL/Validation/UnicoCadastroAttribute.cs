using BeaHelper.BLL.BD;
using BeaHelper.BLL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BeaHelper.BLL.Validation {
    public class UnicoCadastroAttribute : ValidationAttribute {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext) {

            Usuario usuario = validationContext.ObjectInstance as Usuario;
            //var _db = (DatabaseContext)validationContext.GetService(typeof(DatabaseContext));

            List<Usuario> usuarios = Usuario_P2.BuscaUsuario_Email(usuario.Email);

            if (usuarios.Count == 0) {
                return ValidationResult.Success;
            } else {
                return new ValidationResult("O email " + usuario.Email + " já está cadastrado!");
            }
        }
    }
}
