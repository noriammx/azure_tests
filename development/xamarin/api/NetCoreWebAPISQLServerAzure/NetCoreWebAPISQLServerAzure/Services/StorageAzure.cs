using NetCoreWebAPISQLServerAzure.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreWebAPISQLServerAzure.Services
{
    public class StorageAzure
    {
        public List<Clientes> ListadoClientes;
        public bool Almacenar(string Nombre, string Domicilio, string Correo, int edad, double saldo)
        {
            var connect = new SqlConnection("Server=tcp:marinoazure.database.windows.net,1433;Initial Catalog=informacion;Persist Security Info=False;User ID=admincourse;Password=p@ssw0rd12345;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            var query = new SqlCommand("INSERT INTO Datos (Nombre, Domicilio, Correo, Edad, Saldo) VALUES " +
                                        "('" + Nombre + "','" + Domicilio + "','" + Correo + "','" + edad + "','" + saldo + "')", connect);
            try
            {
                connect.Open();
                query.ExecuteNonQuery();
                connect.Close();
                return true;

            }
            catch (SqlException ex)
            {
                connect.Close();
                return false;
            }

        }

        public List<Clientes> Consulta(int ID)
        {
            var dt = new DataTable();
            var connect = new SqlConnection("Server=tcp:marinoazure.database.windows.net,1433;Initial Catalog=informacion;Persist Security Info=False;User ID=admincourse;Password=p@ssw0rd12345;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            var cmd = new SqlCommand("SELECT * FROM Datos WHERE ID='" + ID + "'", connect);
            connect.Open();
            var da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            ListadoClientes = new List<Clientes>();
            Clientes cliente = new Clientes();
            cliente.ID = int.Parse((dt.Rows[0]["ID"]).ToString());
            cliente.Nombre = (dt.Rows[0]["NOMBRE"]).ToString();
            cliente.Domicilio = (dt.Rows[0]["DOMICILIO"]).ToString();
            cliente.Correo = (dt.Rows[0]["CORREO"]).ToString();
            cliente.Edad = int.Parse((dt.Rows[0]["EDAD"]).ToString());
            cliente.Saldo = double.Parse((dt.Rows[0]["SALDO"]).ToString());
            ListadoClientes.Add(cliente);
            return ListadoClientes;
        }
    }
}
