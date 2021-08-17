using SyrusVoluntariado.Library.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SyrusVoluntariado.Models {
    public class Vaga {

        public int Id { get; set; }

        [Required(ErrorMessage = "O Campo é Obrigatório!")]
        [MaxLength(20, ErrorMessage = "O Campo deve possuir no máximo 20 caracteres!")]
        //[UnicoNomePalavra]
        public string Titulo { get; set; }

        [Required(ErrorMessage = "O Campo é Obrigatório!")]
        [MaxLength(10, ErrorMessage = "O Campo deve possuir no máximo 10 caracteres!")]
        public string Categoria { get; set; }

        [Required(ErrorMessage = "O Campo é Obrigatório!")]
        [MaxLength(250, ErrorMessage = "O Campo deve possuir no máximo 250 caracteres!")]
        public string Descricao { get; set; }

        [Required(ErrorMessage = "O Campo é Obrigatório!")]
        [MaxLength(10, ErrorMessage = "O Campo deve possuir no máximo 10 caracteres!")]
        public string Local { get; set; }
    }
}
