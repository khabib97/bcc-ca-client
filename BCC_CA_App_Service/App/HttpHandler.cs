using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unosquare.Labs.EmbedIO;
using Unosquare.Labs.EmbedIO.Modules;

namespace BCC_CA_App_Service.App
{
    class HttpHandler
    {
        public static void WebServerInit()
        {
            var url = "http://127.0.0.1:8090/";


            // Our web server is disposable.
            using (var server = new WebServer(url))
            {
                // First, we will configure our web server by adding Modules.
                // Please note that order DOES matter.
                // ================================================================================================
                // If we want to enable sessions, we simply register the LocalSessionModule
                // Beware that this is an in-memory session storage mechanism so, avoid storing very large objects.
                // You can use the server.GetSession() method to get the SessionInfo object and manupulate it.
                // You could potentially implement a distributed session module using something like Redis
                server.WithLocalSession();

                // Here we setup serving of static files
                //server.RegisterModule(new StaticFilesModule("c:/web"));
                // The static files module will cache small files in ram until it detects they have been modified.
                //server.Module<StaticFilesModule>().UseRamCache = true;
                server.RegisterModule(new WebApiModule());
                server.Module<WebApiModule>().RegisterController<HttpController>();
                // Once we've registered our modules and configured them, we call the RunAsync() method.
                server.RunAsync();

                // Fire up the browser to show the content if we are debugging!
                // Wait for any key to be pressed before disposing of our web server.
                // In a service, we'd manage the lifecycle of our web server using
                // something like a BackgroundWorker or a ManualResetEvent.
                Console.ReadKey(true);
            }
        }
    }
}
