using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace Vinoteca
{
    class Datos
    {
        private SqlConnection cx;
        private SqlCommand cmd;
        private SqlDataReader dr;
        private string cadenaConexion;
        public SqlDataReader pDr { get => dr; set => dr = value; }
        public string pCadenaConexion { get => cadenaConexion; set => cadenaConexion = value; }

        public Datos()
        {
            this.cx = new SqlConnection();
            this.cmd = new SqlCommand();
            this.dr = null;
            this.cadenaConexion = null;
        }

        public Datos(string cadenaConexion)
        {
            this.cx = new SqlConnection();
            this.cmd = new SqlCommand();
            this.dr = null;
            this.cadenaConexion = cadenaConexion;
        }

        public void conectar()
        {
            cx.ConnectionString = cadenaConexion;
            cx.Open();
            cmd.Connection = cx;
            cmd.CommandType = CommandType.Text;
        }

        public void desconectar()
        {
            cx.Close();
            cx.Dispose();
        }
        public DataTable consultarTabla(string nombreTabla)
        {
            DataTable tabla = new DataTable();
            this.conectar();
            this.cmd.CommandText = "SELECT * FROM " + nombreTabla;
            tabla.Load(cmd.ExecuteReader());
            this.desconectar();
            return tabla;
        }

        public void leerTabla(string nombreTabla)
        {
            this.conectar();
            this.cmd.CommandText = "SELECT * FROM " + nombreTabla;
            this.dr = cmd.ExecuteReader();
        }

        public void actualizarParametros(string consultaSQL, Vino v)
        {
            this.conectar();
            this.cmd.CommandText = consultaSQL;
            cmd.Parameters.Clear();

            //agrego parametros al comando
            cmd.Parameters.AddWithValue("@codigo", v.pCodigo);
            cmd.Parameters.AddWithValue("@nombre", v.pNombre);
            cmd.Parameters.AddWithValue("@bodega", v.pBodega);
            cmd.Parameters.AddWithValue("@presentacion", v.pPresentacion);
            cmd.Parameters.AddWithValue("@precio", v.pPrecio);
            cmd.Parameters.AddWithValue("@fecha", v.pFecha);

            //ejecutar parametros
            this.cmd.ExecuteNonQuery();
            this.desconectar();

        }
    }
}
