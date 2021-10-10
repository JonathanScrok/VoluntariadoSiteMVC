using AutoMapper;
using BeaHelper.BLL.Database;
using BeaHelper.BLL.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace BeaHelper.BLL.BD
{
    public class Vaga_P2 {

        #region StringConnection
        //private const string stringConnection = "Data Source=mssql-49550-0.cloudclusters.net,11255;Initial Catalog=be_helper;Integrated Security=False;User Id=AdminBeaHelper;Password=B3ah3lper#2021;MultipleActiveResultSets=True";
        private static string stringConnection = DbAcess.GetConnection();
        #endregion

        #region Atributos

        private int _idVaga;
        private int _idUsuarioAdm;
        private string _titulo;
        private string _categoria;
        private string _descricao;
        private string _cidadeEstado;
        private DateTime _dataPublicacao;
        private DateTime _dataEvento;
        private bool _semData;
        private bool _eventoRecorrente;

        private bool _persisted;
        private bool _modified;

        #endregion

        #region Propriedades

        #region IdVaga
        public int IdVaga
        {
            get
            {
                return this._idVaga;
            }
            set
            {
                this._idVaga = value;
                this._modified = true;
            }
        }
        #endregion

        #region IdUsuarioAdm
        public int IdUsuarioAdm
        {
            get
            {
                return this._idUsuarioAdm;
            }
            set
            {
                this._idUsuarioAdm = value;
                this._modified = true;
            }
        }
        #endregion

        #region Titulo

        public string Titulo
        {
            get
            {
                return this._titulo;
            }
            set
            {
                this._titulo = value;
                this._modified = true;
            }
        }
        #endregion

        #region Categoria

        public string Categoria
        {
            get
            {
                return this._categoria;
            }
            set
            {
                this._categoria = value;
                this._modified = true;
            }
        }
        #endregion

        #region Descricao

        public string Descricao
        {
            get
            {
                return this._descricao;
            }
            set
            {
                this._descricao = value;
                this._modified = true;
            }
        }
        #endregion

        #region CidadeEstado

        public string CidadeEstado
        {
            get
            {
                return this._cidadeEstado;
            }
            set
            {
                this._cidadeEstado = value;
                this._modified = true;
            }
        }
        #endregion

        #region DataPublicacao

        public DateTime DataPublicacao
        {
            get
            {
                return this._dataPublicacao;
            }
            set
            {
                this._dataPublicacao = value;
                this._modified = true;
            }
        }
        #endregion

        #region DataEvento

        public DateTime DataEvento
        {
            get
            {
                return this._dataEvento;
            }
            set
            {
                this._dataEvento = value;
                this._modified = true;
            }
        }
        #endregion

        #region SemData
        public bool SemData
        {
            get
            {
                return this._semData;
            }
            set
            {
                this._semData = value;
                this._modified = true;
            }
        }
        #endregion

        #region EventoRecorrente
        public bool EventoRecorrente
        {
            get
            {
                return this._eventoRecorrente;
            }
            set
            {
                this._eventoRecorrente = value;
                this._modified = true;
            }
        }
        #endregion

        #endregion

        #region Consultas
        private const string SELECT_TODASVAGAS = @"select * from helper.Vagas order by DataPublicacao desc";
        private const string SELECT_ULTIMASVAGAS_TOP8 = @"select top 8 * from helper.Vagas order by DataEvento desc";
        private const string SELECT_MINHASVAGAS = @"select * from helper.Vagas WHERE Id_Usuario_Adm = @Id_Usuario_Adm";
        private const string SELECT_TITULOS = @"select * from helper.Vagas WHERE Titulo = @Titulo";
        #endregion

        #region Metodos

        #region GetParameters

        private static List<SqlParameter> GetParameters()
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Id_Vaga", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Id_Usuario_Adm", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Titulo", SqlDbType.VarChar, 150));
            parms.Add(new SqlParameter("@Categoria", SqlDbType.VarChar, 100));
            parms.Add(new SqlParameter("@Descricao", SqlDbType.VarChar, 250));
            parms.Add(new SqlParameter("@Cidade_Estado", SqlDbType.VarChar, 50));
            parms.Add(new SqlParameter("@DataPublicacao", SqlDbType.DateTime, 8));
            parms.Add(new SqlParameter("@DataEvento", SqlDbType.SmallDateTime, 8));
            parms.Add(new SqlParameter("@SemData", SqlDbType.Bit));
            parms.Add(new SqlParameter("@EventoRecorrente", SqlDbType.Bit));

            return (parms);
        }
        #endregion

        #region SetParameters

        private void SetParameters(List<SqlParameter> parms)
        {
            parms[0].Value = this._idVaga;
            parms[1].Value = this._idUsuarioAdm;
            parms[2].Value = this._titulo;
            parms[3].Value = this._categoria;
            parms[4].Value = this._descricao;
            parms[5].Value = this._cidadeEstado;
            parms[6].Value = this._dataPublicacao;
            parms[7].Value = this._dataEvento;
            parms[8].Value = this._semData;
            parms[9].Value = this._eventoRecorrente;
        }
        #endregion

        #region Busca Top 8 ultimas vagas do Banco
        public static List<Vaga> Top8UltimasVagas()
        {
            SqlConnection conn = null;
            SqlDataReader reader = null;
            List<Vaga> Vagas = new List<Vaga>();

            try
            {
                conn = new SqlConnection(stringConnection);
                conn.Open();

                SqlCommand cmd = new SqlCommand(SELECT_ULTIMASVAGAS_TOP8, conn);

                Mapper.CreateMap<IDataRecord, Vaga>();

                using (reader = cmd.ExecuteReader())
                {
                    Vagas = Mapper.Map<List<Vaga>>(reader);
                    return Vagas;
                }
            }
            finally
            {

                if (reader != null)
                {
                    reader.Close();
                }

                if (conn != null)
                {
                    conn.Close();
                }
            }
        }
        #endregion

        #region Busca todas as Vagas do Banco
        public static List<Vaga> TodasVagas()
        {
            SqlConnection conn = null;
            SqlDataReader reader = null;
            List<Vaga> Vagas = new List<Vaga>();

            try
            {
                conn = new SqlConnection(stringConnection);
                conn.Open();

                SqlCommand cmd = new SqlCommand(SELECT_TODASVAGAS, conn);

                Mapper.CreateMap<IDataRecord, Vaga>();

                using (reader = cmd.ExecuteReader())
                {
                    Vagas = Mapper.Map<List<Vaga>>(reader);
                    return Vagas;
                }
            }
            finally
            {

                if (reader != null)
                {
                    reader.Close();
                }

                if (conn != null)
                {
                    conn.Close();
                }
            }
        }
        #endregion

        #region MinhasVagas por ID
        public static List<Vaga> MinhasVagas(int IdUsuarioAdm)
        {
            SqlConnection conn = null;
            SqlDataReader reader = null;
            List<Vaga> vaga = new List<Vaga>();

            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Id_Usuario_Adm", SqlDbType.Int, 4));
            parms[0].Value = IdUsuarioAdm;

            conn = new SqlConnection(stringConnection);
            conn.Open();

            SqlCommand cmd = new SqlCommand(SELECT_MINHASVAGAS, conn);
            cmd.Parameters.Add(parms[0]);

            Mapper.CreateMap<IDataRecord, Vaga>();

            using (reader = cmd.ExecuteReader())
            {
                vaga = Mapper.Map<List<Vaga>>(reader);
                return vaga;
            }
        }
        #endregion

        #region BuscaTitulo
        public static List<Vaga> BuscaTitulo(string titulo)
        {
            SqlConnection conn = null;
            SqlDataReader reader = null;
            List<Vaga> vaga = new List<Vaga>();

            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Titulo", SqlDbType.VarChar, 100));
            parms[0].Value = titulo;

            conn = new SqlConnection(stringConnection);
            conn.Open();

            SqlCommand cmd = new SqlCommand(SELECT_TITULOS, conn);
            cmd.Parameters.Add(parms[0]);

            Mapper.CreateMap<IDataRecord, Vaga>();

            using (reader = cmd.ExecuteReader())
            {
                vaga = Mapper.Map<List<Vaga>>(reader);
                return vaga;
            }
        }
        #endregion

        #endregion

        #region SetInstance
        /// <summary>
        /// Método auxiliar utilizado pelos métodos LoadObject e CompleteObject para percorrer um IDataReader e vincular as colunas com os atributos da classe.
        /// </summary>
        /// <param name="dr">Cursor de leitura do banco de dados.</param>
        /// <param name="objUsuario">Instância a ser manipulada.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Jonathan Scrok</remarks>
        private static bool SetInstance(IDataReader dr, Vaga_P2 objVaga)
        {
            try
            {
                if (dr.Read())
                {
                    objVaga._idVaga = Convert.ToInt32(dr["Id_Vaga"]);
                    objVaga._idUsuarioAdm = Convert.ToInt32(dr["Id_Usuario_Adm"]);
                    objVaga._titulo = Convert.ToString(dr["Titulo"]);
                    objVaga._descricao = Convert.ToString(dr["Descricao"]);
                    objVaga._categoria = Convert.ToString(dr["Categoria"]);
                    objVaga._cidadeEstado = Convert.ToString(dr["Cidade_Estado"]);
                    objVaga._dataPublicacao = Convert.ToDateTime(dr["DataPublicacao"]);
                    objVaga._dataEvento = Convert.ToDateTime(dr["DataEvento"]);
                    objVaga._semData = Convert.ToBoolean(dr["DataEvento"]);
                    objVaga._eventoRecorrente = Convert.ToBoolean(dr["DataEvento"]);

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                dr.Dispose();
            }
        }
        #endregion
    }

}
