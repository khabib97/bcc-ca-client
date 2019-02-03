using System;
using BCC_CA_App_Service.App;

namespace BCC_CA_App_Service
{
    class Program
    {
        public static String generationMode; // pub-priv key or certificate 
        public static String serverGeneratedEnrollmentID;
        public static int keystore;
        
        static void Main(string[] args){

            Console.WriteLine("BCC-CA Background Service");
            Handler handler = new Handler();

            if (Environment.Is64BitProcess)
                Constants.PKCS11_LIBRARY_PATH = @"x64\eTPKCS11.dll";
            else
                Constants.PKCS11_LIBRARY_PATH = @"x32\eTPKCS11.dll";

            try
            {
                while (true)
                {
                    serverGeneratedEnrollmentID = InputHandler.GetEnrollmentID();
                    generationMode = InputHandler.GetGenerationMode();
                    switch (generationMode)
                    {
                        case Constants.GeneratedTypeCertificateOrKey.CERTIFICATE:
                            keystore =int.Parse(InputHandler.GetKeystoreType())  ;
                            handler.CertificateGenerator(long.Parse(serverGeneratedEnrollmentID), keystore);
                            break;
                        case Constants.GeneratedTypeCertificateOrKey.KEY:
                            handler.KeyGenerator(long.Parse(serverGeneratedEnrollmentID));
                            break;
                    }
                }
            }
            catch (Exception ex){
                System.Diagnostics.Debug.WriteLine( "Error : " + ex );
            }

            Console.Write("Press any key to quite: ");
            Console.ReadKey();
        }
    }
}
