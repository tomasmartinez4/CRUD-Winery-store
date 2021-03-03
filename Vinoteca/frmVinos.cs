using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;


namespace Vinoteca
{
    public partial class frmVinos : Form
    {
        Datos oD = new Datos(@"Data Source=//CONNECTION STRING GOES HERE");
        List<Vino> Vi = new List<Vino>();

        enum Acciones
        {
            nuevo, editado
        }
        Acciones Accion;

        public frmVinos()
        {
            InitializeComponent();
        }

        private void frmVinos_Load(object sender, EventArgs e)
        {
            cargarCombo(cboBodega, "Bodegas");
            cargarLista("Vinos");
            Accion = Acciones.editado;
        }

        private void cargarLista(string nombreTabla)
        {
            Vi.Clear();
            oD.leerTabla(nombreTabla);

            while (oD.pDr.Read())
            {
                Vino v = new Vino();
                if (!oD.pDr.IsDBNull(0))
                {
                    v.pCodigo = oD.pDr.GetInt32(0);
                }
                if (!oD.pDr.IsDBNull(1))
                {
                    v.pNombre = oD.pDr.GetString(1);
                }
                if (!oD.pDr.IsDBNull(2))
                {
                    v.pBodega = oD.pDr.GetInt32(2);
                }
                if (!oD.pDr.IsDBNull(3))
                {
                    v.pPresentacion = oD.pDr.GetInt32(3);
                }
                if (!oD.pDr.IsDBNull(4))
                {
                    v.pPrecio = oD.pDr.GetDouble(4);
                }
                if (!oD.pDr.IsDBNull(5))
                {
                    v.pFecha = oD.pDr.GetDateTime(5);
                }
                Vi.Add(v);
            }
            oD.pDr.Close();
            oD.desconectar();
            lstVinos.Items.Clear();

            for (int i = 0; i < Vi.Count; i++)
            {
                lstVinos.Items.Add(Vi[i].ToString());
            }
            lstVinos.SelectedIndex = 0;
        }

        private void cargarCombo(ComboBox combo, string nombreTabla)
        {
            DataTable dt = new DataTable();
            dt = oD.consultarTabla(nombreTabla);
            combo.DataSource = dt;
            combo.ValueMember = dt.Columns[0].ColumnName;
            combo.DisplayMember = dt.Columns[1].ColumnName;


            combo.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            string consultaSQL = "";
            if (validarDatos())
            {
                Vino v = new Vino();
                v.pNombre = txtNombre.Text;
                v.pBodega = (int)cboBodega.SelectedValue;

                if (cboBodega.SelectedIndex == 2)
                {
                    rbt375.Enabled = false;
                }

                if (rbt375.Checked)
                {
                    v.pPresentacion = 1;
                }
                else
                {
                    v.pPresentacion = 2;
                }
                v.pFecha = dtpFecha.Value;
                v.pPrecio = double.Parse(txtPrecio.Text);
                if (Accion == Acciones.nuevo)
                {
                    //insert
                    consultaSQL = "INSERT INTO Vinos(nombre, bodega, presentacion, precio, fecha)" +
                        "VALUES (@nombre,@bodega,@presentacion,@precio,@fecha)";

                    oD.actualizarParametros(consultaSQL, v);

                    cargarLista("vino");

                }
                else
                {
                    //update
                    v.pCodigo = int.Parse(txtCodigo.Text);

                    consultaSQL = "UPDATE Vinos SET nombre=@nombre WHERE codigo=@codigo";

                    oD.actualizarParametros(consultaSQL, v);

                    cargarLista("vino");
                }

             MessageBox.Show("Éxito!");
            }
        }

        private bool validarDatos()
        {
            
            if (txtNombre.Text == string.Empty)
            {
                MessageBox.Show("Debe ingresar nombre");
                txtNombre.Focus();
                return false;
            }
            if (txtCodigo.Text == string.Empty)
            {
                MessageBox.Show("Debe ingresar codigo");
                txtCodigo.Focus();
                return false;
            }

            if (txtPrecio.Text == string.Empty)
            {
                MessageBox.Show("Debe ingresar codigo");
                txtPrecio.Focus();
                return false;
            }

            if (cboBodega.SelectedIndex == -1)
            {
                MessageBox.Show("Debe ingresar bodega");
                cboBodega.Focus();
                return false;
            }
            return true;
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmVinos_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Está seguro de abandonar la aplicacion?",
                "SALIENDO",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {
                e.Cancel = false;
            }
            else
            {
                e.Cancel = true;
            }

        }
    }
}
