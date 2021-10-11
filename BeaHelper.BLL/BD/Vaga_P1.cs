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
    public partial class Vaga_P1
    {
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
        private DateTime? _dataEvento;
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

        public DateTime? DataEvento
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

        #region Construtores
        public Vaga_P1()
        {
            this._persisted = false;
        }
        public Vaga_P1(int idVaga)
        {
            this._idVaga = idVaga;
            this._persisted = true;
        }
        #endregion

        #region Consultas
        private const string SELECT_TODASVAGAS = @"select * from helper.Vagas order by DataPublicacao desc";
        private const string SELECT_ULTIMASVAGAS_TOP8 = @"select top 8 * from helper.Vagas order by DataEvento desc";
        private const string SELECT_BUSCAVAGAID = @"select * from helper.Vagas where Id_Vaga = @Id_Vaga";

        private const string UPDATE_VAGA = @"UPDATE helper.Vagas SET Id_Usuario_Adm = @Id_Usuario_Adm, Titulo = @Titulo, Categoria = @Categoria, Descricao = @Descricao, Cidade_Estado = @Cidade_Estado, DataPublicacao = @DataPublicacao, DataEvento = @DataEvento, SemData = @SemData, EventoRecorrente = @EventoRecorrente WHERE Id_Vaga = @Id_Vaga";
        private const string INSERT_VAGA = @"INSERT INTO helper.Vagas(Id_Usuario_Adm, Titulo, Categoria ,Descricao, Cidade_Estado, DataPublicacao, DataEvento, SemData, EventoRecorrente) VALUES (@Id_Usuario_Adm, @Titulo, @Categoria, @Descricao, @Cidade_Estado, @DataPublicacao, @DataEvento, @SemData, @EventoRecorrente)";
        private const string DELETE_VAGA = @"DELETE FROM helper.vagas WHERE Id_Vaga = @Id_Vaga";
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

            if (_dataEvento == null)
            {
                parms[7].Value = DBNull.Value;
            }
            else
            {
                parms[7].Value = this._dataEvento;
            }

            parms[8].Value = this._semData;
            parms[9].Value = this._eventoRecorrente;
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
                        SqlCommand cmd = new SqlCommand(INSERT_VAGA, conn, trans);

                        for (int i = 0; i < parms.Count; i++)
                        {
                            cmd.Parameters.Add(parms[i]);
                        }

                        cmd.ExecuteNonQuery();
                        this._idVaga = Convert.ToInt32(cmd.Parameters["@Id_Vaga"].Value);
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


            SqlCommand cmd = new SqlCommand(INSERT_VAGA, conn, trans);

            for (int i = 0; i < parms.Count; i++)
            {
                cmd.Parameters.Add(parms[i]);
            }

            cmd.ExecuteNonQuery();
            this._idVaga = Convert.ToInt32(cmd.Parameters["@Id_Vaga"].Value);
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
                SqlCommand cmd = new SqlCommand(UPDATE_VAGA, conn);

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
                SqlCommand cmd = new SqlCommand(UPDATE_VAGA, conn);

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
        public static bool Delete(int id_Vaga)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Id_Vaga", SqlDbType.Int, 4));

            parms[0].Value = id_Vaga;

            SqlConnection conn = null;
            conn = new SqlConnection(stringConnection);
            conn.Open();

            SqlCommand cmd = new SqlCommand(DELETE_VAGA, conn);

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
            using (IDataReader dr = LoadDataReader(this._idVaga))
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
            using (IDataReader dr = LoadDataReader(this._idVaga, trans))
            {
                return SetInstance(dr, this);
            }
        }
        public async Task<bool> CompleteObjectAsync(SqlTransaction trans)
        {
            using (IDataReader dr = await LoadDataReaderAsync(this._idVaga, trans))
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
        private static IDataReader LoadDataReader(int IdVaga)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Id_Vaga", SqlDbType.Int, 4));

            parms[0].Value = IdVaga;

            SqlConnection conn = null;
            conn = new SqlConnection(stringConnection);
            conn.Open();

            SqlCommand cmd = new SqlCommand(SELECT_BUSCAVAGAID, conn);
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
        private static IDataReader LoadDataReader(int IdVaga, SqlTransaction trans)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Id_Vaga", SqlDbType.Int, 4));

            parms[0].Value = IdVaga;

            SqlConnection conn = null;
            conn = new SqlConnection(stringConnection);
            conn.Open();

            SqlCommand cmd = new SqlCommand(SELECT_BUSCAVAGAID, conn);
            cmd.Parameters.Add(parms[0]);

            return cmd.ExecuteReader();
        }
        private static async Task<IDataReader> LoadDataReaderAsync(int IdVaga, SqlTransaction trans)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Id_Vaga", SqlDbType.Int, 4));

            parms[0].Value = IdVaga;
            SqlConnection conn = null;
            conn = new SqlConnection(stringConnection);
            conn.Open();

            SqlCommand cmd = new SqlCommand(SELECT_BUSCAVAGAID, conn);
            cmd.Parameters.Add(parms[0]);

            return await cmd.ExecuteReaderAsync();
        }
        private static async Task<IDataReader> LoadDataReaderAsync(int Id_Vaga)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Id_Vaga", SqlDbType.Int, 4));

            SqlConnection conn = null;
            conn = new SqlConnection(stringConnection);
            conn.Open();

            SqlCommand cmd = new SqlCommand(SELECT_BUSCAVAGAID, conn);
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
        private static bool SetInstance(IDataReader dr, Vaga_P1 objVaga)
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

                    if (dr["DataEvento"] == DBNull.Value)
                    {
                        objVaga._dataEvento = null;
                    }
                    else
                    {
                        objVaga._dataEvento = Convert.ToDateTime(dr["DataEvento"]);
                    }
                    objVaga._semData = Convert.ToBoolean(dr["SemData"]);
                    objVaga._eventoRecorrente = Convert.ToBoolean(dr["EventoRecorrente"]);

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
