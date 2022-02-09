using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeaHelper.BLL.Models
{
    public class Notificacao
    {
        public int Id_Notificacao { get; set; }
        public string Descricao { get; set; }
        public int Id_Usuario_Notificado { get; set; }
        public int Id_Usuario_Notificou { get; set; }
        public string Url_Notificacao { get; set; }
        public bool NotificacaoAtiva { get; set; }
        public DateTime DataCadastro { get; set; }
    }
}
