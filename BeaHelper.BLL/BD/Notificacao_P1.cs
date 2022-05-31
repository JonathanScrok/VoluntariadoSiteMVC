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
    public partial class Notificacao_P1
    {
        #region StringConnection
        private static string stringConnection = DbAcess.GetConnection();
        #endregion

        #region Atributos

        private int _idNotificacao;
        private string _descricao;
        private string _urlNotificacao;
        private int _idUsuarioNotificado;
        private int? _idUsuarioNotificou;
        private bool _notificacaoAtiva;
        private bool? _flg_Visualizado;
        private DateTime _dataCadastro;

        private bool _persisted;
        private bool _modified;

        #endregion

        #region Propriedades

        #region IdNotificacao
        public int IdNotificacao
        {
            get
            {
                return this._idNotificacao;
            }
            set
            {
                this._idNotificacao = value;
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

        #region UrlNotificacao
        public string UrlNotificacao
        {
            get
            {
                return this._urlNotificacao;
            }
            set
            {
                this._urlNotificacao = value;
                this._modified = true;
            }
        }
        #endregion

        #region IdUsuarioNotificado
        public int IdUsuarioNotificado
        {
            get
            {
                return this._idUsuarioNotificado;
            }
            set
            {
                this._idUsuarioNotificado = value;
                this._modified = true;
            }
        }
        #endregion

        #region IdUsuarioNotificou
        public int? IdUsuarioNotificou
        {
            get
            {
                return this._idUsuarioNotificou;
            }
            set
            {
                this._idUsuarioNotificou = value;
                this._modified = true;
            }
        }
        #endregion

        #region NotificacaoAtiva
        public bool NotificacaoAtiva
        {
            get
            {
                return this._notificacaoAtiva;
            }
            set
            {
                this._notificacaoAtiva = value;
                this._modified = true;
            }
        }
        #endregion

        #region Flg_Visualizado
        public bool? Flg_Visualizado
        {
            get
            {
                return this._flg_Visualizado;
            }
            set
            {
                this._flg_Visualizado = value;
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
        public Notificacao_P1()
        {
            this._persisted = false;
        }
        public Notificacao_P1(int IdNotificacao)
        {
            this._idNotificacao = IdNotificacao;
            this._persisted = true;
        }
        #endregion

        #region Consultas

        private const string SELECT_TODASNOTIFICACOES = @"select * from helper.Notificacao";
        private const string SELECT_BUSCA_NOTIFICACOESID = @"select * from helper.Notificacao where Id_Notificacao = @Id_Notificacao";
        private const string SELECT_BUSCA_NOTIFICACOES_IDUSUARIO = @"select * from helper.Notificacao where Id_Usuario_Notificado = @Id_Usuario_Notificado";
        private const string SELECT_BUSCA_NOTIFICACOES_IDUSUARIO_ATIVAS = @"select * from helper.Notificacao where Id_Usuario_Notificado = @Id_Usuario_Notificado and NotificacaoAtiva = @NotificacaoAtiva";
        private const string SELECT_NOTIFICACOES_RECENTES_ATIVAS = @"select top 3 * from helper.Notificacao where Id_Usuario_Notificado = @Id_Usuario_Notificado and NotificacaoAtiva = @NotificacaoAtiva order by DataCadastro asc";
        private const string SELECT_BUSCA_NOTIFICACOES_IDUSUARIO_ATIVASCOUNT = @"select Count(*) from helper.Notificacao where Id_Usuario_Notificado = @Id_Usuario_Notificado and NotificacaoAtiva = @NotificacaoAtiva and Flg_Visualizado is null";
        private const string SELECT_BUSCA_NOTIFICACOES_IDUSUARIOAVALIADO_E_IDUSUARIOAVALIOU = @"select * from helper.Notificacao where Id_Usuario_Notificado = @Id_Usuario_Notificado AND Id_Usuario_Notificou = @Id_Usuario_Notificou";

        private const string UPDATE_NOTIFICACOES = @"UPDATE helper.Notificacao SET Descricao = @Descricao, Id_Usuario_Notificado = @Id_Usuario_Notificado, Id_Usuario_Notificou = @Id_Usuario_Notificou, Url_Notificacao = @Url_Notificacao, NotificacaoAtiva = @NotificacaoAtiva, Flg_Visualizado = @Flg_Visualizado, DataCadastro = @DataCadastro where Id_Notificacao = @Id_Notificacao";
        private const string INSERT_NOTIFICACOES = @"INSERT INTO helper.Notificacao(Id_Usuario_Notificado, Id_Usuario_Notificou, Descricao, Url_Notificacao, NotificacaoAtiva, Flg_Visualizado, DataCadastro) VALUES (@Id_Usuario_Notificado, @Id_Usuario_Notificou, @Descricao, @Url_Notificacao, @NotificacaoAtiva, @Flg_Visualizado, @DataCadastro)";
        private const string DELETE_NOTIFICACOES = @"DELETE FROM helper.Notificacao WHERE Id_Notificacao = @Id_Notificacao";
        #endregion

        #region Metodos

        #region GetParameters

        private static List<SqlParameter> GetParameters()
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Id_Notificacao", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Descricao", SqlDbType.VarChar, 5000));
            parms.Add(new SqlParameter("@Url_Notificacao", SqlDbType.VarChar, 5000));
            parms.Add(new SqlParameter("@Id_Usuario_Notificado", SqlDbType.BigInt));
            parms.Add(new SqlParameter("@Id_Usuario_Notificou", SqlDbType.BigInt));
            parms.Add(new SqlParameter("@NotificacaoAtiva", SqlDbType.Bit, 1));
            parms.Add(new SqlParameter("@Flg_Visualizado", SqlDbType.Bit, 1));
            parms.Add(new SqlParameter("@DataCadastro", SqlDbType.DateTime, 8));

            return (parms);
        }
        #endregion

        #region SetParameters

        private void SetParameters(List<SqlParameter> parms)
        {
            parms[0].Value = this._idNotificacao;
            parms[1].Value = this._descricao;
            parms[2].Value = this._urlNotificacao;
            parms[3].Value = this._idUsuarioNotificado;

            if (this._idUsuarioNotificou == null)
                parms[4].Value = DBNull.Value;
            else
                parms[4].Value = this._idUsuarioNotificou;

            parms[5].Value = this._notificacaoAtiva;
            
            if (this._flg_Visualizado == null)
                parms[6].Value = DBNull.Value;
            else
                parms[6].Value = this._flg_Visualizado;

            parms[7].Value = this._dataCadastro;

        }
        #endregion

        #region Busca todas as Notificações por Id_Usuario_Notificado
        public static List<Notificacao> TodasNotificacoesUsuario(int IdUsuarioNotificado)
        {
            SqlConnection conn = null;
            SqlDataReader reader = null;
            List<Notificacao> CandidaturasUsuario = new List<Notificacao>();

            try
            {
                List<SqlParameter> parms = new List<SqlParameter>();
                parms.Add(new SqlParameter("@Id_Usuario_Notificado", SqlDbType.Int, 4));

                parms[0].Value = IdUsuarioNotificado;

                conn = new SqlConnection(stringConnection);
                conn.Open();

                SqlCommand cmd = new SqlCommand(SELECT_BUSCA_NOTIFICACOES_IDUSUARIO, conn);
                cmd.Parameters.Add(parms[0]);

                Mapper.CreateMap<IDataRecord, Notificacao>();

                using (reader = cmd.ExecuteReader())
                {
                    CandidaturasUsuario = Mapper.Map<List<Notificacao>>(reader);
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

        #region Busca todas as Notificações por Id_Usuario_Notificado e ATIVA
        public static List<Notificacao> TodasNotificacoesUsuarioAtiva(int IdUsuarioNotificado)
        {
            SqlConnection conn = null;
            SqlDataReader reader = null;
            List<Notificacao> CandidaturasUsuario = new List<Notificacao>();

            try
            {
                List<SqlParameter> parms = new List<SqlParameter>();
                parms.Add(new SqlParameter("@Id_Usuario_Notificado", SqlDbType.Int, 4));
                parms.Add(new SqlParameter("@NotificacaoAtiva", SqlDbType.Bit, 1));

                parms[0].Value = IdUsuarioNotificado;
                parms[1].Value = 1;

                conn = new SqlConnection(stringConnection);
                conn.Open();

                SqlCommand cmd = new SqlCommand(SELECT_BUSCA_NOTIFICACOES_IDUSUARIO_ATIVAS, conn);
                cmd.Parameters.Add(parms[0]);
                cmd.Parameters.Add(parms[1]);

                Mapper.CreateMap<IDataRecord, Notificacao>();

                using (reader = cmd.ExecuteReader())
                {
                    CandidaturasUsuario = Mapper.Map<List<Notificacao>>(reader);
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

        #region Busca as Notificações por Id_Usuario_Notificado e Id_Usuario_Notificou
        public static List<Notificacao> BuscaIdUsuario_NotificouENotificado(int IdUsuarioNotificado, int IdUsuarioNotificou)
        {
            SqlConnection conn = null;
            SqlDataReader reader = null;
            List<Notificacao> CandidaturasUsuario = new List<Notificacao>();

            try
            {
                List<SqlParameter> parms = new List<SqlParameter>();
                parms.Add(new SqlParameter("@Id_Usuario_Notificado", SqlDbType.Int, 4));
                parms.Add(new SqlParameter("@Id_Usuario_Notificou", SqlDbType.Int, 4));

                parms[0].Value = IdUsuarioNotificado;
                parms[1].Value = IdUsuarioNotificou;

                conn = new SqlConnection(stringConnection);
                conn.Open();

                SqlCommand cmd = new SqlCommand(SELECT_BUSCA_NOTIFICACOES_IDUSUARIOAVALIADO_E_IDUSUARIOAVALIOU, conn);
                cmd.Parameters.Add(parms[0]);
                cmd.Parameters.Add(parms[1]);

                Mapper.CreateMap<IDataRecord, Notificacao>();

                using (reader = cmd.ExecuteReader())
                {
                    CandidaturasUsuario = Mapper.Map<List<Notificacao>>(reader);
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

        #region Conta todas as Notificações por Id_Usuario_Notificado, ATIVA e Não Visualizada
        public static int CountTodasNotificacoesUsuarioAtiva(int IdUsuarioNotificado)
        {
            SqlConnection conn = null;
            SqlDataReader reader = null;

            try
            {
                List<SqlParameter> parms = new List<SqlParameter>();
                parms.Add(new SqlParameter("@Id_Usuario_Notificado", SqlDbType.Int, 4));
                parms.Add(new SqlParameter("@NotificacaoAtiva", SqlDbType.Bit, 1));

                parms[0].Value = IdUsuarioNotificado;
                parms[1].Value = 1;

                conn = new SqlConnection(stringConnection);
                conn.Open();

                SqlCommand cmd = new SqlCommand(SELECT_BUSCA_NOTIFICACOES_IDUSUARIO_ATIVASCOUNT, conn);
                cmd.Parameters.Add(parms[0]);
                cmd.Parameters.Add(parms[1]);

                return Convert.ToInt32(cmd.ExecuteScalar());
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

        #region Busca Top 3 Notificações por Id_Usuario_Notificado, ATIVA e 
        public async static Task<List<Notificacao>> NotificacoesRecentes(int IdUsuarioNotificado)
        {
            SqlConnection conn = null;
            SqlDataReader reader = null;
            List<Notificacao> CandidaturasUsuario = new List<Notificacao>();

            try
            {
                List<SqlParameter> parms = new List<SqlParameter>();
                parms.Add(new SqlParameter("@Id_Usuario_Notificado", SqlDbType.Int, 4));
                parms.Add(new SqlParameter("@NotificacaoAtiva", SqlDbType.Bit, 1));

                parms[0].Value = IdUsuarioNotificado;
                parms[1].Value = 1;

                conn = new SqlConnection(stringConnection);
                conn.Open();

                SqlCommand cmd = new SqlCommand(SELECT_NOTIFICACOES_RECENTES_ATIVAS, conn);
                cmd.Parameters.Add(parms[0]);
                cmd.Parameters.Add(parms[1]);

                Mapper.CreateMap<IDataRecord, Notificacao>();

                using (reader = await cmd.ExecuteReaderAsync())
                {
                    CandidaturasUsuario = Mapper.Map<List<Notificacao>>(reader);
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
                        SqlCommand cmd = new SqlCommand(INSERT_NOTIFICACOES, conn, trans);

                        for (int i = 0; i < parms.Count; i++)
                        {
                            cmd.Parameters.Add(parms[i]);
                        }

                        cmd.ExecuteNonQuery();
                        this._idNotificacao = Convert.ToInt32(cmd.Parameters["@Id_Notificacao"].Value);
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


            SqlCommand cmd = new SqlCommand(INSERT_NOTIFICACOES, conn, trans);

            for (int i = 0; i < parms.Count; i++)
            {
                cmd.Parameters.Add(parms[i]);
            }

            cmd.ExecuteNonQuery();
            this._idNotificacao = Convert.ToInt32(cmd.Parameters["@Id_Notificacao"].Value);
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
                SqlCommand cmd = new SqlCommand(UPDATE_NOTIFICACOES, conn);

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
                SqlCommand cmd = new SqlCommand(UPDATE_NOTIFICACOES, conn);

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
        public static bool Delete(int Id_Notificacao)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Id_Notificacao", SqlDbType.Int, 4));

            parms[0].Value = Id_Notificacao;

            SqlConnection conn = null;
            conn = new SqlConnection(stringConnection);
            conn.Open();

            SqlCommand cmd = new SqlCommand(DELETE_NOTIFICACOES, conn);

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
            using (IDataReader dr = LoadDataReader(this._idNotificacao))
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
            using (IDataReader dr = LoadDataReader(this._idNotificacao, trans))
            {
                return SetInstance(dr, this);
            }
        }
        public async Task<bool> CompleteObjectAsync(SqlTransaction trans)
        {
            using (IDataReader dr = await LoadDataReaderAsync(this._idNotificacao, trans))
            {
                return SetInstance(dr, this);
            }
        }
        #endregion

        #region LoadDataReader
        /// <summary>
        /// Método utilizado por retornar as colunas de um registro no banco de dados.
        /// </summary>
        /// <param name="IdUsuarioNotificado">Chave do registro.</param>
        /// <returns>Cursor de leitura do banco de dados.</returns>
        /// <remarks>Jonathan Scrok</remarks>
        private static IDataReader LoadDataReader(int IdNotificacao)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Id_Notificacao", SqlDbType.Int, 4));

            parms[0].Value = IdNotificacao;

            SqlConnection conn = null;
            conn = new SqlConnection(stringConnection);
            conn.Open();

            SqlCommand cmd = new SqlCommand(SELECT_BUSCA_NOTIFICACOESID, conn);
            cmd.Parameters.Add(parms[0]);

            return cmd.ExecuteReader();
        }

        /// <summary>
        /// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="IdUsuarioNotificado">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Cursor de leitura do banco de dados.</returns>
        /// <remarks>Jonathan Scrok</remarks>
        private static IDataReader LoadDataReader(int IdNotificacao, SqlTransaction trans)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Id_Notificacao", SqlDbType.Int, 4));

            parms[0].Value = IdNotificacao;

            SqlConnection conn = null;
            conn = new SqlConnection(stringConnection);
            conn.Open();

            SqlCommand cmd = new SqlCommand(SELECT_BUSCA_NOTIFICACOESID, conn);
            cmd.Parameters.Add(parms[0]);

            return cmd.ExecuteReader();
        }
        private static async Task<IDataReader> LoadDataReaderAsync(int IdNotificacao, SqlTransaction trans)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Id_Notificacao", SqlDbType.Int, 4));

            parms[0].Value = IdNotificacao;
            SqlConnection conn = null;
            conn = new SqlConnection(stringConnection);
            conn.Open();

            SqlCommand cmd = new SqlCommand(SELECT_BUSCA_NOTIFICACOESID, conn);
            cmd.Parameters.Add(parms[0]);

            return await cmd.ExecuteReaderAsync();
        }
        private static async Task<IDataReader> LoadDataReaderAsync(int Id_Notificacao)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Id_Notificacao", SqlDbType.Int, 4));

            SqlConnection conn = null;
            conn = new SqlConnection(stringConnection);
            conn.Open();

            SqlCommand cmd = new SqlCommand(SELECT_BUSCA_NOTIFICACOESID, conn);
            cmd.Parameters.Add(parms[0]);

            return await cmd.ExecuteReaderAsync();
        }
        #endregion

        #region SetInstance
        /// <summary>
        /// Método auxiliar utilizado pelos métodos LoadObject e CompleteObject para percorrer um IDataReader e vincular as colunas com os atributos da classe.
        /// </summary>
        /// <param name="dr">Cursor de leitura do banco de dados.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Jonathan Scrok</remarks>
        private static bool SetInstance(IDataReader dr, Notificacao_P1 objVaga)
        {
            try
            {
                if (dr.Read())
                {
                    objVaga._idNotificacao = Convert.ToInt32(dr["Id_Notificacao"]);
                    objVaga._descricao = Convert.ToString(dr["Descricao"]);
                    objVaga._idUsuarioNotificado = Convert.ToInt32(dr["Id_Usuario_Notificado"]);
                    objVaga._urlNotificacao = Convert.ToString(dr["Url_Notificacao"]);
                    objVaga._notificacaoAtiva = Convert.ToBoolean(dr["NotificacaoAtiva"]);

                    if (dr["Id_Usuario_Notificou"] != DBNull.Value)
                        objVaga._idUsuarioNotificou = Convert.ToInt32(dr["Id_Usuario_Notificou"]);
                    else
                        objVaga._idUsuarioNotificou = null;

                    if (dr["Flg_Visualizado"] != DBNull.Value)
                        objVaga._flg_Visualizado = Convert.ToBoolean(dr["Flg_Visualizado"]);
                    else
                        objVaga._flg_Visualizado = null;

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
