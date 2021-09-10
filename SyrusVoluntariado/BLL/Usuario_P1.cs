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
    public partial class Usuario_P1
    {
        #region StringConnection
        private const string stringConnection = "Data Source=mssql-49550-0.cloudclusters.net,11255;Initial Catalog=be_helper;Integrated Security=False;User Id=AdminBeaHelper;Password=B3ah3lper#2021;MultipleActiveResultSets=True";
        #endregion

        #region Atributos

        private int _idUsuario;
        private string _nome;
        private int? _sexo;
        private string _email;
        private DateTime _dataCadastro;

        private bool _persisted;
        private bool _modified;

        #endregion

        #region Construtores
        public Usuario_P1()
        {
            this._persisted = false;
        }
        public Usuario_P1(int idUsuario)
        {
            this._idUsuario = idUsuario;
            this._persisted = true;
        }
        #endregion

        #region Propriedades

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

        #region Nome
        public string Nome
        {
            get
            {
                return this._nome;
            }
            set
            {
                this._nome = value;
                this._modified = true;
            }
        }
        #endregion

        #region Sexo
        public int? Sexo
        {
            get
            {
                return this._sexo;
            }
            set
            {
                this._sexo = value;
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

        #region Consultas
        private const string SELECT_TODOSUSUARIOS = @"select * from helper.Usuarios";
        private const string SELECT_BUSCAUSUARIOID = @"select * from helper.Usuarios WITH(NOLOCK) where Id_Usuario = @Id_Usuario";
        private const string SELECT_BUSCAUSUARIOEMAIL = @"select * from helper.Usuarios WITH(NOLOCK) where Email = @Email";

        private const string UPDATE_USUARIO = @"UPDATE helper.Usuarios SET Nome = @Nome, Sexo = @Sexo, Email = @Email, DataCadastro = @DataCadastro WHERE Id_Usuario = @Id_Usuario";
        private const string INSERT_USUARIO = @"INSERT INTO helper.Usuarios(Nome, Sexo ,Email, DataCadastro) VALUES (@Nome, @Sexo, @Email, @DataCadastro);";
        #endregion

        #region Metodos

        #region GetParameters

        private static List<SqlParameter> GetParameters()
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Id_Usuario", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Nome", SqlDbType.VarChar, 100));
            parms.Add(new SqlParameter("@Sexo", SqlDbType.Int, 1));
            parms.Add(new SqlParameter("@Email", SqlDbType.VarChar, 100));
            parms.Add(new SqlParameter("@DataCadastro", SqlDbType.DateTime, 8));

            return (parms);
        }
        #endregion

        #region SetParameters

        private void SetParameters(List<SqlParameter> parms)
        {
            parms[0].Value = this._idUsuario;
            parms[1].Value = this._nome;
            parms[2].Value = this._sexo;
            parms[3].Value = this._email;
            parms[4].Value = this._dataCadastro;
        }
        #endregion

        #region Busca todos os Usuários do Banco
        public static List<Usuario> TodosUsuarios()
        {
            SqlConnection conn = null;
            SqlDataReader reader = null;
            List<Usuario> usuarios = new List<Usuario>();

            try
            {
                conn = new SqlConnection(stringConnection);
                conn.Open();

                SqlCommand cmd = new SqlCommand(SELECT_TODOSUSUARIOS, conn);

                Mapper.CreateMap<IDataRecord, Usuario>();

                using (reader = cmd.ExecuteReader())
                {
                    usuarios = Mapper.Map<List<Usuario>>(reader);
                    return usuarios;
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
                        SqlCommand cmd = new SqlCommand(INSERT_USUARIO, conn, trans);

                        for (int i = 0; i < parms.Count; i++)
                        {
                            cmd.Parameters.Add(parms[i]);
                        }

                        cmd.ExecuteNonQuery();
                        this._idUsuario = Convert.ToInt32(cmd.Parameters["@Id_Usuario"].Value);
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


            SqlCommand cmd = new SqlCommand(INSERT_USUARIO, conn, trans);

            for (int i = 0; i < parms.Count; i++)
            {
                cmd.Parameters.Add(parms[i]);
            }

            cmd.ExecuteNonQuery();
            this._idUsuario = Convert.ToInt32(cmd.Parameters["@Id_Usuario"].Value);
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
                SqlCommand cmd = new SqlCommand(UPDATE_USUARIO, conn);

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
                SqlCommand cmd = new SqlCommand(UPDATE_USUARIO, conn);

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

        public bool CompleteObject()
        {
            using (IDataReader dr = LoadDataReader(this._idUsuario))
            {
                return SetInstance(dr, this);
            }
        }

        public bool CompleteObject(string email)
        {
            using (IDataReader dr = LoadDataReader(email))
            {
                return SetInstance(dr, this);
            }
        }
        public bool CompleteObject(SqlTransaction trans)
        {
            using (IDataReader dr = LoadDataReader(this._idUsuario, trans))
            {
                return SetInstance(dr, this);
            }
        }
        public async Task<bool> CompleteObjectAsync(SqlTransaction trans)
        {
            using (IDataReader dr = await LoadDataReaderAsync(this._idUsuario, trans))
            {
                return SetInstance(dr, this);
            }
        }
        #endregion

        #region LoadDataReader
        private static IDataReader LoadDataReader(int IdUsuario)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Id_Usuario", SqlDbType.Int, 4));

            parms[0].Value = IdUsuario;

            SqlConnection conn = null;
            conn = new SqlConnection(stringConnection);
            conn.Open();

            SqlCommand cmd = new SqlCommand(SELECT_BUSCAUSUARIOID, conn);
            cmd.Parameters.Add(parms[0]);

            return cmd.ExecuteReader();
        }

        private static IDataReader LoadDataReader(string email)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Email", SqlDbType.VarChar, 100));

            parms[0].Value = email;

            SqlConnection conn = null;
            conn = new SqlConnection(stringConnection);
            conn.Open();

            SqlCommand cmd = new SqlCommand(SELECT_BUSCAUSUARIOEMAIL, conn);
            cmd.Parameters.Add(parms[0]);

            return cmd.ExecuteReader();
        }

        private static IDataReader LoadDataReader(int IdUsuario, SqlTransaction trans)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Id_Usuario", SqlDbType.Int, 4));

            parms[0].Value = IdUsuario;

            SqlConnection conn = null;
            conn = new SqlConnection(stringConnection);
            conn.Open();

            SqlCommand cmd = new SqlCommand(SELECT_BUSCAUSUARIOID, conn);
            cmd.Parameters.Add(parms[0]);

            return cmd.ExecuteReader();
        }
        private static async Task<IDataReader> LoadDataReaderAsync(int IdUsuario, SqlTransaction trans)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Id_Usuario", SqlDbType.Int, 4));

            parms[0].Value = IdUsuario;
            SqlConnection conn = null;
            conn = new SqlConnection(stringConnection);
            conn.Open();

            SqlCommand cmd = new SqlCommand(SELECT_BUSCAUSUARIOID, conn);
            cmd.Parameters.Add(parms[0]);

            return await cmd.ExecuteReaderAsync();
        }
        private static async Task<IDataReader> LoadDataReaderAsync(int Id_Usuario)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Id_Usuario", SqlDbType.Int, 4));

            SqlConnection conn = null;
            conn = new SqlConnection(stringConnection);
            conn.Open();

            SqlCommand cmd = new SqlCommand(SELECT_BUSCAUSUARIOID, conn);
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
        private static bool SetInstance(IDataReader dr, Usuario_P1 objVaga)
        {
            try
            {
                if (dr.Read())
                {
                    objVaga._idUsuario = Convert.ToInt32(dr["Id_Usuario"]);
                    objVaga._nome = Convert.ToString(dr["Nome"]);
                    objVaga._sexo = Convert.ToInt32(dr["Sexo"]);
                    objVaga._email = Convert.ToString(dr["Email"]);
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
