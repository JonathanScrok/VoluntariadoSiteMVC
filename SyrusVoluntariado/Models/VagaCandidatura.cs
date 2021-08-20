using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SyrusVoluntariado.Models {
    public class VagaCandidatura {

        public int Id { get; set; }

        public int Idf_Vaga { get; set; }

        public int Idf_Usuario_Candidatado { get; set; }
    }
}
