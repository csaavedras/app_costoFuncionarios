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
using System.Xml;

namespace WindowsFormsApp_horasExtras
{
    public partial class Form1 : Form
    {
        public DataTable Dt;
        public SqlDataReader Dr;
        public SqlCommand SqlC;

        //Credenciales para conectar a nuestra base de datos local 
        string sqlCredencial = "SERVER=LENOVO_S940;DataBase=horas_extras; Integrated Security=SSPI";


        private BindingSource bindingSource1 = new BindingSource();
        private SqlDataAdapter dataAdapter = new SqlDataAdapter();
        public Form1()
        {

            InitializeComponent();
            
        }

        public DataTable CargarDatos(string Tabla)
        {
            SqlConnection Conexion = new SqlConnection(sqlCredencial);
            Dt = new DataTable();
            SqlC = new SqlCommand("Select * from " + Tabla, Conexion);
            Dr = SqlC.ExecuteReader();
            Dt.Load(Dr);

            Conexion.Close();

            return Dt;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            SqlConnection Conexion = new SqlConnection(sqlCredencial);
            Conexion.Open();

            SqlCommand myCommand = new SqlCommand("Select nombre_mes from dias", Conexion);

            SqlDataAdapter sdr = new SqlDataAdapter(myCommand);
            DataTable dt = new DataTable();
            sdr.Fill(dt);
            comboBox1.DisplayMember = "nombre_mes";
            comboBox1.DataSource = dt;

            Conexion.Close();

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

        
            SqlConnection Conexion = new SqlConnection(sqlCredencial);
            Conexion.Open();

            SqlDataReader myReader = null;

            //SQL COMANDS
            SqlCommand myCommandGetRut = new SqlCommand("leer", Conexion);

            myReader = myCommandGetRut.ExecuteReader();

            //While
            while (myReader.Read())
            {
                string rut = myReader[0].ToString();
                //HORA SALIDA
                string hora_salida = myReader["OUT"].ToString();
                //HORA ENTRADA
                string hora_entrada = myReader["IN"].ToString();
                //Nombre
                string nombre = myReader["nombre"].ToString();

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

                if (cantidad_h_trabajadas > total_h_contrato)
                {
                    dataGridView.Rows.Add(rut, nombre, total_h_extra);
                } 


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
