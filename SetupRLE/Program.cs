using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;

namespace SetupRLE
{
    static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        /// 
        static List<string> archivos = new List<string>();
        static int cntArchivo = 1;
        [STAThread]
        static void Main()
        {
            archivos.Add("SetupRLE.Renci.SshNet.dll");
            archivos.Add("MySql.Data.dll");
            archivos.Add("System.Management.dll");
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolver);
           // AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolverRenci);
            Application.Run(new frm());
        }

        static Assembly CurrentDomain_AssemblyResolver(object sender, ResolveEventArgs args)
        {
            if (cntArchivo == 1)
            {
                using (var stream = Assembly.GetEntryAssembly().GetManifestResourceStream("SetupRLE.Renci.SshNet.dll"))
                {
                    byte[] assemblyData = new byte[stream.Length];
                    stream.Read(assemblyData, 0, assemblyData.Length);
                    cntArchivo++;
                    return Assembly.Load(assemblyData);
                }

            }
            else if (cntArchivo == 2)
            {
                using (var stream = Assembly.GetEntryAssembly().GetManifestResourceStream("SetupRLE.MySql.Data.dll"))
                {
                    byte[] assemblyData = new byte[stream.Length];
                    stream.Read(assemblyData, 0, assemblyData.Length);
                    cntArchivo++;
                    return Assembly.Load(assemblyData);
                }
            }
            //else if (cntArchivo == 3)
            //{
            //    using (var stream = Assembly.GetEntryAssembly().GetManifestResourceStream("SetupRLE.System.Management.dll"))
            //    {
            //        byte[] assemblyData = new byte[stream.Length];
            //        stream.Read(assemblyData, 0, assemblyData.Length);
            //        cntArchivo++;
            //        return Assembly.Load(assemblyData);
            //    }

            //}
            else
            {
                cntArchivo = 1;
                return null;
            }


        }

        static Assembly CurrentDomain_AssemblyResolverRenci(object sender, ResolveEventArgs args)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            List<string> embeddedResources = new List<string>(assembly.GetManifestResourceNames());

            string assemblyName = new AssemblyName(args.Name).Name;
            string fileName = string.Format("{0}.dll", assemblyName);
            string resourceName = embeddedResources.Where(ern => ern.EndsWith(fileName)).FirstOrDefault();

            if (!string.IsNullOrWhiteSpace(resourceName))
            {
                using (var stream = assembly.GetManifestResourceStream(resourceName))
                {
                    Byte[] assemblyData = new Byte[stream.Length];
                    stream.Read(assemblyData, 0, assemblyData.Length);
                    var test = Assembly.Load(assemblyData);
                    string namespace_ = test.GetTypes().Where(t => t.Name == assemblyName).Select(t => t.Namespace).FirstOrDefault();
                    return Assembly.Load(assemblyData);
                }
            }

            return null;

            //using (var stream = Assembly.GetEntryAssembly().GetManifestResourceStream("SetupRLE.Renci.SshNet.dll"))
            // {
            //     byte[] assemblyData = new byte[stream.Length];
            //     stream.Read(assemblyData, 0, assemblyData.Length);
            //     return Assembly.Load(assemblyData);
            // }
        }
    }
}
