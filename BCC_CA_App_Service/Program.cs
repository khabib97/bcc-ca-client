using System;
using BCC_CA_App_Service.App;
using System.Windows.Forms;


namespace BCC_CA_App_Service
{
    class Program
    {
       
        //public static String generationMode =""; 
        //public static String serverGeneratedEnrollmentID="";

        [STAThread]
        static void Main(string[] args){

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Installer());
            //Application.Run(new UserForm());

            //  Console.WriteLine("BCC-CA Background Service");
            //Handler handler = new Handler();

            if (Environment.Is64BitProcess)
                Constants.PKCS11_LIBRARY_PATH = @"x64\eTPKCS11.dll";
            else
                Constants.PKCS11_LIBRARY_PATH = @"x32\eTPKCS11.dll";


            //try
            //{
            //    while (true) 
            //    {
            //        serverGeneratedEnrollmentID = InputHandler.GetEnrollmentID();
            //        generationMode = InputHandler.GetGenerationMode();
            //        switch (generationMode)
            //        {
            //            case Constants.GeneratedTypeCertificateOrKey.CERTIFICATE:
            //                handler.CertificateGenerator(long.Parse(serverGeneratedEnrollmentID));
            //                break;
            //            case Constants.GeneratedTypeCertificateOrKey.KEY:
            //                handler.KeyGenerator(long.Parse(serverGeneratedEnrollmentID));
            //                break;
            //        }
            //    }
            //}
            //catch (Exception ex){
            //    System.Diagnostics.Debug.WriteLine( "Error : " + ex );
            //}

            //Console.Write("Press any key to quit: ");
            Console.ReadKey();
        }

        public static void InvokePrograme(String serverGeneratedEnrollmentID, String generationMode)
        {
            Handler handler = new Handler();
            try
            {
                while (true)
                {
                    switch (generationMode)
                    {
                        case Constants.GeneratedTypeCertificateOrKey.CERTIFICATE:
                            handler.CertificateGenerator(long.Parse(serverGeneratedEnrollmentID));
                            break;
                        case Constants.GeneratedTypeCertificateOrKey.KEY:
                            handler.KeyGenerator(long.Parse(serverGeneratedEnrollmentID));
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error : " + ex);
            }

        }
    }
}
