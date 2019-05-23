using Newtonsoft.Json;
using System;

namespace BCC_CA_App_Service.App
{
    class HttpServerHandler
    {
        public static void WebServerInit()
        {
            HttpServer ws = new HttpServer(SendResponse, "http://127.0.0.1:8087/test/");
            ws.Run();
            Console.WriteLine("A simple webserver. Press a key to quit.");
            Console.ReadKey();
            ws.Stop();
        }

        private static void DataArrayToPinPassPhase(string[] dataArray, out string pin, out string passphase, int storeType)
        {
            passphase = dataArray[0].Trim();
            pin = "";
            if (storeType == 2)
            {
                pin = dataArray[1].Trim();
            }
        }

        private static void Reset(string pin, string passphase)
        {
            pin = "";
            passphase = "";
        }

        public static string SendResponse(System.Net.HttpListenerRequest request)
        {
            string json;
            using (System.IO.StreamReader streamReader = new System.IO.StreamReader(request.InputStream))
            {
                string passphase = "";
                string pin = "";
                Response response = new Response("default", "GET", 200, "default echo msg");
                try
                {
                    string jsonString = streamReader.ReadToEnd();
                    Request requestObj = JsonConvert.DeserializeObject<Request>(jsonString);
                    string actionSwitch = requestObj.action;

                    switch (actionSwitch)
                    {
                        case "welcome":
                            response = new Response("default", "GET", 200, "connection establish");
                            break;
                        case "key":

                            EnrollementDTO enrollmentDTO = requestObj.data.ToObject<EnrollementDTO>();
                            //System.Diagnostics.Debug.WriteLine(enrollmentDTO.ToString());
                            DataArrayToPinPassPhase(requestObj.msg.Split(' '), out pin, out passphase, enrollmentDTO.keyStoreType);

                            Program.InvokeKeyPrograme(enrollmentDTO, pin, passphase, out response);
                            Reset(pin, passphase);
                            break;
                        case "certificate":
                            enrollmentDTO = requestObj.data.ToObject<EnrollementDTO>();
                            //System.Diagnostics.Debug.WriteLine(requestObj.msg);
                            DataArrayToPinPassPhase(requestObj.method.Split(' '), out pin, out passphase, enrollmentDTO.keyStoreType);

                            if (enrollmentDTO.passPhase.Equals(Utility.SHA256(passphase)))
                            {
                                Program.InvokeCertificatePrograme(pin, enrollmentDTO.keyStoreType, enrollmentDTO.ID, requestObj.msg, out response);
                            }
                            else
                            {
                                System.Diagnostics.Debug.WriteLine("Error : " + "Passphase mismatch");
                                response = new Response("certificate", "GET", 0, "Error " + "Passphase mismatch");

                            }
                            Reset(pin, passphase);
                            break;
                        case "default":
                            System.Diagnostics.Debug.WriteLine(requestObj.msg);
                            break;
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex);
                    response = new Response("error", "GET", 0, "Server has faced some problem");
                }
                finally
                {
                    json = JsonConvert.SerializeObject(response);

                }
                return json;
            }
        }
    }
}
