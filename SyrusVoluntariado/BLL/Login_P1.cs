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
    public partial class Login_P1
    {
        #region StringConnection
        private const string stringConnection = "Data Source=mssql-49550-0.cloudclusters.net,11255;Initial Catalog=be_helper;Integrated Security=False;User Id=AdminBeaHelper;Password=B3ah3lper#2021;MultipleActiveResultSets=True";
        #endregion

        #region Atributos

        private int _idLogin;
        private int _idUsuario;
        private string _email;
        private string _senha;
        private DateTime _dataCadastro;

        private bool _persisted;
        private bool _modified;

        #endregion

        #region Propriedades

        #region IdLogin
        public int IdLogin
        {
            get
            {
                return this._idLogin;
            }
            set
            {
                this._idLogin = value;
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

        #region Email
        public string Email
        {
            get
            {
                return this._email;
            }
            set
            {
                this._email = value;
                this._modified = true;
            }
        }
        #endregion

        #region Senha

        public string Senha
        {
            get
            {
                return this._senha;
            }
            set
            {
                this._senha = value;
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
        public Login_P1()
        {
            this._persisted = false;
        }
        public Login_P1(int IdLogin)
        {
            this._idLogin = IdLogin;
            this._persisted = true;
        }
        #endregion

        #region Consultas
        private const string SELECT_TODOSLOGINS = @"select * from helper.Logins";
        private const string SELECT_BUSCALOGINID = @"select * from helper.Logins where Id_Login = @Id_Login";
        private const string SELECT_BUSCALOGIN_IDUSUARIO = @"select * from helper.Logins where Id_Usuario = @Id_Usuario";
        private const string SELECT_BUSCALOGIN_EMAIL = @"select Email from helper.Logins where Email = @Email";
        private const string SELECT_BUSCALOGIN_EMAILSENHA = @"select * from helper.Logins where Email = @Email AND Senha = @Senha";

        private const string UPDATE_LOGIN = @"UPDATE helper.Logins SET Id_Login = @Id_Login,Id_Usuario = @Id_Usuario, Email = @Email, Senha = @Senha, DataCadastro = @DataCadastro WHERE Id_Login = @Id_Login";
        private const string INSERT_LOGIN = @"INSERT INTO helper.Logins(Id_Usuario, Email, Senha , DataCadastro) VALUES (@Id_Usuario, @Email, @Senha, @DataCadastro)";
        #endregion

        #region Metodos

        #region GetParameters

        private static List<SqlParameter> GetParameters()
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Id_Login", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Id_Usuario", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Email", SqlDbType.VarChar, 100));
            parms.Add(new SqlParameter("@Senha", SqlDbType.VarChar, 50));
            parms.Add(new SqlParameter("@DataCadastro", SqlDbType.DateTime, 8));

            return (parms);
        }
        #endregion

        #region SetParameters

        private void SetParameters(List<SqlParameter> parms)
        {
            parms[0].Value = this._idLogin;
            parms[1].Value = this._idUsuario;
            parms[2].Value = this._email;
            parms[3].Value = this._senha;
            parms[4].Value = this._dataCadastro;
        }
        #endregion

        #region Busca todas os Logins do Banco
        public static List<Login> TodosLogins()
        {
            SqlConnection conn = null;
            SqlDataReader reader = null;
            List<Login> Logins = new List<Login>();

            try
            {
                conn = new SqlConnection(stringConnection);
                conn.Open();

                SqlCommand cmd = new SqlCommand(SELECT_TODOSLOGINS, conn);

                Mapper.CreateMap<IDataRecord, Login>();

                using (reader = cmd.ExecuteReader())
                {
                    Logins = Mapper.Map<List<Login>>(reader);
                    return Logins;
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

        #region Busca Login por Email
        public static List<Usuario> BuscaLogin_Email(string Email)
        {
            SqlConnection conn = null;
            SqlDataReader reader = null;
            List<Usuario> emails = new List<Usuario>();

            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Email", SqlDbType.VarChar, 100));
            parms[0].Value = Email;

            conn = new SqlConnection(stringConnection);
            conn.Open();

            SqlCommand cmd = new SqlCommand(SELECT_BUSCALOGIN_EMAIL, conn);
            cmd.Parameters.Add(parms[0]);

            Mapper.CreateMap<IDataRecord, Usuario>();

            using (reader = cmd.ExecuteReader())
            {
                emails = Mapper.Map<List<Usuario>>(reader);
                return emails;
            }
        }
        #endregion

        #region Busca Login por EMAIL e SENHA
        public static List<Login> BuscaLogin_EmailSenha(string Email, string Senha)
        {
            SqlConnection conn = null;
            SqlDataReader reader = null;
            List<Login> login = new List<Login>();

            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Email", SqlDbType.VarChar, 100));
            parms.Add(new SqlParameter("@Senha", SqlDbType.VarChar, 100));
            parms[0].Value = Email;
            parms[1].Value = Senha;

            conn = new SqlConnection(stringConnection);
            conn.Open();

            SqlCommand cmd = new SqlCommand(SELECT_BUSCALOGIN_EMAILSENHA, conn);
            cmd.Parameters.Add(parms[0]);
            cmd.Parameters.Add(parms[1]);

            Mapper.CreateMap<IDataRecord, Login>();

            using (reader = cmd.ExecuteReader())
            {
                login = Mapper.Map<List<Login>>(reader);
                return login;
            }
        }
        #endregion

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
                        SqlCommand cmd = new SqlCommand(INSERT_LOGIN, conn, trans);

                        for (int i = 0; i < parms.Count; i++)
                        {
                            cmd.Parameters.Add(parms[i]);
                        }

                        cmd.ExecuteNonQuery();
                        this._idLogin = Convert.ToInt32(cmd.Parameters["@Id_Login"].Value);
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


            SqlCommand cmd = new SqlCommand(INSERT_LOGIN, conn, trans);

            for (int i = 0; i < parms.Count; i++)
            {
                cmd.Parameters.Add(parms[i]);
            }

            cmd.ExecuteNonQuery();
            this._idLogin = Convert.ToInt32(cmd.Parameters["@Id_Login"].Value);
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
                SqlCommand cmd = new SqlCommand(UPDATE_LOGIN, conn);

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
                SqlCommand cmd = new SqlCommand(UPDATE_LOGIN, conn);

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

        #region CompleteObject
        /// <summary>
        /// Método utilizado para completar uma instância de Usuario a partir do banco de dados.
        /// </summary>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Jonathan Scrok</remarks>
        public bool CompleteObject()
        {
            using (IDataReader dr = LoadDataReader(this._idLogin))
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
            using (IDataReader dr = LoadDataReader(this._idLogin, trans))
            {
                return SetInstance(dr, this);
            }
        }
        public async Task<bool> CompleteObjectAsync(SqlTransaction trans)
        {
            using (IDataReader dr = await LoadDataReaderAsync(this._idLogin, trans))
            {
                return SetInstance(dr, this);
            }
        }
        #endregion

        #region LoadDataReader
        private static IDataReader LoadDataReader(int IdLogin)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Id_Login", SqlDbType.Int, 4));

            parms[0].Value = IdLogin;

            SqlConnection conn = null;
            conn = new SqlConnection(stringConnection);
            conn.Open();

            SqlCommand cmd = new SqlCommand(SELECT_BUSCALOGINID, conn);
            cmd.Parameters.Add(parms[0]);

            return cmd.ExecuteReader();
        }
        private static IDataReader LoadDataReader(int IdLogin, SqlTransaction trans)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Id_Login", SqlDbType.Int, 4));

            parms[0].Value = IdLogin;

            SqlConnection conn = null;
            conn = new SqlConnection(stringConnection);
            conn.Open();

            SqlCommand cmd = new SqlCommand(SELECT_BUSCALOGINID, conn);
            cmd.Parameters.Add(parms[0]);

            return cmd.ExecuteReader();
        }
        private static async Task<IDataReader> LoadDataReaderAsync(int IdLogin, SqlTransaction trans)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Id_Login", SqlDbType.Int, 4));

            parms[0].Value = IdLogin;
            SqlConnection conn = null;
            conn = new SqlConnection(stringConnection);
            conn.Open();

            SqlCommand cmd = new SqlCommand(SELECT_BUSCALOGINID, conn);
            cmd.Parameters.Add(parms[0]);

            return await cmd.ExecuteReaderAsync();
        }
        private static async Task<IDataReader> LoadDataReaderAsync(int Id_Login)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Id_Login", SqlDbType.Int, 4));

            SqlConnection conn = null;
            conn = new SqlConnection(stringConnection);
            conn.Open();

            SqlCommand cmd = new SqlCommand(SELECT_BUSCALOGINID, conn);
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
        private static bool SetInstance(IDataReader dr, Login_P1 objVaga)
        {
            try
            {
                if (dr.Read())
                {
                    objVaga._idLogin = Convert.ToInt32(dr["Id_Login"]);
                    objVaga._idUsuario = Convert.ToInt32(dr["Id_Usuario"]);
                    objVaga._email = Convert.ToString(dr["Email"]);
                    objVaga._senha = Convert.ToString(dr["Senha"]);
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
    }
}
