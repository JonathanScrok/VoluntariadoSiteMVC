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
        private const string stringConnection = "Data Source=DESKTOP-P97AO4H;Initial Catalog=be_helper;Integrated Security=False;User Id=sa;Password=b3ah3lper#2021;MultipleActiveResultSets=True";
        #endregion

        #region Atributos

        private int _idUsuario;
        private string _nome;
        private int _sexo;
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
        public int Sexo
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

        #region Busca Usuario por Id
        public static List<Usuario> UsuarioPorId(int idUsuario)
        {
            SqlConnection conn = null;
            SqlDataReader reader = null;
            List<Usuario> usuarios = new List<Usuario>();

            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Id_Usuario", SqlDbType.Int, 4));
            parms[0].Value = idUsuario;

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

            //return cmd.ExecuteReader(SELECT_BUSCAUSUARIOID, parms);
        }

        #endregion

        #endregion
    }
}
