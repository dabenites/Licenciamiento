using System;
using System.Net.Mail;
using MySql.Data.MySqlClient;
using System.Windows.Forms;
using System.Data;
using Renci.SshNet;
using Renci.SshNet.Sftp;
using System.IO;
using System.Text;
using System.Net.NetworkInformation;
using System.Management;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Linq;

namespace SetupRLE
{
    public partial class frm : Form
    {

        private int cantidaArchivos = 0;
        List<string> lctArchivo = new List<string>();
        public frm()
        {
            InitializeComponent();
        }

        private void btn_Solicitar_Click(object sender, EventArgs e)
        {
            string correoUser = txt_correo.Text;
            // validar que no tenga licencia ocupada.
            if (verificarExisteCorreo(correoUser))
            {
                if (verificaTieneLicencia(correoUser))
                {
                    lbl_mensajeCorreo.Text = "El usuario ya tiene asociada una licencia con otro correo.";
                    lbl_mensajeCorreo.Visible = true;
                }
                else
                {
                    string codigo = GeneroCodigotemporal(correoUser);

                    if (codigo != "")
                    {
                        if (solicitarKeyIngresoMail(correoUser, codigo))
                        {
                            MuestroInformacionSegunEstado(2);
                            lbl_mensajeCorreo.Text = "Se ha enviado un mail con el código, Favor ingresar.";
                            lbl_mensajeCorreo.Visible = true;
                        }
                    }
                    else
                    {
                        //  MessageBox.Show("Problema al generar el archivo");
                        lbl_mensajeCorreo.Text = "Problema al generar la Key, contactar a I+D.";
                        lbl_mensajeCorreo.Visible = true;
                    }
                }
            }
            else
            {
                //MessageBox.Show("Correo no existe");
                lbl_mensajeCorreo.Text = "Correo ingresado no existe en nuestros registros.";
                lbl_mensajeCorreo.Visible = true;
            }
        }



        private Boolean solicitarKeyIngresoMail(string correo , string codigo)
        {
            // 
            MailMessage mmsg = new MailMessage();
            mmsg.To.Add(correo);
            mmsg.Subject = "Sistema Licencia RLE - SUITE";
            mmsg.SubjectEncoding = System.Text.Encoding.UTF8;
            mmsg.Body = "El código de autorización es el siguiente  : " + codigo;
            mmsg.BodyEncoding = System.Text.Encoding.UTF8;
            mmsg.From = new System.Net.Mail.MailAddress("licencias.bim@renelagos.com");

            SmtpClient cliente = new SmtpClient();
            cliente.Credentials = new System.Net.NetworkCredential("licencias.bim@renelagos.com", "123456789RlE");

            cliente.Port = 587;
            cliente.EnableSsl = true;
            cliente.Host = "smtp.gmail.com";

            try
            {
                cliente.Send(mmsg);
                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw;
            }

        }

        private void btn_Ingresar_Click(object sender, EventArgs e)
        {
            // Validar la informacion que nos enviara la base de datos
            verificarCodigoEnviado();
        }



        // comprobacion que el mail ingresado exista en los registros de la aplicación 
        private Boolean verificaTieneLicencia(string correo)
        {
            // preguntar si existe en la base de datos este correo.

            Boolean estado = false;

            string database = "rle_aws";
            string server = "18.228.215.203"; // verificar si podemos llevar esto a un servidor en la nube.
            string userName = "root";
            string password = "NRle.2019$TI*.";

            string connectionString = "datasource=" + server + ";port=3306;username=" + userName + ";password=" + password + ";database=" + database + ";";


            string query = " SELECT " +
                                    " t1.id_sist_user_install, " +
                                    " t1.fechaSolicitud, " +
                                    " t1.fechaCaduca " +
                           " FROM " +
                                    " sist_user_install AS t1 " +
                           "  WHERE " +
                                    " t1.mailSolicitante = '" + correo + "'";


            MySqlConnection databaseConnection = new MySqlConnection(connectionString);
            MySqlCommand commandDatabase = new MySqlCommand(query, databaseConnection);
            commandDatabase.CommandTimeout = 60;
            MySqlDataReader reader;


            try
            {
                databaseConnection.Open();
                reader = commandDatabase.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        estado = true;
                    }
                }
                else
                {

                }

                databaseConnection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }

