using System;
using BCC_CA_App_Service.App;
using System.IO;

namespace BCC_CA_App_Service
{
    class Program
    {
        static String deploymentMode = "Dev"; // Prod, Dev, Test
        public static String generationMode; // pub-priv key or certificate 
        public static String serverGeneratedEnrollmentID;
        public static String baseRemoteURL;
        public static int keyStoreType;

        static void Main(string[] args){

            Console.WriteLine("BCC-CA Background Service");
            Handler handler = new Handler();
            NetworkHandler networkHandler = new NetworkHandler();
            Pkcs1xHandler pkcs1xHandler = new Pkcs1xHandler();
            InputHandler inputHandler = new InputHandler();

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
