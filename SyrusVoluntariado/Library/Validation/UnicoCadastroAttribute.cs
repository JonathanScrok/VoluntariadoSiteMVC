using SyrusVoluntariado.BLL;
using SyrusVoluntariado.Database;
using SyrusVoluntariado.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SyrusVoluntariado.Library.Validation {
    public class UnicoCadastroAttribute : ValidationAttribute {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext) {

            Usuario usuario = validationContext.ObjectInstance as Usuario;
            //var _db = (DatabaseContext)validationContext.GetService(typeof(DatabaseContext));

            List<Usuario> usuarios = Usuario_P1.TodosUsuarios();

            // - Verificar se o nome existe
            // - Verificar se o Id é o mesmo do registro no banco.
            var UsuariosBanco = usuarios.Where(a => a.Email == usuario.Email && a.Id != usuario.Id).FirstOrDefault();

            if (UsuariosBanco == null) {
                return ValidationResult.Success;
            } else {
                return new ValidationResult("O email " + usuario.Email + " já está cadastrado!");
            }
        }
    }
}
