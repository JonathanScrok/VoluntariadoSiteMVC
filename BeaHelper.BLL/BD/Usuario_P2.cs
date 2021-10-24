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
    public partial class Usuario_P2
    {
        #region StringConnection
        //private const string stringConnection = "Data Source=mssql-49550-0.cloudclusters.net,11255;Initial Catalog=be_helper;Integrated Security=False;User Id=AdminBeaHelper;Password=B3ah3lper#2021;MultipleActiveResultSets=True";
        private static string stringConnection = DbAcess.GetConnection();
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
        private const string SELECT_BUSCAEMAILUSUARIO = @"select * from helper.Usuarios WITH(NOLOCK) where Email = @Email";
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

        #region Busca Usuario Por ID
        public static List<Usuario> BusaUsuario_PorID(int id)
        {
            SqlConnection conn = null;
            SqlDataReader reader = null;
            List<Usuario> usuarios = new List<Usuario>();

            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Id_Usuario", SqlDbType.BigInt, 4));
            parms[0].Value = id;

            conn = new SqlConnection(stringConnection);
            conn.Open();

            SqlCommand cmd = new SqlCommand(SELECT_BUSCAUSUARIOID, conn);
            cmd.Parameters.Add(parms[0]);

            Mapper.CreateMap<IDataRecord, Usuario>();

            using (reader = cmd.ExecuteReader())
            {
                usuarios = Mapper.Map<List<Usuario>>(reader);
                return usuarios;
            }
        }
        #endregion

        #region Busca Usuario por Email
        public static List<Usuario> BuscaUsuario_Email(string Email)
        {
            SqlConnection conn = null;
            SqlDataReader reader = null;
            List<Usuario> emails = new List<Usuario>();

            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Email", SqlDbType.VarChar, 100));
            parms[0].Value = Email;

            conn = new SqlConnection(stringConnection);
            conn.Open();

            SqlCommand cmd = new SqlCommand(SELECT_BUSCAEMAILUSUARIO, conn);
            cmd.Parameters.Add(parms[0]);

            Mapper.CreateMap<IDataRecord, Usuario>();

            using (reader = cmd.ExecuteReader())
            {
                emails = Mapper.Map<List<Usuario>>(reader);
                return emails;
            }
        }
        #endregion

        #endregion

    }
}
