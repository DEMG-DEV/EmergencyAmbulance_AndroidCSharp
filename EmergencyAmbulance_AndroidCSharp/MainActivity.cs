using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using MySql.Data.MySqlClient;
using System.Data;

namespace EmergencyAmbulance_AndroidCSharp
{
    [Activity(Label = "Reporte - EmergencyAmbulance", Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        string s1, s2, s3;
        string connsqlstring = "Server={0};Port=3306;database=emergency;User Id={1};Password={2};charset=utf8";
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            connsqlstring = string.Format(connsqlstring, Intent.Extras.GetString("servidor"), Intent.Extras.GetString("user"), Intent.Extras.GetString("pass"));
            // Get our button from the layout resource,
            // and attach an event to it
            Spinner spinner1 = FindViewById<Spinner>(Resource.Id.spinner1);
            Spinner spinner2 = FindViewById<Spinner>(Resource.Id.spinner2);
            Spinner spinner3 = FindViewById<Spinner>(Resource.Id.spinner3);

            //Llenado Spinner Ambulancias
            spinner1.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinner1_ItemSelected);
            var adapter1 = ArrayAdapter.CreateFromResource(
                    this, Resource.Array.ambulance_array, Android.Resource.Layout.SimpleSpinnerItem);

            adapter1.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spinner1.Adapter = adapter1;

            //Llenado Spinner Sexo
            spinner2.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinner2_ItemSelected);
            var adapter2 = ArrayAdapter.CreateFromResource(
                    this, Resource.Array.sexo, Android.Resource.Layout.SimpleSpinnerItem);

            adapter2.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spinner2.Adapter = adapter2;

            //Llenado Spinner Tipo Sangre
            spinner3.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinner3_ItemSelected);
            var adapter3 = ArrayAdapter.CreateFromResource(
                    this, Resource.Array.tipo_sangre, Android.Resource.Layout.SimpleSpinnerItem);

            adapter3.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spinner3.Adapter = adapter3;

            //Obtengo objetos de componentes
            Button enviar = FindViewById<Button>(Resource.Id.btnGuardar);

            enviar.Click += (object sender, EventArgs e) =>
            {

                Object[] data = new Object[10];

                EditText nombre = FindViewById<EditText>(Resource.Id.txtNombre);
                EditText apellido = FindViewById<EditText>(Resource.Id.txtApellido);
                EditText edad = FindViewById<EditText>(Resource.Id.txtEdad);
                EditText presion = FindViewById<EditText>(Resource.Id.txtPresion);
                EditText pulso = FindViewById<EditText>(Resource.Id.txtPulso);
                EditText diagnostico = FindViewById<EditText>(Resource.Id.txtDiagnostico);

                //Select id
                try
                {
                    MySqlConnection sqlconn = new MySqlConnection(connsqlstring);
                    sqlconn.Open();

                    string Query = "SELECT idAmbulancia FROM ambulanciasdisponibles WHERE nombreAmbulancia='" + s1 + "';";

                    DataTable t = new DataTable();
                    try
                    {
                        MySqlDataAdapter da = new MySqlDataAdapter(Query, sqlconn);
                        da.Fill(t);
                        DataRow row = t.Rows[0];
                        data[0] = row["idAmbulancia"].ToString();

                    }
                    catch (MySqlException ex)
                    {

                    }

                    sqlconn.Close();

                    string toast = string.Format(" Aviso: Reporte Enviado");
                    Toast.MakeText(this, toast, ToastLength.Long).Show();
                }
                catch (Exception ex)
                {
                    Toast.MakeText(this, ex.Message, ToastLength.Long).Show();
                }

                DateTime thisDay = DateTime.Now;
                data[1] = thisDay.Day.ToString() + "/" + thisDay.Month.ToString() + "/" + thisDay.Year.ToString() + " " + thisDay.Hour.ToString() + ":" + thisDay.Minute.ToString() + ":" + thisDay.Second.ToString();
                data[2] = nombre.Text.ToString();
                data[3] = apellido.Text.ToString();
                data[4] = s2;
                data[5] = edad.Text.ToString();
                data[6] = presion.Text.ToString();
                data[7] = pulso.Text.ToString();
                data[8] = s3;
                data[9] = diagnostico.Text.ToString();

                //Insercion
                try
                {
                    MySqlConnection sqlconn = new MySqlConnection(connsqlstring);
                    sqlconn.Open();

                    string nonQuery = "INSERT INTO reporte_medico(idAmbulancia,fechaReporte,nombrePaciente,apellidoPaciente,sexoPaciente,edadPaciente,presionPaciente,pulsoPaciente,sangrePaciente,diagnosticoPaciente)" +
                " VALUES({0},'{1}','{2}','{3}','{4}',{5},'{6}','{7}','{8}','{9}');";
                    nonQuery = String.Format(nonQuery, data[0], data[1], data[2], data[3], data[4], data[5], data[6], data[7], data[8], data[9]);

                    MySqlCommand instruccion = new MySqlCommand(nonQuery, sqlconn);

                    instruccion.ExecuteNonQuery();

                    sqlconn.Close();

                    string toast = string.Format(" Aviso: Reporte Enviado");
                    Toast.MakeText(this, toast, ToastLength.Long).Show();
                }
                catch (Exception ex)
                {
                    Toast.MakeText(this, ex.Message, ToastLength.Long).Show();
                }
            };
        }

        private void spinner1_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            Spinner spinner = (Spinner)sender;

            s1 = "" + spinner.GetItemAtPosition(e.Position);
            //string toast = string.Format("The planet is {0}", spinner.GetItemAtPosition(e.Position));
            //Toast.MakeText(this, toast, ToastLength.Long).Show();
        }

        private void spinner2_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            Spinner spinner = (Spinner)sender;

            s2 = "" + spinner.GetItemAtPosition(e.Position);
            //string toast = string.Format("The planet is {0}", spinner.GetItemAtPosition(e.Position));
            //Toast.MakeText(this, toast, ToastLength.Long).Show();
        }

        private void spinner3_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            Spinner spinner = (Spinner)sender;

            s3 = "" + spinner.GetItemAtPosition(e.Position);
            //string toast = string.Format("The planet is {0}", spinner.GetItemAtPosition(e.Position));
            //Toast.MakeText(this, toast, ToastLength.Long).Show();
        }
    }
}