            return estado;
        }

        private Boolean verificarExisteCorreo(string correo)
        {
            // preguntar si existe en la base de datos este correo.

            Boolean estado = false;

            string database = "rle_aws";
            string server = "18.228.215.203"; // verificar si podemos llevar esto a un servidor en la nube.
            string userName = "root";
            string password = "NRle.2019$TI*.";

                string connectionString = "datasource=" + server + ";port=3306;username=" + userName + ";password=" + password + ";database=" + database + ";";


                string query = " SELECT " +
                                        " t1.login, " +
                                        " t1.nombre, " +
                                        " t1.correo_electronico " +
                               " FROM " +
                                        " sist_user AS t1 "+
                               "  WHERE " +
                                        " t1.correo_electronico = '" + correo + "'";


                MySqlConnection databaseConnection = new MySqlConnection(connectionString);
                MySqlCommand commandDatabase = new MySqlCommand(query, databaseConnection);
                commandDatabase.CommandTimeout = 60;
                MySqlDataReader reader;

               
                try
                {
                    databaseConnection.Open();
                    reader = commandDatabase.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            estado = true;
                        }
                    }
                    else
                    {

                    }

                    databaseConnection.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);

                }

            return estado;
        }


        private string  GeneroCodigotemporal(string correo)
        {
            string loginUser = "";
            string codigoEnviar = "";

            // primer paso que tengo que realizar es obtener el login del usuario asociado al correo.
            loginUser = getLoginBD(correo);
            codigoEnviar = getCodigoEnviaMail();

            if (loginUser == "" || codigoEnviar == "")
            {
                // trabajar en la logina de ingreso 
                return "";
            }
            else
            {
                // realizar el registro de la peticion en la base de datos.
                if (!RegistroEnvio(correo, codigoEnviar))
                {
                    return "";
                }
            }
            return codigoEnviar;
        }


        private string getLoginBD(string correo)
        {
            string login = "";


            string database = "rle_aws";
            string server = "18.228.215.203"; // verificar si podemos llevar esto a un servidor en la nube.
            string userName = "root";
            string password = "NRle.2019$TI*.";

            string connectionString = "datasource=" + server + ";port=3306;username=" + userName + ";password=" + password + ";database=" + database + ";";


            string query = " SELECT " +
                                    " t1.login, " +
                                    " t1.nombre, " +
                                    " t1.correo_electronico " +
                           " FROM " +
                                    " sist_user AS t1 " +
                           "  WHERE " +
                                    " t1.correo_electronico = '" + correo + "'";


            MySqlConnection databaseConnection = new MySqlConnection(connectionString);
            MySqlCommand commandDatabase = new MySqlCommand(query, databaseConnection);
            commandDatabase.CommandTimeout = 60;
            MySqlDataReader reader;


            try
            {
                databaseConnection.Open();
                reader = commandDatabase.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        login = reader.GetString(0);
                    }
                }
                databaseConnection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }


            return login;
        }


        private string getCodigoEnviaMail()
        {
            string codigo = "";

            string database = "rle_aws";
            string server = "18.228.215.203"; // verificar si podemos llevar esto a un servidor en la nube.
            string userName = "root";
            string password = "NRle.2019$TI*.";

            string connectionString = "datasource=" + server + ";port=3306;username=" + userName + ";password=" + password + ";database=" + database + ";";


            string query = " select " +
                                " concat(substring('ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789', rand() * 36 + 1, 1), " +
                                " substring('ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789', rand() * 36 + 1, 1), " +
                                " substring('ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789', rand() * 36 + 1, 1)," +
                                " substring('ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789', rand() * 36 + 1, 1)," +
                                " substring('ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789', rand() * 36 + 1, 1)," +
                                " substring('ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789', rand() * 36 + 1, 1)," +
                                " substring('ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789', rand() * 36 + 1, 1)," +
                                " substring('ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789', rand() * 36 + 1, 1)) as LicensePlaceNumber";


            MySqlConnection databaseConnection = new MySqlConnection(connectionString);
            MySqlCommand commandDatabase = new MySqlCommand(query, databaseConnection);
            commandDatabase.CommandTimeout = 60;
            MySqlDataReader reader;


            try
            {
                databaseConnection.Open();
                reader = commandDatabase.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        codigo = reader.GetString(0);
                    }
                }
                databaseConnection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }

            return codigo;
        }


        private Boolean RegistroEnvio(string correo, string codigo)
        {

            string database = "rle_aws";
            string server = "18.228.215.203"; // verificar si podemos llevar esto a un servidor en la nube.
            string userName = "root";
            string password = "NRle.2019$TI*.";

            string connectionString = "datasource=" + server + ";port=3306;username=" + userName + ";password=" + password + ";database=" + database + ";";


            string query = @"  INSERT " +
                                    " INTO " +
                                           " sist_user_install " +
                                    " ( " +
                                            " fechaSolicitud, " +
                                            " mailSolicitante, " +
                                            " fechaCaduca, " +
                                            " codigoEntregado, " +
                                            " id_estado " +
                                    " ) " +
                                    " VALUES " +
                                    " ( NOW()  ,'"+ correo + "' , DATE_ADD(NOW(),INTERVAL 60 MINUTE), '" + codigo + "' , '1' ) ";


            MySqlConnection databaseConnection = new MySqlConnection(connectionString);
            try
            {
                MySqlCommand cmd = new MySqlCommand(query, databaseConnection);
                databaseConnection.Open();
                cmd.ExecuteNonQuery();
                cmd.CommandText = "SELECT LAST_INSERT_ID()";
                IDataReader reader = cmd.ExecuteReader();
                if (reader != null && reader.Read())
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("error " + ex.Message);
                return false;
            }

        }


        private void verificarCodigoEnviado()
        {
            string correo = txt_correo.Text;
            string codigo = txt_codigo.Text;

            if (validoCodigo(correo, codigo))
            {
                if (ActualizoEstadoDescarga(correo, codigo, "2","1"))
                {
                    // mostrar la informacion para poder descargar las rutinas 
                    lbl_mensajeCodigo.Text = "Descargar la rutina.";
                    lbl_mensajeCodigo.Visible = true;
                    MuestroInformacionSegunEstado(3);
                }
            }
            else
            {
                lbl_mensajeCodigo.Text = "Código ingresado no es válido, favor verificar.";
                lbl_mensajeCodigo.Visible = true;
            }
        }
        private Boolean validoCodigo(string correo, string codigo)
        {
            // preguntar si existe en la base de datos este correo.

            Boolean estado = false;

            string database = "rle_aws";
            string server = "18.228.215.203"; // verificar si podemos llevar esto a un servidor en la nube.
            string userName = "root";
            string password = "NRle.2019$TI*.";

            string connectionString = "datasource=" + server + ";port=3306;username=" + userName + ";password=" + password + ";database=" + database + ";";


            string query = " SELECT " +
                                    " t1.id_sist_user_install " +
                           " FROM " +
                                    " sist_user_install AS t1 " +
                           "  WHERE " +
                                    " t1.mailSolicitante = '" + correo + "'" +
                           " AND " +
                                    " t1.codigoEntregado = '" + codigo+ "'" +
                           " AND " + 
                                    " t1.id_estado = 1";


            MySqlConnection databaseConnection = new MySqlConnection(connectionString);
            MySqlCommand commandDatabase = new MySqlCommand(query, databaseConnection);
            commandDatabase.CommandTimeout = 60;
            MySqlDataReader reader;


            try
            {
                databaseConnection.Open();
                reader = commandDatabase.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        estado = true;
                    }
                }
                else
                {

                }

                databaseConnection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }

            return estado;


        }


        private Boolean ActualizoEstadoDescarga(string correo, string codigo, string estadoActual, string estadoRequerido)
        {
            Boolean estado = false;

            string database = "rle_aws";
            string server = "18.228.215.203"; // verificar si podemos llevar esto a un servidor en la nube.
            string userName = "root";
            string password = "NRle.2019$TI*.";

            string connectionString = "datasource=" + server + ";port=3306;username=" + userName + ";password=" + password + ";database=" + database + ";";

            string query = @"  UPDATE " +
                                           " sist_user_install as t1 " +
                                    " SET " +
                                            " t1.id_estado = '" + estadoActual + "', t1.fechaIngreso = now()" +
                                      "  WHERE " +
                                             " t1.mailSolicitante = '" + correo + "'" +
                                      " AND " +
                                             " t1.codigoEntregado = '" + codigo + "'" +
                                     " AND " +
                                             " t1.id_estado = '"+ estadoRequerido + "'";

            MySqlConnection databaseConnection = new MySqlConnection(connectionString);
            try
            {
                MySqlCommand cmd = new MySqlCommand(query, databaseConnection);
                cmd.Parameters.Add("@codigo_proyecto", MySqlDbType.VarChar).Value = codigo;
                databaseConnection.Open();
                cmd.ExecuteNonQuery();
                estado = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("error " + ex.Message);
            }



            return estado;


        }
        private Boolean ActualizoLicencia(string correo, string codigo, string m, string h , string c)
        {
            Boolean estado = false;

            string database = "rle_aws";
            string server = "18.228.215.203"; // verificar si podemos llevar esto a un servidor en la nube.
            string userName = "root";
            string password = "NRle.2019$TI*.";

            string connectionString = "datasource=" + server + ";port=3306;username=" + userName + ";password=" + password + ";database=" + database + ";";

            string query = @"  UPDATE " +
                                           " sist_user_install as t1 " +
                                    " SET " +
                                            " t1.motherBoard = '" + m + "', t1.harddisk = '"+ h + "' , t1.cpu = '"+c+"'" +
                                      "  WHERE " +
                                             " t1.mailSolicitante = '" + correo + "'" +
                                      " AND " +
                                             " t1.codigoEntregado = '" + codigo + "'";

            MySqlConnection databaseConnection = new MySqlConnection(connectionString);
            try
            {
                MySqlCommand cmd = new MySqlCommand(query, databaseConnection);
                cmd.Parameters.Add("@codigo_proyecto", MySqlDbType.VarChar).Value = codigo;
                databaseConnection.Open();
                cmd.ExecuteNonQuery();
                estado = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("error " + ex.Message);
            }



            return estado;


        }

        private void frm_Load(object sender, EventArgs e)
        {
            MuestroInformacionSegunEstado(1);


            txt_macaddress_l.Text= GetMacAddress();
            txt_motherborad_l.Text = getMotherBoard();
            txt_hardiskt_l.Text= getHardDisk();
            txt_cpu_l.Text = getCPU();

            string appData = System.Environment.GetEnvironmentVariable("APPDATA");
            lbl_destino.Text = appData + "\\login.key";
            lbl_appdata.Text = appData;
        }

        private void MuestroInformacionSegunEstado(int estado)
        {
            switch (estado)
            {
                case 1:
                    lbl_correo.Visible = true;
                    txt_correo.Visible = true;
                    btn_Solicitar.Visible = true;

                    lbl_Codigo.Visible = false;
                    txt_codigo.Visible = false;
                    btn_verificar.Visible = false;

                    btn_Descargar.Visible = false;
                    //btn_Descargar.Visible = true;
                    pgr_descarga.Visible = false;
                break;
                case 2:
                    lbl_correo.Visible = true;
                    txt_correo.Visible = true;
                    btn_Solicitar.Visible = true;

                    txt_correo.ReadOnly = true;
                    btn_Solicitar.Enabled = false;

                    lbl_Codigo.Visible = true;
                    txt_codigo.Visible = true;
                    btn_verificar.Visible = true;

                    btn_Descargar.Visible = false;
                    pgr_descarga.Visible = false;
                    break;
                case 3:
                    lbl_correo.Visible = true;
                    txt_correo.Visible = true;
                    btn_Solicitar.Visible = true;

                    txt_correo.ReadOnly = true;
                    btn_Solicitar.Enabled = false;

                    lbl_Codigo.Visible = true;
                    txt_codigo.Visible = true;
                    btn_verificar.Visible = true;

                    txt_codigo.ReadOnly = true;
                    btn_verificar.Enabled = false;
                    btn_Descargar.Visible = true;
                    pgr_descarga.Visible = true;
                    break;
            }
        }

        private void btn_Descargar_Click(object sender, EventArgs e)
        {
            DescargarRutinas();
        }


        private void DescargarRutinas()
        {

            // validar que las rutinas no existan anteriormente.



            string host = "18.228.215.203";
            string username = "apprle";
            string password = "AdsRle$*2019.";

            string annio = "2019";
            string rutaSFTP = "/rleinstall/Revit/2019/Version_vigente/";
            string ruta_local = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Autodesk\Revit\Addins\" + annio + @"\";


            // verificar si existe la carpeta 
            if (Directory.Exists(ruta_local + "RLE_Suite_2019"))
            {
                // verificar si puedo borrar la carpeta
                EraseDirectory(ruta_local + "RLE_Suite_2019", true);
            }
            if (File.Exists(ruta_local + "RLE_Suite_2019.addin"))
            {
                File.Delete(ruta_local + "RLE_Suite_2019.addin");
            }

            using (SftpClient sftp = new SftpClient(host, username, password))
            {
                try
                {
                    btn_Descargar.Enabled = false;
                    sftp.Connect();
                    cantidaArchivos = 0;
                    __descarga.Text = "Los archivos se estan descargando, favor esperar";
                    __descarga.Visible = true;
                    CantidadArchivosServisor(sftp, rutaSFTP);
                    setMaxProgressBar(cantidaArchivos);
                    DownloadDirectory(sftp, rutaSFTP, ruta_local);
                    setReloadProgressBar();
                    sftp.Disconnect();

                    pgr_descarga.Visible = false;
                    btn_Descargar.Visible = false;
                    lbl_descarga.Text = "Se han descargado las rutinas y licenciado el programa.";
                    lbl_descarga.Visible = true;
                    __descarga.Visible = false;
                    string correo = txt_correo.Text;
                    string codigo = txt_codigo.Text;
                    ActualizoEstadoDescarga(correo, codigo, "3", "2");
                    LicenciarProyecto(correo,codigo);
                }
                catch (Exception eT)
                {
                    // Console.WriteLine("An exception has been caught " + eT.ToString());
                    lbl_descarga.Text = "Problema en descargar las rutinas " + eT.Message.ToString();
                    lbl_descarga.Visible = true;
                    pgr_descarga.Visible = false;
                    btn_Descargar.Visible = false;
                }
            }

            
        }

        private int CantidadArchivosServisor(SftpClient client , string source)
        {
            var files = client.ListDirectory(source);
            
            foreach (var file in files)
            {
                
                if (Path.GetExtension(file.FullName) != ".dll")
                {
                    if (!file.IsDirectory && !file.IsSymbolicLink)
                    {
                        //DownloadFile(client, file, destination);
                        cantidaArchivos = cantidaArchivos + 1;
                    }
                    else if (file.IsSymbolicLink)
                    {
                        Console.WriteLine("Ignoring symbolic link {0}", file.FullName);
                    }
                    else if (file.Name != "." && file.Name != "..")
                    {
                        CantidadArchivosServisor(client, file.FullName);
                    }
                }
                else
                {
                    if (!file.IsDirectory && !file.IsSymbolicLink)
                    {
                        // DownloadFile(client, file, destination);
                        cantidaArchivos = cantidaArchivos + 1;
                    }
                }
            }
            return cantidaArchivos;
        }

        private  void DownloadDirectory(SftpClient client, string source, string destination)
        {
            var files = client.ListDirectory(source);
            foreach (var file in files)
            {
                if (Path.GetExtension(file.FullName) != ".dll")
                {
                    if (!file.IsDirectory && !file.IsSymbolicLink)
                    {
                        DownloadFile(client, file, destination);
                        setPerformStep();
                    }
                    else if (file.IsSymbolicLink)
                    {
                        Console.WriteLine("Ignoring symbolic link {0}", file.FullName);
                    }
                    else if (file.Name != "." && file.Name != "..")
                    {
                        var dir = Directory.CreateDirectory(Path.Combine(destination, file.Name));
                        DownloadDirectory(client, file.FullName, dir.FullName);
                    }
                }
                else
                {
                    if (!file.IsDirectory && !file.IsSymbolicLink)
                    {
                        DownloadFile(client, file, destination);
                        setPerformStep();
                    }
                }
            }
        }
        private static void DownloadFile(SftpClient client, SftpFile file, string directory)
        {
            using (Stream fileStream = File.OpenWrite(Path.Combine(directory, file.Name)))
            {
                client.DownloadFile(file.FullName, fileStream);
            }
        }

        public void setMaxProgressBar(int cantidad)
        {

            pgr_descarga.Minimum = 0;

            pgr_descarga.Maximum = cantidad;

            pgr_descarga.Step = 1;

        }

        public void setPerformStep()
        {
            pgr_descarga.PerformStep();
        }

        public void setReloadProgressBar()
        {
            pgr_descarga.Minimum = 0;
            pgr_descarga.Value = 0;
        }

        public static bool EraseDirectory(string folderPath, bool recursive)
        {
            //Safety check for directory existence.
            if (!Directory.Exists(folderPath))
                return false;

            foreach (string file in Directory.GetFiles(folderPath))
            {
                File.Delete(file);
            }

            //Iterate to sub directory only if required.
            if (recursive)
            {
                foreach (string dir in Directory.GetDirectories(folderPath))
                {
                    EraseDirectory(dir, recursive);
                }
            }
            //Delete the parent directory before leaving
            Directory.Delete(folderPath);
            return true;
        }

        private void LicenciarProyecto(string correo, string codigo)
        {

            //string path = @"c:\temp\MyTest.txt";
            string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\login.key";


            //DIRECCION FISICA:30 - 9C - 23 - 0D - B5 - D0
            //LOGIN: dbenites

            try
            {
                string correoUser = correo;
                string macAddress = GetMacAddress();
                string motherBoard = getMotherBoard();
                string hardDisk = getHardDisk();
                string cpu = getCPU();
                string g = getLoginBD(correoUser);
                // Create the file, or overwrite if the file exists.
                using (FileStream fs = File.Create(path))
                {
                    byte[] info = new UTF8Encoding(true).GetBytes("DIRECCION FISICA:"+ macAddress + "\nLOGIN:"+ g + "\nMOTHERBOARD:"+motherBoard+"\nHARDDISK:"+ hardDisk + "\nCPU:"+cpu+"");
                    // Add some information to the file.
                    fs.Write(info, 0, info.Length);
                }

                // Open the stream and read it back.
                using (StreamReader sr = File.OpenText(path))
                {
                    string s = "";
                    while ((s = sr.ReadLine()) != null)
                    {
                        Console.WriteLine(s);
                    }
                }
                string keyEncrypter = "renelago";
                EncryptFile(path, keyEncrypter);
                ActualizoLicencia(correo, codigo, motherBoard, hardDisk, cpu);
                CargoLicenciaAlServidor(correo, codigo, motherBoard, hardDisk, cpu,macAddress);
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

        }

        private string GetMacAddress()
        {
            string macAddresses = string.Empty;

            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (nic.OperationalStatus == OperationalStatus.Up)
                {
                    macAddresses += nic.GetPhysicalAddress().ToString();
                    //break;
                }
            }

            return macAddresses;
        }

        private string getMotherBoard()
        {
            ManagementObjectSearcher mos = new ManagementObjectSearcher("SELECT * FROM Win32_BaseBoard");
            ManagementObjectCollection moc = mos.Get();
            string serial = "";
            foreach (ManagementObject mo in moc)
            {
                serial = (string)mo["SerialNumber"];
            }

            return serial;
        }

        private string getHardDisk()
        {
            ManagementObject dsk = new ManagementObject(@"win32_logicaldisk.deviceid=""c:""");
            dsk.Get();
            string id = dsk["VolumeSerialNumber"].ToString();

            return id;

        }
        private string getCPU()
        {
            ManagementObjectCollection mbsList = null;
            ManagementObjectSearcher mbs = new ManagementObjectSearcher("Select * From Win32_processor");
            mbsList = mbs.Get();
            string id = "";
            foreach (ManagementObject mo in mbsList)
            {
                id = mo["ProcessorID"].ToString();
            }
            return id;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //LicenciarProyecto();
        }

        static void EncryptFile(string file, string key)
        {
            byte[] plainContent = File.ReadAllBytes(file);

            using (var DES = new DESCryptoServiceProvider())
            {
                DES.IV = Encoding.UTF8.GetBytes(key);
                DES.Key = Encoding.UTF8.GetBytes(key);
                DES.Mode = CipherMode.CBC;
                DES.Padding = PaddingMode.PKCS7;

                using (var memStream = new MemoryStream())
                {
                    CryptoStream cryptoStream = new CryptoStream(memStream, DES.CreateEncryptor(), CryptoStreamMode.Write);

                    cryptoStream.Write(plainContent, 0, plainContent.Length);
                    cryptoStream.FlushFinalBlock();
                    File.WriteAllBytes(file, memStream.ToArray());
                }
            }

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            LicenciarProyecto("asdada","asdsa");
        }

        private Boolean CargoLicenciaAlServidor(string correo, string codigo, string motherBoard, string hardDisk, string cpu , string mc)
        {
            string database = "rle_aws";
            string server = "18.228.215.203"; // verificar si podemos llevar esto a un servidor en la nube.
            string userName = "root";
            string password = "NRle.2019$TI*.";

            string connectionString = "datasource=" + server + ";port=3306;username=" + userName + ";password=" + password + ";database=" + database + ";";

            string g = getLoginBD(correo);
            string query = @"  INSERT " +
                                    " INTO " +
                                           " sist_user_keys " +
                                    " ( " +
                                            " login, " +
                                            " fechaInicio, " +
                                            " fechaTermino, " +
                                            " mac, " +
                                            " motherBoard, " +
                                            " harddisk, " +
                                            " cpu " +
                                    " ) " +
                                    " VALUES " +
                                    " ( '" + g + "'  ,NOW(), DATE_ADD(NOW(),INTERVAL 1 YEAR),'"+ mc + "' , '" + motherBoard + "', '" + hardDisk + "' , '" + cpu + "') ";


            MySqlConnection databaseConnection = new MySqlConnection(connectionString);
            try
            {
                MySqlCommand cmd = new MySqlCommand(query, databaseConnection);
                databaseConnection.Open();
                cmd.ExecuteNonQuery();
                cmd.CommandText = "SELECT LAST_INSERT_ID()";
                IDataReader reader = cmd.ExecuteReader();
                if (reader != null && reader.Read())
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("error " + ex.Message);
                return false;
            }

        }



        private string buscoCorreoByLogin(string login)
        {

            string correo = "";

            string database = "rle_aws";
            string server = "18.228.215.203"; // verificar si podemos llevar esto a un servidor en la nube.
            string userName = "root";
            string password = "NRle.2019$TI*.";

            string connectionString = "datasource=" + server + ";port=3306;username=" + userName + ";password=" + password + ";database=" + database + ";";


            string query = " SELECT " +
                                    " t1.correo_electronico " +
                           " FROM " +
                                    " sist_user AS t1 " +
                           "  WHERE " +
                                    " t1.login = '" + login + "'";


            MySqlConnection databaseConnection = new MySqlConnection(connectionString);
            MySqlCommand commandDatabase = new MySqlCommand(query, databaseConnection);
            commandDatabase.CommandTimeout = 60;
            MySqlDataReader reader;


            try
            {
                databaseConnection.Open();
                reader = commandDatabase.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        correo = reader.GetString(0);
                    }
                }
                else
                {

                }

                databaseConnection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }

            return correo;
        }

        private void button1_Click_2(object sender, EventArgs e)
        {
            string path = @"C:\\Users\\dbenites\\Desktop\\RENE LAGOS ENGINEERS\\RLE INSTALL\\aquino2\\login.key";


            //DIRECCION FISICA:30 - 9C - 23 - 0D - B5 - D0
            //LOGIN: dbenites

            try
            {
                string correoUser = "ang.aquino92@gmail.com";
                string macAddress = "D43D7E56EC7B";
                string motherBoard = "To be filled by O.E.M.";
                string hardDisk = "A4ADE838";
                string cpu = "BFEBFBFF000306A9";
                string g = "aaquino2";
                // Create the file, or overwrite if the file exists.
                using (FileStream fs = File.Create(path))
                {
                    byte[] info = new UTF8Encoding(true).GetBytes("DIRECCION FISICA:" + macAddress + "\nLOGIN:" + g + "\nMOTHERBOARD:" + motherBoard + "\nHARDDISK:" + hardDisk + "\nCPU:" + cpu + "");
                    // Add some information to the file.
                    fs.Write(info, 0, info.Length);
                }

                // Open the stream and read it back.
                using (StreamReader sr = File.OpenText(path))
                {
                    string s = "";
                    while ((s = sr.ReadLine()) != null)
                    {
                        Console.WriteLine(s);
                    }
                }
                string keyEncrypter = "renelago";
                EncryptFile(path, keyEncrypter);
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void btn_Folder_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog fbd_ruta = new System.Windows.Forms.FolderBrowserDialog();
            fbd_ruta.Description = "Seleccione donde quedara la licencia";

            if (fbd_ruta.ShowDialog() == DialogResult.OK)
            {
                string ruta  = fbd_ruta.SelectedPath + "\\login.key";
                lbl_destino.Text = ruta;
            }

        }

        private void btn_Generar_Click(object sender, EventArgs e)
        {
            string path = lbl_destino.Text;


            //DIRECCION FISICA:30 - 9C - 23 - 0D - B5 - D0
            //LOGIN: dbenites

            try
            {
                string correoUser = txt_correo_base.Text;
                string macAddress = txt_macadress_bd.Text;
                string motherBoard = txt_morherboard_bd.Text;
                string hardDisk = txt_hardisk_bd.Text;
                string cpu = txt_cpu_bd.Text;
                string g = txt_login_l.Text;
                // Create the file, or overwrite if the file exists.
                using (FileStream fs = File.Create(path))
                {
                    byte[] info = new UTF8Encoding(true).GetBytes("DIRECCION FISICA:" + macAddress + "\nLOGIN:" + g + "\nMOTHERBOARD:" + motherBoard + "\nHARDDISK:" + hardDisk + "\nCPU:" + cpu + "");
                    // Add some information to the file.
                    fs.Write(info, 0, info.Length);
                }

                // Open the stream and read it back.
                using (StreamReader sr = File.OpenText(path))
                {
                    string s = "";
                    while ((s = sr.ReadLine()) != null)
                    {
                        Console.WriteLine(s);
                    }
                }
                string keyEncrypter = "renelago";
                EncryptFile(path, keyEncrypter);
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            btn_Buscar_Click(sender, e);
        }

        private void adminitraciónToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void btn_LecturaLicencia_Click(object sender, EventArgs e)
        {
            ///
            // revisare la informacion de la licencia cargada en el equipo.
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.ShowDialog();
            openFileDialog1.Title = "Buscar la Licencia para uso de rutinas revit";
            openFileDialog1.InitialDirectory = System.Environment.GetEnvironmentVariable("APPDATA");
            //openFileDialog1.DefaultExt = "key";

            string rutaArchivo = openFileDialog1.FileName;

            if (File.Exists(rutaArchivo))
            {
                // lectura del archivo de licenciamiento.
                string appData = System.Environment.GetEnvironmentVariable("APPDATA");
                
                string keyEncrypter = "renelago";

                //EncryptFile(file, keyEncrypter);
                DencryptFile(rutaArchivo, keyEncrypter);

                // Leeer el archivo de configuracion entregar las creadenciales del LOGIN y MAC
                //Dictionary<string, string> informacion = LecturaArchivoLicencia(file);

                List<string> informacion = LecturaArchivoLicencia(rutaArchivo);
                lctArchivo = informacion;

                EncryptFile(rutaArchivo, keyEncrypter);


                //___ cargar la informacion. 

                ///string login = Base_aws.getUserByFilev2(informacion[1], informacion[2], informacion[3], informacion[4]);
                txt_correo_equipo.Text = buscoCorreoByLogin(informacion[1]);
                txt_macaddress_equipo.Text = informacion[0];
                txt_motherboard_equipo.Text = informacion[2];
                txt_hardisk_equipo.Text = informacion[3];
                txt_cpu_equipo.Text = informacion[4];
                txt_login_equipo.Text = informacion[1];

                // verificar los componentes. 
                if (txt_macaddress_equipo.Text == txt_macaddress_l.Text) { ptb_macaddres.BackColor = System.Drawing.Color.Green; }
                else { ptb_macaddres.BackColor = System.Drawing.Color.Red; }

                if (txt_motherboard_equipo.Text == txt_motherborad_l.Text) { ptb_motherboard.BackColor = System.Drawing.Color.Green; }
                else { ptb_motherboard.BackColor = System.Drawing.Color.Red; }

                if (txt_hardisk_equipo.Text == txt_hardiskt_l.Text) { ptb_hardisk.BackColor = System.Drawing.Color.Green; }
                else { ptb_hardisk.BackColor = System.Drawing.Color.Red; }

                if (txt_cpu_equipo.Text == txt_cpu_l.Text) { ptb_cpu.BackColor = System.Drawing.Color.Green; }
                else { ptb_cpu.BackColor = System.Drawing.Color.Red; }





            }

        }

        static void DencryptFile(string file, string key)
        {
            byte[] plainContent = File.ReadAllBytes(file);

            using (var DES = new DESCryptoServiceProvider())
            {
                DES.IV = Encoding.UTF8.GetBytes(key);
                DES.Key = Encoding.UTF8.GetBytes(key);
                DES.Mode = CipherMode.CBC;
                DES.Padding = PaddingMode.PKCS7;

                using (var memStream = new MemoryStream())
                {
                    CryptoStream cryptoStream = new CryptoStream(memStream, DES.CreateDecryptor(), CryptoStreamMode.Write);

                    cryptoStream.Write(plainContent, 0, plainContent.Length);
                    cryptoStream.FlushFinalBlock();
                    File.WriteAllBytes(file, memStream.ToArray());
                }
            }

        }

        private List<string> LecturaArchivoLicencia(string rutaArchivo)
        {
            //Dictionary<string, string> info = new Dictionary<string, string>();
            List<string> informacion = new List<string>();
            // 0 LOGIN
            // 1 MAC
            // 2  

            string line = "";
            using (StreamReader file = new StreamReader(rutaArchivo))
            {
                string[] separators = { ":" };
                string mac = "";
                string login = "";
                while ((line = file.ReadLine()) != null)                //Leer linea por linea
                {
                    string[] words = line.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                    if (words.Count() == 2)
                    {
                        switch (words[0])
                        {
                            case "DIRECCION FISICA":
                                informacion.Add(words[1]);
                                break;
                            case "LOGIN":
                                informacion.Add(words[1]);
                                break;
                            case "MOTHERBOARD":
                                informacion.Add(words[1]);
                                break;
                            case "HARDDISK":
                                informacion.Add(words[1]);
                                break;
                            case "CPU":
                                informacion.Add(words[1]);
                                break;
                        }
                    }
                }
                //info.Add(login, mac);
                file.Close();
            }

            return informacion;
        }

        private void btn_Buscar_Click(object sender, EventArgs e)
        {
            // 
            List<string> informacion = informacionLicenciaByLogin(txt_login_l.Text);

            if (informacion.Count > 0)
            {
                txt_correo_base.Text = informacion[0];
                txt_macadress_bd.Text = informacion[1];
                txt_morherboard_bd.Text = informacion[2];
                txt_hardisk_bd.Text = informacion[3];
                txt_cpu_bd.Text = informacion[4];


                if (informacion[1] != "")
                {
                    btn_liberar.Visible = true;
                    btn_Generar.Visible = false;
                    btn_ImportarIds.Visible = false;
                }
                else
                {
                    btn_liberar.Visible = false;
                    btn_Generar.Visible = true;
                    btn_ImportarIds.Visible = true;
                    //txt_macadress_bd.Text = txt_macaddress_l.Text;
                    //txt_morherboard_bd.Text = txt_motherborad_l.Text;
                    //txt_hardisk_bd.Text = txt_hardiskt_l.Text;
                    //txt_cpu_bd.Text = txt_cpu_l.Text;

                }
            }
            else
            { 
             // enviar mensaje que 
            }

        }


        private List<string> informacionLicenciaByLogin(string login)
        {
            List<string> informacion = new List<string>();

            string database = "rle_aws";
            string server = "18.228.215.203"; // verificar si podemos llevar esto a un servidor en la nube.
            string userName = "root";
            string password = "NRle.2019$TI*.";

            string connectionString = "datasource=" + server + ";port=3306;username=" + userName + ";password=" + password + ";database=" + database + ";";


            string query = " SELECT " +
                                    " t1.correo_electronico, " +
                                    " if (t2.mac IS NULL , '', t2.mac)," +
                                    " if (t2.motherBoard IS NULL , '', t2.motherBoard) , " +
                                    " if (t2.harddisk IS NULL , '', t2.harddisk), " +
                                    " if (t2.cpu IS NULL , '', t2.cpu) " +
                            " FROM " +
                                    " sist_user AS t1 " +
                                    " LEFT JOIN sist_user_keys AS t2 ON t1.login = t2.login " +
                            " WHERE " +
                                    " t1.login = '" + login + "' " ;


            MySqlConnection databaseConnection = new MySqlConnection(connectionString);
            MySqlCommand commandDatabase = new MySqlCommand(query, databaseConnection);
            commandDatabase.CommandTimeout = 60;
            MySqlDataReader reader;


            try
            {
                databaseConnection.Open();
                reader = commandDatabase.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        informacion.Add(reader.GetString(0));
                        informacion.Add(reader.GetString(1));
                        informacion.Add(reader.GetString(2));
                        informacion.Add(reader.GetString(3));
                        informacion.Add(reader.GetString(4));
                    }
                }
                else
                {

                }

                databaseConnection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }

            return informacion;

        }

        private void btn_liberar_Click(object sender, EventArgs e)
        {
            // para poder eliminar la licencia tenemos que eliminar los registros de las tablas 

            // sist_user_install
            // sist_user_keys

            BorrarLicenciaUserInstall(txt_correo_base.Text);
            BorrarLicenciaUserKeys(txt_login_l.Text);

            // limpiar las variables 
            txt_macadress_bd.Text = "";
            txt_morherboard_bd.Text = "";
            txt_hardisk_bd.Text = "";
            txt_cpu_bd.Text = "";

            btn_Buscar_Click(sender, e);
        }

        private Boolean BorrarLicenciaUserInstall(string correo)
        {
            string database = "rle_aws";
            string server = "18.228.215.203"; // verificar si podemos llevar esto a un servidor en la nube.
            string userName = "root";
            string password = "NRle.2019$TI*.";

            string connectionString = "datasource=" + server + ";port=3306;username=" + userName + ";password=" + password + ";database=" + database + ";";

            string query = "DELETE FROM sist_user_install where mailSolicitante = '" + correo + "'";


            MySqlConnection databaseConnection = new MySqlConnection(connectionString);
            try
            {
                MySqlCommand cmd = new MySqlCommand(query, databaseConnection);
                databaseConnection.Open();
                cmd.ExecuteNonQuery();
                databaseConnection.Close();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("error " + ex.Message);
                return false;
            }

        }
        private Boolean BorrarLicenciaUserKeys(string login)
        {
            string database = "rle_aws";
            string server = "18.228.215.203"; // verificar si podemos llevar esto a un servidor en la nube.
            string userName = "root";
            string password = "NRle.2019$TI*.";

            string connectionString = "datasource=" + server + ";port=3306;username=" + userName + ";password=" + password + ";database=" + database + ";";

            string query = "DELETE FROM sist_user_keys  where login = '" + login + "'";


            MySqlConnection databaseConnection = new MySqlConnection(connectionString);
            try
            {
                MySqlCommand cmd = new MySqlCommand(query, databaseConnection);
                databaseConnection.Open();
                cmd.ExecuteNonQuery();
                databaseConnection.Close();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("error " + ex.Message);
                return false;
            }

        }

        private void btn_ImportarIds_Click(object sender, EventArgs e)
        {
            txt_macadress_bd.Text = txt_macaddress_l.Text;
            txt_morherboard_bd.Text = txt_motherborad_l.Text;
            txt_hardisk_bd.Text = txt_hardiskt_l.Text;
            txt_cpu_bd.Text = txt_cpu_l.Text;
        }
    }
}
