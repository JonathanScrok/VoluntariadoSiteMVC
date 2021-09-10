using AutoMapper;
using SyrusVoluntariado.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace SyrusVoluntariado.BLL
{
    public partial class VagaCandidaturas_P1
    {
        #region StringConnection
        private const string stringConnection = "Data Source=mssql-49550-0.cloudclusters.net,11255;Initial Catalog=be_helper;Integrated Security=False;User Id=AdminBeaHelper;Password=B3ah3lper#2021;MultipleActiveResultSets=True";
        #endregion

        #region Atributos

        private int _idCandidatura;
        private int _idVaga;
        private int _idUsuario;
        private DateTime _dataCadastro;

        private bool _persisted;
        private bool _modified;

        #endregion

        #region Propriedades

        #region IdCandidatura
        public int IdCandidatura
        {
            get
            {
                return this._idCandidatura;
            }
            set
            {
                this._idCandidatura = value;
                this._modified = true;
            }
        }
        #endregion

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

        #region IdUsuario
        public int IdUsuario
        {
            get
            {
                return this._idUsuario;
            }
            set
            {
                this._idUsuario = value;
                this._modified = true;
            }
        }
        #endregion

        #region DataCadastro

        public DateTime DataCadastro
        {
            get
            {
                return this._dataCadastro;
            }
            set
            {
                this._dataCadastro = value;
                this._modified = true;
            }
        }
        #endregion

        #endregion

        #region Construtores
        public VagaCandidaturas_P1()
        {
            this._persisted = false;
        }
        public VagaCandidaturas_P1(int IdCandidatura)
        {
            this._idCandidatura = IdCandidatura;
            this._persisted = true;
        }
        #endregion

        #region Consultas
        private const string SELECT_TODASCANDIDATURAS = @"select * from helper.VagaCandidaturas";
        private const string SELECT_BUSCA_CANDIDATURAID = @"select * from helper.VagaCandidaturas where Id_Candidatura = @Id_Candidatura";
        private const string SELECT_BUSCA_CANDIDATURA_IDUSUARIO = @"select * from helper.VagaCandidaturas where Id_Usuario = @Id_Usuario";

        private const string UPDATE_CANDIDATURA = @"UPDATE helper.VagaCandidaturas SET Id_Candidatura = @Id_Candidatura, Id_Vaga = @Id_Vaga, Id_Usuario = @Id_Usuario, DataCadastro = @DataCadastro where Id_Candidatura = @Id_Candidatura";
        private const string INSERT_CANDIDATURA = @"INSERT INTO helper.VagaCandidaturas(Id_Vaga, Id_Usuario, DataCadastro) VALUES (@Id_Vaga, @Id_Usuario, @DataCadastro)";
        private const string DELETE_CANDIDATURA = @"DELETE FROM helper.VagaCandidaturas WHERE Id_Candidatura = @Id_Candidatura";
        #endregion

        #region Metodos

        #region GetParameters

        private static List<SqlParameter> GetParameters()
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Id_Candidatura", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Id_Vaga", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Id_Usuario", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@DataCadastro", SqlDbType.DateTime, 8));

            return (parms);
        }
        #endregion

        #region SetParameters

        private void SetParameters(List<SqlParameter> parms)
        {
            parms[0].Value = this._idCandidatura;
            parms[1].Value = this._idVaga;
            parms[2].Value = this._idUsuario;
            parms[3].Value = this._dataCadastro;

        }
        #endregion

        #region Busca todas as Candidaturas do Banco
        public static List<VagaCandidatura> TodasCandidaturas()
        {
            SqlConnection conn = null;
            SqlDataReader reader = null;
            List<VagaCandidatura> Candidaturas = new List<VagaCandidatura>();

            try
            {
                conn = new SqlConnection(stringConnection);
                conn.Open();

                SqlCommand cmd = new SqlCommand(SELECT_TODASCANDIDATURAS, conn);

                Mapper.CreateMap<IDataRecord, VagaCandidatura>();

                using (reader = cmd.ExecuteReader())
                {
                    Candidaturas = Mapper.Map<List<VagaCandidatura>>(reader);
                    return Candidaturas;
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
        
        #region Busca todas as Candidaturas do Banco
        public static List<VagaCandidatura> TodasCandidaturasUsuario(int IdUsuario)
        {
            SqlConnection conn = null;
            SqlDataReader reader = null;
            List<VagaCandidatura> CandidaturasUsuario = new List<VagaCandidatura>();
           
            try
            {
                List<SqlParameter> parms = new List<SqlParameter>();
                parms.Add(new SqlParameter("@Id_Usuario", SqlDbType.Int, 4));

                parms[0].Value = IdUsuario;

                conn = new SqlConnection(stringConnection);
                conn.Open();

                SqlCommand cmd = new SqlCommand(SELECT_BUSCA_CANDIDATURA_IDUSUARIO, conn);
                cmd.Parameters.Add(parms[0]);

                Mapper.CreateMap<IDataRecord, VagaCandidatura>();

                using (reader = cmd.ExecuteReader())
                {
                    CandidaturasUsuario = Mapper.Map<List<VagaCandidatura>>(reader);
                    return CandidaturasUsuario;
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

        #region Insert

        private void Insert()
        {
            List<SqlParameter> parms = GetParameters();
            SetParameters(parms);

            using (SqlConnection conn = new SqlConnection(stringConnection))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        SqlCommand cmd = new SqlCommand(INSERT_CANDIDATURA, conn, trans);

                        for (int i = 0; i < parms.Count; i++)
                        {
                            cmd.Parameters.Add(parms[i]);
                        }

                        cmd.ExecuteNonQuery();
                        this._idCandidatura = Convert.ToInt32(cmd.Parameters["@Id_Candidatura"].Value);
                        cmd.Parameters.Clear();
                        this._persisted = true;
                        this._modified = false;
                        trans.Commit();

                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
        }

        private void Insert(SqlTransaction trans)
        {
            List<SqlParameter> parms = GetParameters();
            SetParameters(parms);
            SqlConnection conn = null;
            conn = new SqlConnection(stringConnection);
            conn.Open();


            SqlCommand cmd = new SqlCommand(INSERT_CANDIDATURA, conn, trans);

            for (int i = 0; i < parms.Count; i++)
            {
                cmd.Parameters.Add(parms[i]);
            }

            cmd.ExecuteNonQuery();
            this._idCandidatura = Convert.ToInt32(cmd.Parameters["@Id_Candidatura"].Value);
            cmd.Parameters.Clear();
            this._persisted = true;
            this._modified = false;

        }
        #endregion

        #region Update
        private void Update()
        {

            if (this._modified)
            {
                SqlConnection conn = null;
                conn = new SqlConnection(stringConnection);
                conn.Open();

                List<SqlParameter> parms = GetParameters();
                SetParameters(parms);
                SqlCommand cmd = new SqlCommand(UPDATE_CANDIDATURA, conn);

                for (int i = 0; i < parms.Count; i++)
                {
                    cmd.Parameters.Add(parms[i]);
                }

                cmd.ExecuteNonQuery();
                this._modified = false;
            }
        }

        private void Update(SqlTransaction trans)
        {
            if (this._modified)
            {
                SqlConnection conn = null;
                conn = new SqlConnection(stringConnection);
                conn.Open();

                List<SqlParameter> parms = GetParameters();
                SetParameters(parms);
                SqlCommand cmd = new SqlCommand(UPDATE_CANDIDATURA, conn);

                for (int i = 0; i < parms.Count; i++)
                {
                    cmd.Parameters.Add(parms[i]);
                }

                cmd.ExecuteNonQuery();
                this._modified = false;
            }
        }
        #endregion

        #region Save

        public void Save()
        {
            if (!this._persisted)
                this.Insert();
            else
                this.Update();
        }

        public void Save(SqlTransaction trans)
        {
            if (!this._persisted)
                this.Insert(trans);
            else
                this.Update(trans);
        }
        #endregion

        #region Delete
        public static bool Delete(int Id_Candidatura)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Id_Candidatura", SqlDbType.Int, 4));

            parms[0].Value = Id_Candidatura;

            SqlConnection conn = null;
            conn = new SqlConnection(stringConnection);
            conn.Open();

            SqlCommand cmd = new SqlCommand(DELETE_CANDIDATURA, conn);

            for (int i = 0; i < parms.Count; i++)
            {
                cmd.Parameters.Add(parms[i]);
            }

            var quantidade = cmd.ExecuteNonQuery();

            if (quantidade > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region CompleteObject
        /// <summary>
        /// Método utilizado para completar uma instância de Usuario a partir do banco de dados.
        /// </summary>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Jonathan Scrok</remarks>
        public bool CompleteObject()
        {
            using (IDataReader dr = LoadDataReader(this._idCandidatura))
            {
                return SetInstance(dr, this);
            }
        }
        /// <summary>
        /// Método utilizado para completar uma instância de Usuario a partir do banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Jonathan Scrok</remarks>
        public bool CompleteObject(SqlTransaction trans)
        {
            using (IDataReader dr = LoadDataReader(this._idCandidatura, trans))
            {
                return SetInstance(dr, this);
            }
        }
        public async Task<bool> CompleteObjectAsync(SqlTransaction trans)
        {
            using (IDataReader dr = await LoadDataReaderAsync(this._idCandidatura, trans))
            {
                return SetInstance(dr, this);
            }
        }
        #endregion

        #region LoadDataReader
        /// <summary>
        /// Método utilizado por retornar as colunas de um registro no banco de dados.
        /// </summary>
        /// <param name="idUsuario">Chave do registro.</param>
        /// <returns>Cursor de leitura do banco de dados.</returns>
        /// <remarks>Jonathan Scrok</remarks>
        private static IDataReader LoadDataReader(int IdCandidatura)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Id_Candidatura", SqlDbType.Int, 4));

            parms[0].Value = IdCandidatura;

            SqlConnection conn = null;
            conn = new SqlConnection(stringConnection);
            conn.Open();

            SqlCommand cmd = new SqlCommand(SELECT_BUSCA_CANDIDATURAID, conn);
            cmd.Parameters.Add(parms[0]);

            return cmd.ExecuteReader();
        }

        /// <summary>
        /// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idUsuario">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Cursor de leitura do banco de dados.</returns>
        /// <remarks>Jonathan Scrok</remarks>
        private static IDataReader LoadDataReader(int IdCandidatura, SqlTransaction trans)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Id_Candidatura", SqlDbType.Int, 4));

            parms[0].Value = IdCandidatura;

            SqlConnection conn = null;
            conn = new SqlConnection(stringConnection);
            conn.Open();

            SqlCommand cmd = new SqlCommand(SELECT_BUSCA_CANDIDATURAID, conn);
            cmd.Parameters.Add(parms[0]);

            return cmd.ExecuteReader();
        }
        private static async Task<IDataReader> LoadDataReaderAsync(int IdCandidatura, SqlTransaction trans)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Id_Candidatura", SqlDbType.Int, 4));

            parms[0].Value = IdCandidatura;
            SqlConnection conn = null;
            conn = new SqlConnection(stringConnection);
            conn.Open();

            SqlCommand cmd = new SqlCommand(SELECT_BUSCA_CANDIDATURAID, conn);
            cmd.Parameters.Add(parms[0]);

            return await cmd.ExecuteReaderAsync();
        }
        private static async Task<IDataReader> LoadDataReaderAsync(int Id_Candidatura)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Id_Candidatura", SqlDbType.Int, 4));

            SqlConnection conn = null;
            conn = new SqlConnection(stringConnection);
            conn.Open();

            SqlCommand cmd = new SqlCommand(SELECT_BUSCA_CANDIDATURAID, conn);
            cmd.Parameters.Add(parms[0]);

            return await cmd.ExecuteReaderAsync();
        }
        #endregion

        #region SetInstance
        /// <summary>
        /// Método auxiliar utilizado pelos métodos LoadObject e CompleteObject para percorrer um IDataReader e vincular as colunas com os atributos da classe.
        /// </summary>
        /// <param name="dr">Cursor de leitura do banco de dados.</param>
        /// <param name="objUsuario">Instância a ser manipulada.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Jonathan Scrok</remarks>
        private static bool SetInstance(IDataReader dr, VagaCandidaturas_P1 objVaga)
        {
            try
            {
                if (dr.Read())
                {
                    objVaga._idCandidatura = Convert.ToInt32(dr["Id_Candidatura"]);
                    objVaga._idVaga = Convert.ToInt32(dr["Id_Vaga"]);
                    objVaga._idUsuario = Convert.ToInt32(dr["Id_Usuario"]);
                    objVaga._dataCadastro = Convert.ToDateTime(dr["DataCadastro"]);


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

        #endregion
    }
}
