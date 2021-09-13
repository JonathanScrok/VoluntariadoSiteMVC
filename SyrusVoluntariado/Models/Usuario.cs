using SyrusVoluntariado.Library.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SyrusVoluntariado.Models {
    public class Usuario {

        public int Id { get; set; }

        [Required(ErrorMessage = "O Campo é Obrigatório!")]
        [RegularExpression("^[a-zA-ZçÇáéíóúÁÉÍÓÚ ]+$", ErrorMessage = "O Nome só pode conter letras")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O Campo é Obrigatório!")]
        public int? Sexo { get; set; }

        [Required(ErrorMessage = "O Campo é Obrigatório!")]
        [EmailAddress(ErrorMessage = "O Email é inválido!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "O Campo é Obrigatório!")]
        public string Senha { get; set; }
    }
}
