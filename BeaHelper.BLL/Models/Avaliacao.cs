using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeaHelper.BLL.Models
{
    public class Avaliacao
    {
        public int Id_Avaliacao { get; set; }

        public int Id_Usuario_Avaliado { get; set; }
        public int Id_Usuario_Avaliou { get; set; }

        public int Nota { get; set; }

        public DateTime DataCadastro { get; set; }
    }
}
