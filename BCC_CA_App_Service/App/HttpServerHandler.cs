using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        /* 
         * Access-Control-Allow-Origin: http://api.bob.com
Access-Control-Allow-Credentials: true
Access-Control-Expose-Headers: FooBar
*/
        public static string SendResponse(System.Net.HttpListenerRequest request)
        {
            
            string name = request.QueryString.Get("data");
            switch (name)
            {
                case "a":
                    Console.WriteLine("aa");
                    return "aa";
                    //break;
                case "b":
                    Console.WriteLine("bb");
                    return "bb";
                    //break;
                default:
                    Console.WriteLine("cc");
                    return "cc";
                    //break;
            }
           // return string.Format("<HTML><BODY>My web page.<br>{0}</BODY></HTML>", DateTime.Now);
        }
    }
}
