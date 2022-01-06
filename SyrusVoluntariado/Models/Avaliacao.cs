using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeaHelper.Models
{
    public class Avaliacao
    {
        public int Id_Avaliacao { get; set; }

        public int Id_Usuario { get; set; }

        public int Nota { get; set; }

        public DateTime DataCadastro { get; set; }
    }
}
