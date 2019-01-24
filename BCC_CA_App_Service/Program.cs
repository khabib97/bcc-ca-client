using System;
using BCC_CA_App_Service.App;
using System.IO;
using Net.Pkcs11Interop.HighLevelAPI;

namespace BCC_CA_App_Service
{
    class Program
    {
        public static String generationMode; // pub-priv key or certificate 
        public static String serverGeneratedEnrollmentID;

        static void Main(string[] args){

            Console.WriteLine("BCC-CA Background Service");
            Handler handler = new Handler();
            NetworkHandler networkHandler = new NetworkHandler();
            Pkcs1xHandler pkcs1xHandler = new Pkcs1xHandler();
            InputHandler inputHandler = new InputHandler();
            SmartCardHandler smartCardHandler = new SmartCardHandler();
            Session session = null;
            

            if (Environment.Is64BitProcess)
                Constants.PKCS11_LIBRARY_PATH = @"x64\eTPKCS11.dll";
            else
                Constants.PKCS11_LIBRARY_PATH = @"x32\eTPKCS11.dll";

            smartCardHandler.StartSmartCardSession(out session);

            try
            {
                while (true)
                {
                    serverGeneratedEnrollmentID = inputHandler.GetEnrollmentID();
                    generationMode = inputHandler.GetGenerationMode();
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
            catch (Exception ex){
                System.Diagnostics.Debug.WriteLine( "Error : " + ex );
            }
            finally {

            }

            Console.Write("Press any key to quite: ");
            Console.ReadKey();
        }
    }
}
