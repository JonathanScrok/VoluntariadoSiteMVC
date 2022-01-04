using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BeaHelper.BLL.Models
{
    public class AlterarSenha
    {
        public int IdUsuario { get; set; }

        [Required(ErrorMessage = "O Campo é Obrigatório!")]
        public string Senha { get; set; }

        [Required(ErrorMessage = "O Campo é Obrigatório!")]
        public string ConfirmacaoSenha { get; set; }
    }
}
