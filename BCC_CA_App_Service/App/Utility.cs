using System;
using System.Text;
using System.Security.Cryptography;
using Newtonsoft.Json.Linq;

namespace BCC_CA_App_Service.App
{
    public class Utility{

        public static String SHA1(String data) {

            byte[] shaBytes;
            SHA1 sha1 = new SHA1CryptoServiceProvider();

            shaBytes = sha1.ComputeHash(Encoding.UTF8.GetBytes(data));

            StringBuilder stringBuilder = new StringBuilder();

            foreach (byte shaByte in shaBytes) {
                var hex = shaByte.ToString("x2");
                stringBuilder.Append(hex);
            }
            return stringBuilder.ToString();
        }

        public static String SHA256(String data)
        {

            byte[] shaBytes;
            SHA256 sha256 = new SHA256CryptoServiceProvider();

            shaBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(data));

            StringBuilder stringBuilder = new StringBuilder();

            foreach (byte shaByte in shaBytes)
            {
                var hex = shaByte.ToString("x2");
                stringBuilder.Append(hex);
            }
            return stringBuilder.ToString();
        }

        public static void DataArrayToPinPassPhase(string[] dataArray, out string pin, out string passphase, int storeType)
        {
            passphase = dataArray[0].Trim();
            pin = "";
            if (storeType == 2)
            {
                pin = dataArray[1].Trim();
            }
        }

        public static void Reset(string pin, string passphase)
        {
            pin = "";
            passphase = "";
        }


        public class Request
        {
            public string action { get; set; }
            public string method { get; set; }
            public string msg { get; set; }
            public JObject data { get; set; }
        }

        public class Response
        {
            public Response(string action, string method, int status, string msg)
            {
                this.action = action;
                this.method = method;
                this.status = status;
                this.msg = msg;
            }
            public int status { get; set; }
            public string action { get; set; }
            public string method { get; set; }
            public string msg { get; set; }

        }

    }
}
