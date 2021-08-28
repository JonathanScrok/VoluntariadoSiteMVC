using AutoMapper;
using SyrusVoluntariado.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace SyrusVoluntariado.BLL {
    public class Vaga_P2 {
        private const string stringConnection = "Data Source=DESKTOP-MF80E68;Initial Catalog=be_helper;Integrated Security=False;User Id=sa;Password=b3ah3lper#2021;MultipleActiveResultSets=True";

        private const string SELECT_TODASVAGAS = @"select * from helper.Vagas";

        private const string SELECT_TODOSUSUARIOS = @"select * from helper.Usuarios";

       
        #region Busca todas as Vagas do Banco
        public static List<Vaga> TodasVagas() {
            SqlConnection conn = null;
            SqlDataReader reader = null;
            List<Vaga> Vagas = new List<Vaga>();

            try {
                conn = new SqlConnection(stringConnection);
                conn.Open();

                SqlCommand cmd = new SqlCommand(SELECT_TODASVAGAS, conn);

                Mapper.CreateMap<IDataRecord, Vaga>();

                using (reader = cmd.ExecuteReader()) {
                    Vagas = Mapper.Map<List<Vaga>>(reader);
                    return Vagas;
                }
            } finally {

                if (reader != null) {
                    reader.Close();
                }

                if (conn != null) {
                    conn.Close();
                }
            }
        }
        #endregion

        #region Busca todos os Usuários do Banco
        public static List<Usuario> TodosUsuarios() {
            SqlConnection conn = null;
            SqlDataReader reader = null;
            List<Usuario> usuarios = new List<Usuario>();

            try {
                conn = new SqlConnection(stringConnection);
                conn.Open();

                SqlCommand cmd = new SqlCommand(SELECT_TODOSUSUARIOS, conn);

                Mapper.CreateMap<IDataRecord, Usuario>();

                using (reader = cmd.ExecuteReader()) {
                    usuarios = Mapper.Map<List<Usuario>>(reader);
                    return usuarios;
                }
            } finally {

                if (reader != null) {
                    reader.Close();
                }

                if (conn != null) {
                    conn.Close();
                }
            }
        }
        #endregion

    }

}
