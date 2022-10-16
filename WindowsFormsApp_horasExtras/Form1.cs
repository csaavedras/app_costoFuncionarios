using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
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
            dataGridView.Columns.Add("rut", "RUT");
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
            //SqlCommand myCommandGetRut = new SqlCommand("select funcionarios.nombre,funcionarios.rut,registro.fecha from funcionarios\r\ninner join registro on funcionarios.rut=registro.rut\r\norder by nombre", Conexion);

            SqlCommand myCommandGetRut = new SqlCommand("WITH cte\r\n     AS (\r\nSELECT ROW_NUMBER() OVER(PARTITION BY a.rut\r\n       ORDER BY a.rut) AS fila,\r\n       a.rut, \r\n       a.fecha, \r\n       LEAD(a.fecha) OVER(PARTITION BY a.rut\r\n       ORDER BY a.fecha) AS siguiente\r\n         FROM dbo.registro AS a\r\n)\r\nSELECT c.rut,\r\n       \r\n       CASE\r\n           WHEN c.fecha > c.siguiente\r\n           THEN c.fecha\r\n           ELSE c.siguiente\r\n       END AS [IN],\r\n       CASE\r\n           WHEN c.siguiente IS NULL\r\n           THEN '19000101'\r\n           WHEN c.fecha < c.siguiente\r\n           THEN c.fecha\r\n           ELSE c.siguiente\r\n       END AS [OUT]\r\nFROM cte AS c\r\nWHERE c.fila % 2 != 0;\r\n", Conexion);

            myReader = myCommandGetRut.ExecuteReader();

            while (myReader.Read())
            {

                //string rut = myReader["rut"].ToString();
                //string nombre = myReader["nombre"].ToString();
                //string fecha = myReader["fecha"].ToString();
                string rut = myReader["rut"].ToString();
                string hora_entrada = myReader["IN"].ToString();
                string hora_salida = myReader["OUT"].ToString();

                

                dataGridView.Rows.Add(rut, hora_entrada, hora_salida);



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
