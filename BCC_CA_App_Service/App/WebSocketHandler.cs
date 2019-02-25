using System;
using SuperWebSocket;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Linq;
//using System.Web.Script.Serisalization;

namespace BCC_CA_App_Service.App
{
    class WebSocketHandler
    {
        public static void WebServerInit()
        {
            try
            {
                WebSocketServer webSocketServer = new WebSocketServer();
                int port = 8421;


                if (!webSocketServer.Setup(port))
                {
                    throw new Exception("Failed To Setup Socket Server");
                }
                if (!webSocketServer.Start())
                {
                    throw new Exception("Failed to Start Socket Server");
                }

                webSocketServer.NewSessionConnected += WsServer_NewSessionConnected;
                webSocketServer.NewMessageReceived += WsServer_NewMessageReceived;
            }
            catch (Exception ex) {
                System.Diagnostics.Debug.WriteLine(ex);
            }
        }

        private static void WsServer_NewSessionConnected(WebSocketSession session)
        {
        }

        private static void WsServer_NewMessageReceived(WebSocketSession session, String _request)
        {
            Response response = new Response("default", "GET", 200, "default echo msg");
            try
            {
                Request requestObj = JsonConvert.DeserializeObject<Request>(_request);
                string actionSwitch = requestObj.action;

                switch (actionSwitch)
                {
                    case "welcome":
                        response = new Response("default", "GET", 200, "connection establish");
                        break;
                    case "key":
                        EnrollementDTO enrollmentDTO = requestObj.data.ToObject<EnrollementDTO>();
                        System.Diagnostics.Debug.WriteLine(enrollmentDTO.ToString());
                        
                        String[] dataArray = requestObj.msg.Split(' ');
                        string passphase = dataArray[0].Trim();
                        string pin = "";
                        if (enrollmentDTO.keyStoreType == 2) {
                            pin = dataArray[1].Trim();
                        }
                        Program.InvokeKeyPrograme(enrollmentDTO,pin,passphase,out response);
                        break;
                    case "certificate":
                        enrollmentDTO = requestObj.data.ToObject<EnrollementDTO>();
                        //System.Diagnostics.Debug.WriteLine(requestObj.msg);
                        dataArray = requestObj.method.Split(' ');
                        passphase = dataArray[0].Trim();
                        pin = "";
                        if (enrollmentDTO.keyStoreType == 2) pin = dataArray[1].Trim();

                        if (enrollmentDTO.passPhase.Equals(passphase))
                        {
                            Program.InvokeCertificatePrograme(pin, enrollmentDTO.keyStoreType, enrollmentDTO.ID, requestObj.msg, out response);
                        }
                        else {
                            System.Diagnostics.Debug.WriteLine("Error : " + "Passphase mismatch");
                            response = new Response("certificate", "GET", 0, "Error " + "Passphase mismatch");
                            
                        }
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
            finally {
                string json = JsonConvert.SerializeObject(response);
                session.Send(json);
            }
        }
    }

    public class Request
    {
        public string action { get; set; }
        public string method { get; set; }
        public string msg { get; set; }
        public JObject data { get; set; }
    }

    public class Response {
        public Response(string action, string method, int status, string msg) {
            this.action = action;
            this.method = method;
            this.status = status;
            this.msg = msg;
        }
        public int status { get; set; }
        public string action { get; set; }
        public string method { get; set;}
        public string msg { get; set; }
        
    }
}
