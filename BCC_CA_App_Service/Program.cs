using System;
using System.IO;
using BCC_CA_App_Service.App;
using SuperWebSocket;

namespace BCC_CA_App_Service
{
    class Program
    {
        public static int keystore;

        static void Main(string[] args)
        {

            Console.WriteLine("Start:");
            WebSocketHandler.WebServerInit();
            Console.ReadKey();
        }

        internal static void InvokePrograme(string generationMode, string enrollmentID, string storeType, WebSocketSession webSocketSession)
        {
            Handler handler = new Handler();
            Boolean canMoveNext = true;
            if (storeType.Equals(Constants.KeyStore.SMART_CARD)) SetUpSmartCard(canMoveNext);

            if (canMoveNext)
            {
                try
                {
                    switch (generationMode)
                    {
                        case Constants.GeneratedTypeCertificateOrKey.CERTIFICATE:
                            keystore = int.Parse(storeType);
                            handler.CertificateGenerator(long.Parse(enrollmentID), keystore);
                            Constants.GLOBAL_RESPONSE_MSG = "Certificate Generated Successfully";
                            break;
                        case Constants.GeneratedTypeCertificateOrKey.KEY:
                            handler.KeyGeneratorCaller(long.Parse(enrollmentID));
                            Constants.GLOBAL_RESPONSE_MSG = "Key Generated Successfully";
                            break;
                    }

                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Error : " + ex);
                    //MessagePrompt.ShowDialog(ex.Message,"Internal Error!");
                    Constants.GLOBAL_RESPONSE_MSG = ex.Message;
                }
                finally {
                    webSocketSession.Send(Constants.GLOBAL_RESPONSE_MSG);
                }
            }
        }

        private static void SetUpSmartCard(Boolean canMoveNext)
        {
            if (Environment.Is64BitProcess)
                Constants.PKCS11_LIBRARY_PATH = @"x64\eTPKCS11.dll";
            else
                Constants.PKCS11_LIBRARY_PATH = @"x32\eTPKCS11.dll";

            try
            {
                File.Exists(Constants.PKCS11_LIBRARY_PATH);
            }
            catch (Exception ex)
            {
                canMoveNext = false;
                System.Diagnostics.Debug.WriteLine("DLL Not Found : " + ex);
                MessagePrompt.ShowDialog("Pkcs11.dll Not Found! Update Your App", "Error!");
            }

        }
    }
}
