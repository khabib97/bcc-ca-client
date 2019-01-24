using System;
using BCC_CA_App_Service.App;
using System.IO;

namespace BCC_CA_App_Service
{
    class Program
    {
        static String deploymentMode = "Dev"; // Prod, Dev, Test
        public static String generationMode; // pub-priv key or certificate 
        public static int serverGeneratedEnrollmentID;
        public static String baseRemoteURL;
        public static int keyStoreType;

        static void Main(string[] args){

            Console.WriteLine("BCC-CA Background Service");
            Handler handler = new Handler();
            NetworkHandler networkHandler = new NetworkHandler();
            Pkcs1xHandler pkcs1xHandler = new Pkcs1xHandler();

            try
            {
                if (deploymentMode.Equals("Prod")){
                    if (args.Length.Equals(0) || !args.Length.Equals(4))
                    {
                        throw new System.ArgumentException("Invalid number of arguments");
                    }

                    generationMode = args[0];
                    serverGeneratedEnrollmentID = Int32.Parse(args[1]);
                    baseRemoteURL = args[2];
                    keyStoreType = Int32.Parse(args[3]);

                    Console.WriteLine("1 :" + args[0] + " 2 :" + args[1] + " 3 :" + args[2] + " 4 :" + args[3]);
                } else if (deploymentMode.Equals("Dev")){
                    generationMode = "key";
                    serverGeneratedEnrollmentID = 26012;
                    baseRemoteURL = "bcc-ca.gov.bd";
                    keyStoreType = 2;
                    Constants.ENROLLMENT_ID = serverGeneratedEnrollmentID;
                    Console.WriteLine("1 :" + generationMode  + " 2 :"+ serverGeneratedEnrollmentID + " 3 :"+ baseRemoteURL + " 4 :"+ keyStoreType);
                }

                handler.Generator(baseRemoteURL, Constants.PartialUrlOfApi.ENROLLMENT_INFO, serverGeneratedEnrollmentID, generationMode);
                //String cert = networkHandler.GetCertificateByteArray(26007, "bcc-ca.gov.bd");
                //pki.x509certificate = 
                //pkcs1xHandler.GenerateCertificate(cert);


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
