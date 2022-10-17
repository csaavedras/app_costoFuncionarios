using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp_horasExtras
{
    public partial class Form1 : Form
    {
        private BindingSource bindingSource1 = new BindingSource();
        private SqlDataAdapter dataAdapter = new SqlDataAdapter();
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
            //dataGridView.Rows.Add("18.954.102-3", "Camilo", "100", "$24.000");

         
            String sqlCredencial = "SERVER=LENOVO_S940;DataBase=horas_extras; Integrated Security=SSPI";
            SqlConnection Conexion = new SqlConnection(sqlCredencial);
            Conexion.Open();

            SqlDataReader myReader = null;

            //SQL COMANDS
            SqlCommand myCommandGetRut = new SqlCommand("WITH cte\r\n     AS (\r\nSELECT ROW_NUMBER() OVER(PARTITION BY a.rut\r\n       ORDER BY a.rut) AS fila,\r\n       a.rut, \r\n       a.fecha, \r\n       LEAD(a.fecha) OVER(PARTITION BY a.rut\r\n       ORDER BY a.fecha) AS siguiente\r\n         FROM dbo.registro AS a\r\n)\r\nSELECT c.rut,\r\n       \r\n       CASE\r\n           WHEN c.fecha > c.siguiente\r\n           THEN c.fecha\r\n           ELSE c.siguiente\r\n       END AS [IN],\r\n       CASE\r\n           WHEN c.siguiente IS NULL\r\n           THEN '19000101'\r\n           WHEN c.fecha < c.siguiente\r\n           THEN c.fecha\r\n           ELSE c.siguiente\r\n       END AS [OUT]\r\nFROM cte AS c\r\nWHERE c.fila % 2 != 0;\r\n", Conexion);

            myReader = myCommandGetRut.ExecuteReader();

            //While
            while (myReader.Read())
            {

                string rut = myReader["rut"].ToString();
                //HORA SALIDA
                string hora_salida = myReader["IN"].ToString();
                //HORA ENTRADA
                string hora_entrada = myReader["OUT"].ToString();

                //Obtener hora de SALIDA formateada
                string anho = hora_entrada.Substring(0, 2);
                string mes = hora_entrada.Substring(3, 2);
                string dia = hora_entrada.Substring(6, 4);
                string horaDia = hora_entrada.Substring(11, 5);
                string[] valores = horaDia.Split(':');
                string hora = valores[0];
                string minutos = valores[1];

                //Conversión string a int 
                int hora_num_entrada = Int32.Parse(hora);

                Console.WriteLine(mes);


                //Obtener hora de ENTRADA formateada
                string anho2 = hora_salida.Substring(0, 2);
                string mes2 = hora_salida.Substring(3, 2);
                string dia2 = hora_salida.Substring(6, 4);
                string horaDia2 = hora_salida.Substring(11, 5);
                string[] valores2 = horaDia2.Split(':');
                string hora2 = valores2[0];
                string minutos2 = valores2[1];

                //Conversión string a int 
                int hora_num_salida = Int32.Parse(hora2);

                //Calculo de cantidad de horas trabajadas, y horas extras por item

                int cantidad_h_trabajadas = hora_num_salida - hora_num_entrada;
                int total_h_contrato = 9;
                int total_h_extra = cantidad_h_trabajadas - total_h_contrato;

                //Logia para calcular horas extras: 
                /* if (cantidad_h_trabajadas > total_h_contrato)
                 {   
                     Console.WriteLine("El rut: " + " " + rut + " " + "trabajo: " + "  " + total_h_extra + " " + "horas extras");

                 } else
                 {
                     Console.WriteLine("El rut: " + " " + rut + " " + "No tiene hora extras");
                 } */

                //Console.WriteLine("Cantidad de horas trabajadas por "+ rut  + " " + cantidad_h_trabajadas + " " + "horas" + " " + "en la fecha" + " " + anho + '*' + mes + '*' + dia);


                dataGridView.Rows.Add(rut, hora_entrada, total_h_extra);

            }
            //Conexion.Close();

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

    }
}
