using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BeaHelper.BLL.Models {
    public class Usuario {
        public int Id_Usuario { get; set; }

        [Required(ErrorMessage = "O Campo é Obrigatório!")]
        [RegularExpression("^[a-zA-ZçÇáéíóúÁÉÍÓÚ ]+$", ErrorMessage = "O Nome só pode conter letras")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O Campo é Obrigatório!")]
        public int? Sexo { get; set; }

        [Required(ErrorMessage = "O Campo é Obrigatório!")]
        [EmailAddress(ErrorMessage = "O Email é inválido!")]
        public string Email { get; set; }

        [MinLength(15, ErrorMessage = "Número de celular incorreto!")]
        [MaxLength(15, ErrorMessage = "Número de celular incorreto!")]
        public string NumeroCelular { get; set; }

        [Required(ErrorMessage = "O Campo é Obrigatório!")]
        public string Senha { get; set; }

        public bool ManterConectado { get; set; }
    }
}
