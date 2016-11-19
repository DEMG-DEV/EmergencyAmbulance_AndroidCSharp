using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MySql.Data.MySqlClient;
using System.Data;

namespace EmergencyAmbulance_AndroidCSharp
{
    [Activity(MainLauncher = true, Theme = "@style/ThemeCustom", Label = "EmergencyAmbulance", Icon = "@drawable/logo")]
    public class Login : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            //a partir de aqui se programa
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Login);

            // Create your application here
            Button enviar = FindViewById<Button>(Resource.Id.button1);

            enviar.Click += (object sender, EventArgs e) =>
            {
                EditText servidor = FindViewById<EditText>(Resource.Id.editText1);
                EditText user = FindViewById<EditText>(Resource.Id.editText2);
                EditText pass = FindViewById<EditText>(Resource.Id.editText3);

                //string toast = string.Format("" + servidor.Text.ToString() + " " + user.Text.ToString() + " " + pass.Text.ToString());
                //Toast.MakeText(this, toast, ToastLength.Long).Show();

                string connsqlstring = "Server={0};Port=3306;database=emergency;Uid={1};Pwd={2};";
                connsqlstring = string.Format(connsqlstring, servidor.Text, user.Text, pass.Text);

                MySqlConnection sqlconn = new MySqlConnection(connsqlstring);
                try
                {
                    sqlconn.Open();

                    string Query = "SELECT count(0) FROM reporte_medico;";

                    MySqlCommand da = new MySqlCommand(Query, sqlconn);
                    string conexion = da.ExecuteScalar().ToString();

                    string toast = string.Format(" Aviso: Conexion Exitosa");
                    Toast.MakeText(this, toast, ToastLength.Long).Show();


                    if (conexion != "")
                    {
                        var intent = new Intent(this, typeof(MainActivity));
                        intent.PutExtra("servidor", servidor.Text);
                        intent.PutExtra("user", user.Text);
                        intent.PutExtra("pass", pass.Text);
                        StartActivity(intent);
                    }
                }
                catch (Exception ex)
                {
                    string toast = string.Format(" Aviso: "+ex.Message);
                    Toast.MakeText(this, toast, ToastLength.Long).Show();
                }
                finally
                {
                    sqlconn.Close();
                }
            };
        }
    }
}