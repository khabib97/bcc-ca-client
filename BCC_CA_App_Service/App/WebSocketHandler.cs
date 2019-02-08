using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperWebSocket;

namespace BCC_CA_App_Service.App
{
    class WebSocketHandler
    {
        public static void WebServerInit() {
             WebSocketServer webSocketServer = new WebSocketServer();
             int port = 9991;
             Microsoft.Win32.RegistryKey registryKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            
             webSocketServer.Setup(port);
             webSocketServer.Start();
             webSocketServer.NewSessionConnected += WsServer_NewSessionConnected;
             webSocketServer.NewMessageReceived += WsServer_NewMessageReceived;
        }

        private static void WsServer_NewSessionConnected(WebSocketSession session)
        {
            Console.WriteLine("New Session Connected");
        }

        private static void WsServer_NewMessageReceived(WebSocketSession session, String value)
        {
            String[] words = System.Text.RegularExpressions.Regex.Split(value, @"\W+");
            String actionType = words[0];
            
            
            switch (actionType)
            {
                case "key_generation":
                    String generationMode = words[1];
                    String enrollmentID = words[2];
                    String storeType = words[3];
                    Console.WriteLine("Mode:" + generationMode + " ID:" + enrollmentID + " Store:" + storeType);
                    Program.InvokePrograme(generationMode, enrollmentID, storeType, session);
                    break;
                case "certificate_generation":
                    generationMode = words[1];
                    enrollmentID = words[2];
                    storeType = words[3];
                    Console.WriteLine("Mode:" + generationMode + " ID:" + enrollmentID + " Store:" + storeType);
                    Program.InvokePrograme(generationMode, enrollmentID, storeType, session);
                    break;
                case "passphase":
                    Constants.PASSPHASE = words[1];
                    break;
                case "pin":
                    Constants.PIN = words[1];
                    break;


            }
        }
    }
}
