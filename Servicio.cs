using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace ServicioRespladoBDBorboletas
{
    public partial class Servicio : ServiceBase
    {
        private Timer timer;

        public Servicio()
        {
            InitializeComponent();
        }

        

        protected override void OnStop()
        {
            timer1.Stop();
        }

        private void timer1_Elapsed(object sender, ElapsedEventArgs e)
        {
            //Recordar que para pruebas configurar el timer en 120000 milisegundos (2 minutos) para producción serían 86400000 Milisegundos (1 día)
            RealizarRespaldo();
        }

        protected override void OnStart(string[] args)
        {
            timer1.Start();
        }

        private void RealizarRespaldo()
        {
            string connectionString = "Server=192.168.0.24,1433\\SQLEXPRESS;Database=BORBOLETAS;User Id=Admin_Sistema;Password=Admin1234;";
            string databaseName = "BORBOLETAS";
            string backupPath = @"C:\RespaldoBDBorboletas\";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("EXEC PA_Respaldo_BD @BDBorboletas, @RutaRespaldo", connection);
                command.Parameters.AddWithValue("@BDBorboletas", databaseName);
                command.Parameters.AddWithValue("@RutaRespaldo", backupPath);
                command.ExecuteNonQuery();
            }
        }

        
    }
}
