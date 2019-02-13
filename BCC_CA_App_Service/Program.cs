using System;
using System.IO;
using BCC_CA_App_Service.App;

namespace BCC_CA_App_Service
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Start:");
                Microsoft.Win32.RegistryKey registryKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
                WebSocketHandler.WebServerInit();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error : " + ex);
                MessagePrompt.ShowDialog(ex.Message, "Internal Application Error!");
            }
            finally
            {
                Console.ReadKey();
            }
        }

        internal static void InvokeKeyPrograme(EnrollementDTO enrollementDTO, out Response response)
        {
            Handler handler = new Handler();
            Boolean canMoveNext = true;
            if (enrollementDTO.keyStoreType.Equals(Constants.KeyStore.SMART_CARD)) SetUpSmartCard(canMoveNext);
            try
            {
                string dotP7bURI = handler.KeyGeneratorCaller(enrollementDTO);
                response = new Response("dotp7b", "GET", 200, dotP7bURI);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error : " + ex);
                response = new Response("error", "GET", 0, ex.Message);
            }
        }

        internal static void InvokeCertificatePrograme(int storeType, long enrollmentID,String dotP7b, out Response response)
        {
            Handler handler = new Handler();
            Boolean canMoveNext = true;
            if (storeType.Equals(Constants.KeyStore.SMART_CARD)) SetUpSmartCard(canMoveNext);
            try
            {
                handler.CertificateGenerator(enrollmentID, storeType, dotP7b);
                response = new Response("certificate", "GET", 200, "Certificate Generated Successfully");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error : " + ex);
                response = new Response("certificate", "GET", 0, ex.Message);
            }
        }

        private static void SetUpSmartCard(Boolean canMoveNext)
        {
            if (Environment.Is64BitProcess)
                Constants.PKCS11_LIBRARY_PATH = @"x64eTPKCS11.dll";
            else
                Constants.PKCS11_LIBRARY_PATH = @"x32eTPKCS11.dll";

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
