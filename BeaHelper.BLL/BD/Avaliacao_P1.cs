using AutoMapper;
using BeaHelper.BLL.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using BeaHelper.BLL.Database;

namespace BeaHelper.BLL.BD
{
    public partial class Avaliacao_P1
    {
        #region StringConnection
        //private const string stringConnection = "Data Source=mssql-49550-0.cloudclusters.net,11255;Initial Catalog=be_helper;Integrated Security=False;User Id=AdminBeaHelper;Password=B3ah3lper#2021;MultipleActiveResultSets=True";
        private static string stringConnection = DbAcess.GetConnection();
        #endregion

        #region Atributos

        private int _idAvaliacao;
        private int _nota;
        private int _idUsuarioAvaliado;
        private int _idUsuarioAvaliou;
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
                return this._idAvaliacao;
            }
            set
            {
                this._idAvaliacao = value;
                this._modified = true;
            }
        }
        #endregion

        #region Nota
        public int Nota
        {
            get
            {
                return this._nota;
            }
            set
            {
                this._nota = value;
                this._modified = true;
            }
        }
        #endregion

        #region IdUsuarioAvaliado
        public int IdUsuarioAvaliado
        {
            get
            {
                return this._idUsuarioAvaliado;
            }
            set
            {
                this._idUsuarioAvaliado = value;
                this._modified = true;
            }
        }
        #endregion

        #region IdUsuarioAvaliado
        public int IdUsuarioAvaliou
        {
            get
            {
                return this._idUsuarioAvaliou;
            }
            set
            {
                this._idUsuarioAvaliou = value;
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
        public Avaliacao_P1()
        {
            this._persisted = false;
        }
        public Avaliacao_P1(int IdCandidatura)
        {
            this._idAvaliacao = IdCandidatura;
            this._persisted = true;
        }
        #endregion

        #region Consultas

        private const string SELECT_TODASAVALIACOES = @"select * from helper.Avaliacao";
        private const string SELECT_BUSCA_AVALIACOESID = @"select * from helper.Avaliacao where Id_Avaliacao = @Id_Avaliacao";
        private const string SELECT_BUSCA_AVALIACOES_IDUSUARIO = @"select * from helper.Avaliacao where Id_Usuario_Avaliado = @Id_Usuario_Avaliado";
        private const string SELECT_BUSCA_AVALIACOES_IDUSUARIOAVALIADO_E_IDUSUARIOAVALIOU = @"select * from helper.Avaliacao where Id_Usuario_Avaliado = @Id_Usuario_Avaliado AND Id_Usuario_Avaliou = @Id_Usuario_Avaliou";

        private const string UPDATE_AVALIACOES = @"UPDATE helper.Avaliacao SET Nota = @Nota, Id_Usuario_Avaliado = @Id_Usuario_Avaliado, Id_Usuario_Avaliou = @Id_Usuario_Avaliou, DataCadastro = @DataCadastro where Id_Avaliacao = @Id_Avaliacao";
        private const string INSERT_AVALIACOES = @"INSERT INTO helper.Avaliacao(Id_Usuario_Avaliado, Id_Usuario_Avaliou, Nota, DataCadastro) VALUES (@Id_Usuario_Avaliado, @Id_Usuario_Avaliou, @Nota, @DataCadastro)";
        private const string DELETE_AVALIACOES = @"DELETE FROM helper.Avaliacao WHERE Id_Avaliacao = @Id_Avaliacao";
        #endregion

        #region Metodos

        #region GetParameters

        private static List<SqlParameter> GetParameters()
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Id_Avaliacao", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Nota", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Id_Usuario_Avaliado", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Id_Usuario_Avaliou", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@DataCadastro", SqlDbType.DateTime, 8));

            return (parms);
        }
        #endregion

        #region SetParameters

        private void SetParameters(List<SqlParameter> parms)
        {
            parms[0].Value = this._idAvaliacao;
            parms[1].Value = this._nota;
            parms[2].Value = this._idUsuarioAvaliado;
            parms[3].Value = this._idUsuarioAvaliou;
            parms[4].Value = this._dataCadastro;

        }
        #endregion

        #region Busca todas as Avaliacoes
        public static List<Avaliacao> TodasAvaliacoes()
        {
            SqlConnection conn = null;
            SqlDataReader reader = null;
            List<Avaliacao> Candidaturas = new List<Avaliacao>();

            try
            {
                conn = new SqlConnection(stringConnection);
                conn.Open();

                SqlCommand cmd = new SqlCommand(SELECT_TODASAVALIACOES, conn);

                Mapper.CreateMap<IDataRecord, Avaliacao>();

                using (reader = cmd.ExecuteReader())
                {
                    Candidaturas = Mapper.Map<List<Avaliacao>>(reader);
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

        #region Busca todas as Avaliacoes por Id_Usuario_Avaliado
        public static List<Avaliacao> TodasAvaliacoesUsuario(int IdUsuarioAvaliado)
        {
            SqlConnection conn = null;
            SqlDataReader reader = null;
            List<Avaliacao> CandidaturasUsuario = new List<Avaliacao>();

            try
            {
                List<SqlParameter> parms = new List<SqlParameter>();
                parms.Add(new SqlParameter("@Id_Usuario_Avaliado", SqlDbType.Int, 4));

                parms[0].Value = IdUsuarioAvaliado;

                conn = new SqlConnection(stringConnection);
                conn.Open();

                SqlCommand cmd = new SqlCommand(SELECT_BUSCA_AVALIACOES_IDUSUARIO, conn);
                cmd.Parameters.Add(parms[0]);

                Mapper.CreateMap<IDataRecord, Avaliacao>();

                using (reader = cmd.ExecuteReader())
                {
                    CandidaturasUsuario = Mapper.Map<List<Avaliacao>>(reader);
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

        #region Busca as Avaliacoes por Id_Usuario_Avaliado e Id_Usuario_Avaliou
        public static List<Avaliacao> BuscaIdUsuario_AvaliouEAvaliado(int IdUsuarioAvaliado, int IdUsuarioAvaliou)
        {
            SqlConnection conn = null;
            SqlDataReader reader = null;
            List<Avaliacao> CandidaturasUsuario = new List<Avaliacao>();

            try
            {
                List<SqlParameter> parms = new List<SqlParameter>();
                parms.Add(new SqlParameter("@Id_Usuario_Avaliado", SqlDbType.Int, 4));
                parms.Add(new SqlParameter("@Id_Usuario_Avaliou", SqlDbType.Int, 4));

                parms[0].Value = IdUsuarioAvaliado;
                parms[1].Value = IdUsuarioAvaliou;

                conn = new SqlConnection(stringConnection);
                conn.Open();

                SqlCommand cmd = new SqlCommand(SELECT_BUSCA_AVALIACOES_IDUSUARIOAVALIADO_E_IDUSUARIOAVALIOU, conn);
                cmd.Parameters.Add(parms[0]);
                cmd.Parameters.Add(parms[1]);

                Mapper.CreateMap<IDataRecord, Avaliacao>();

                using (reader = cmd.ExecuteReader())
                {
                    CandidaturasUsuario = Mapper.Map<List<Avaliacao>>(reader);
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
                        SqlCommand cmd = new SqlCommand(INSERT_AVALIACOES, conn, trans);

                        for (int i = 0; i < parms.Count; i++)
                        {
                            cmd.Parameters.Add(parms[i]);
                        }

                        cmd.ExecuteNonQuery();
                        this._idAvaliacao = Convert.ToInt32(cmd.Parameters["@Id_Avaliacao"].Value);
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


            SqlCommand cmd = new SqlCommand(INSERT_AVALIACOES, conn, trans);

            for (int i = 0; i < parms.Count; i++)
            {
                cmd.Parameters.Add(parms[i]);
            }

            cmd.ExecuteNonQuery();
            this._idAvaliacao = Convert.ToInt32(cmd.Parameters["@Id_Avaliacao"].Value);
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
                SqlCommand cmd = new SqlCommand(UPDATE_AVALIACOES, conn);

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
                SqlCommand cmd = new SqlCommand(UPDATE_AVALIACOES, conn);

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
        public static bool Delete(int Id_Avaliacao)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Id_Avaliacao", SqlDbType.Int, 4));

            parms[0].Value = Id_Avaliacao;

            SqlConnection conn = null;
            conn = new SqlConnection(stringConnection);
            conn.Open();

            SqlCommand cmd = new SqlCommand(DELETE_AVALIACOES, conn);

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
            using (IDataReader dr = LoadDataReader(this._idAvaliacao))
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
            using (IDataReader dr = LoadDataReader(this._idAvaliacao, trans))
            {
                return SetInstance(dr, this);
            }
        }
        public async Task<bool> CompleteObjectAsync(SqlTransaction trans)
        {
            using (IDataReader dr = await LoadDataReaderAsync(this._idAvaliacao, trans))
            {
                return SetInstance(dr, this);
            }
        }
        #endregion

        #region LoadDataReader
        /// <summary>
        /// Método utilizado por retornar as colunas de um registro no banco de dados.
        /// </summary>
        /// <param name="IdUsuarioAvaliado">Chave do registro.</param>
        /// <returns>Cursor de leitura do banco de dados.</returns>
        /// <remarks>Jonathan Scrok</remarks>
        private static IDataReader LoadDataReader(int IdCandidatura)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Id_Avaliacao", SqlDbType.Int, 4));

            parms[0].Value = IdCandidatura;

            SqlConnection conn = null;
            conn = new SqlConnection(stringConnection);
            conn.Open();

            SqlCommand cmd = new SqlCommand(SELECT_BUSCA_AVALIACOESID, conn);
            cmd.Parameters.Add(parms[0]);

            return cmd.ExecuteReader();
        }

        /// <summary>
        /// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="IdUsuarioAvaliado">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Cursor de leitura do banco de dados.</returns>
        /// <remarks>Jonathan Scrok</remarks>
        private static IDataReader LoadDataReader(int IdCandidatura, SqlTransaction trans)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Id_Avaliacao", SqlDbType.Int, 4));

            parms[0].Value = IdCandidatura;

            SqlConnection conn = null;
            conn = new SqlConnection(stringConnection);
            conn.Open();

            SqlCommand cmd = new SqlCommand(SELECT_BUSCA_AVALIACOESID, conn);
            cmd.Parameters.Add(parms[0]);

            return cmd.ExecuteReader();
        }
        private static async Task<IDataReader> LoadDataReaderAsync(int IdCandidatura, SqlTransaction trans)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Id_Avaliacao", SqlDbType.Int, 4));

            parms[0].Value = IdCandidatura;
            SqlConnection conn = null;
            conn = new SqlConnection(stringConnection);
            conn.Open();

            SqlCommand cmd = new SqlCommand(SELECT_BUSCA_AVALIACOESID, conn);
            cmd.Parameters.Add(parms[0]);

            return await cmd.ExecuteReaderAsync();
        }
        private static async Task<IDataReader> LoadDataReaderAsync(int Id_Avaliacao)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Id_Avaliacao", SqlDbType.Int, 4));

            SqlConnection conn = null;
            conn = new SqlConnection(stringConnection);
            conn.Open();

            SqlCommand cmd = new SqlCommand(SELECT_BUSCA_AVALIACOESID, conn);
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
        private static bool SetInstance(IDataReader dr, Avaliacao_P1 objVaga)
        {
            try
            {
                if (dr.Read())
                {
                    objVaga._idAvaliacao = Convert.ToInt32(dr["Id_Avaliacao"]);
                    objVaga._nota = Convert.ToInt32(dr["Nome"]);
                    objVaga._idUsuarioAvaliado = Convert.ToInt32(dr["Id_Usuario_Avaliado"]);
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
