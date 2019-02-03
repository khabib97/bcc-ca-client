using System;
using SuperWebSocket;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace BCC_CA_App_Service.App
{
   
    class SocketCom
    {
        public static String generationMode = ""; 
        public static String serverGeneratedEnrollmentID = "";


        public void WsServer_SessionClosed(WebSocketSession session, SuperSocket.SocketBase.CloseReason value)
        {
            Console.WriteLine("Session Closed");
        }

        public void WsServer_NewDataReceived(WebSocketSession session, byte[] value)
        {
            Console.WriteLine("New Data Received");
        }
      
        public void WsServer_NewMessageReceived(WebSocketSession session1, String value)
        {
            string[] words = Regex.Split(value, @"\W+");
            serverGeneratedEnrollmentID = words[0];
           // Console.WriteLine("serverGeneratedEnrollmentID: " + serverGeneratedEnrollmentID);
            generationMode = words[1];
            // Console.WriteLine("generationMode: " + generationMode);
            Program.InvokePrograme(serverGeneratedEnrollmentID,generationMode);
            
        }
        public void WsServer_NewSessionConnected(WebSocketSession session)
        {
            Console.WriteLine("New Session Connected");
        }
    }
}
