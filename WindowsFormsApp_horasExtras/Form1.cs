using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp_horasExtras
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            
            string sqlCredencial = "SERVER=LENOVO_S940;DataBase=horas_extras; Integrated Security=SSPI";
            SqlConnection Conexion = new SqlConnection(sqlCredencial);
            Conexion.Open();

            SqlDataReader myReader = null;
            SqlCommand myCommand = new SqlCommand("Select nombre_mes from dias order by mes", Conexion);
            myReader = myCommand.ExecuteReader();


            while (myReader.Read())
            {
                comboBox1.Items.Add(myReader["nombre_mes"].ToString());
            }
            Conexion.Close();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox1.Text = "Selecciona el mes";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //FORMA DE AGREGAR COLUMNAS AL DATA GRID
            dataGridView.Columns.Add("rut", "Rut");
            dataGridView.Columns.Add("nombre", "NOMBRE");
            dataGridView.Columns.Add("horas_extras", "HORAS EXTRAS");
            dataGridView.Columns.Add("costo", "COSTO");

            //FORMA DE AGREGAR DATOS A LAS FILAS DEL DATA GRID
            dataGridView.Rows.Add("18.954.102-3", "Camilo", "100", "$24.000");



            string sqlCredencial = "SERVER=LENOVO_S940;DataBase=horas_extras; Integrated Security=SSPI";
            SqlConnection Conexion = new SqlConnection(sqlCredencial);
            Conexion.Open();

            SqlDataReader myReader = null;
            //SQL COMANDS
            SqlCommand myCommand = new SqlCommand("Select fecha from registro", Conexion);

            SqlCommand myCommandGetRut = new SqlCommand("select rut, nombre from funcionarios", Conexion);

            myReader = myCommandGetRut.ExecuteReader();
            //myReader = myCommand.ExecuteReader();


            while (myReader.Read())
            {

                string rut = myReader["rut"].ToString();
                string nombre = myReader["nombre"].ToString();
                dataGridView.Rows.Add(rut, nombre);



            }
            Conexion.Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
