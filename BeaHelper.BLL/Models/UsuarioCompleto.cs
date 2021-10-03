using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeaHelper.BLL.Models
{
    public class UsuarioCompleto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public int? Sexo { get; set; }
        public string Email { get; set; }
        public int Id_Avaliacao { get; set; }
        public double NotaMedia { get; set; }
        public bool NuncaAvaliado { get; set; }
        public bool UsuarioLogadoAvaliou { get; set; }


    }
}
