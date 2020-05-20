using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using Java.Security;
using System.Threading.Tasks;
using System.Net;
using Android.Net;
using System;
using System.Json;
using System.IO;

namespace EbibitApp
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        EditText txtNombre, txtDomicilio, txtCorreo, txtEdad, txtSaldo, txtID;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);
            txtID = FindViewById<EditText>(Resource.Id.txtID);
            txtNombre = FindViewById<EditText>(Resource.Id.txtNombre);
            txtDomicilio = FindViewById<EditText>(Resource.Id.txtDomicilio);
            txtCorreo = FindViewById<EditText>(Resource.Id.txtCorreo);
            txtEdad = FindViewById<EditText>(Resource.Id.txtEdad);
            txtSaldo = FindViewById<EditText>(Resource.Id.txtSaldo);
            var btnGuardar = FindViewById<Button>(Resource.Id.btnAlmacenar);
            var btnConsultar = FindViewById<Button>(Resource.Id.btnBuscarRegistro);

            btnConsultar.Click += async delegate
            {
                try
                {
                    int ID = int.Parse(txtID.Text);
                    var API = "https://ebibitservices.azurewebsites.net/Azure/ConsultarSQLServer?ID=" + ID;
                    JsonValue json = await Datos(API);
                    Transform(json);

                }
                catch (Exception ex)
                {
                    Toast.MakeText(this, ex.Message, ToastLength.Long).Show();
                }


            };

            btnGuardar.Click += delegate
            {
                try
                {
                    var Nombre = txtNombre.Text;
                    var Domicilio = txtDomicilio.Text;
                    var Correo = txtCorreo.Text;
                    var Edad = int.Parse(txtEdad.Text);
                    var saldo = double.Parse(txtSaldo.Text);
                    var API = "https://ebibitservices.azurewebsites.net/Azure/AlmacenarSQLServer?Nombre="+Nombre+"&Domicilio="+Domicilio+"&Correo="+Correo+"&edad="+Edad+"&saldo="+saldo;

                    var request = (HttpWebRequest)WebRequest.Create(API);
                    WebResponse response = request.GetResponse();
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    string responseText = reader.ReadToEnd();
                    Toast.MakeText(this, responseText.ToString(), ToastLength.Long).Show();


                }
                catch (Exception)
                {

                    throw;
                }

            };


        }


        public void Transform(JsonValue json)
        {
            try
            {
                var Resultados = json[0];
                txtNombre.Text = Resultados["nombre"];
                txtCorreo.Text = Resultados["correo"];
                txtDomicilio.Text = Resultados["domicilio"];
                txtEdad.Text = Resultados["edad"].ToString();
                txtSaldo.Text = Resultados["saldo"].ToString();
            }
            catch (Exception ex)
            {

                Toast.MakeText(this, ex.Message, ToastLength.Long).Show();
            }
        }

        public async Task<JsonValue> Datos(string API)
        {
            var request = (HttpWebRequest)WebRequest.Create(new System.Uri(API));
            request.ContentType = "application/json";
            request.Method = "GET";
            using(WebResponse response = await request.GetResponseAsync())
            {
                using(System.IO.Stream stream = response.GetResponseStream())
                {
                    var jsondoc = await Task.Run(()=> JsonValue.Load(stream));
                    return jsondoc;
                }
            }


        }


        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}