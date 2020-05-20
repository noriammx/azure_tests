using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetCoreWebAPISQLServerAzure.Services;

namespace NetCoreWebAPISQLServerAzure.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AzureController : ControllerBase
    {
        [HttpGet("ConsultarSQLServer")]
        public JsonResult Consultar(int ID)
        {
            var Consulta = new StorageAzure();
            var Lista = Consulta.Consulta(ID);
            return new JsonResult(Lista);

        }

        [HttpGet("AlmacenarSQLServer")]
        public string Almacenar(string Nombre, string Domicilio, string Correo, int edad, double saldo)
        {
            var Almacena = new StorageAzure();
            bool resultado = Almacena.Almacenar(Nombre, Domicilio, Correo, edad, saldo);
            if (resultado == true)
                return "Almacenado en SQL Server sobre Azure desde .Net Core Web API";
            else
            {
                return "No Almacenado";
            }
        }


    }
}
