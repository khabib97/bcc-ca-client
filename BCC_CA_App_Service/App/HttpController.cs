using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCC_CA_App_Service.App
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using System.Threading.Tasks;
    using Unosquare.Labs.EmbedIO;
    using Unosquare.Labs.EmbedIO.Modules;
#if NET47
    using System.Net;
#else
    using Unosquare.Net;
    using static Utility;
#endif

    /// <summary>
    /// A very simple controller to handle generate keys and install certificate
    /// Notice how it Inherits from WebApiController and the methods have WebApiHandler attributes 
    /// This is for sampling purposes only.
    /// </summary>
    public class HttpController : WebApiController
    {
        [WebApiHandler(HttpVerbs.Get, "/api/key/*")]
        public bool GetPeople(WebServer server, HttpListenerContext context)
        {
            //context.ur
            //var data = context.RequestFormDataDictionary();
           
            //var model = context.ParseJson();
            try
            {
                var lastSegment = context.Request.Url.Segments.Last();
                Request requestObj = JsonConvert.DeserializeObject<Request>(lastSegment);
                EnrollementDTO enrollmentDTO = requestObj.data.ToObject<EnrollementDTO>();
                //System.Diagnostics.Debug.WriteLine(enrollmentDTO.ToString());
                string passphase = "";
                string pin = "";
                Utility.DataArrayToPinPassPhase(requestObj.msg.Split(' '), out pin, out passphase, enrollmentDTO.keyStoreType);

                Response response= Program.InvokeKeyPrograme(enrollmentDTO, pin, passphase);
                Utility.Reset(pin, passphase);
                string jsonResponse = JsonConvert.SerializeObject(response);
                return context.JsonResponse(jsonResponse);
            }
            catch (Exception ex)
            {
                // here the error handler will respond with a generic 500 HTTP code a JSON-encoded object
                // with error info. You will need to handle HTTP status codes correctly depending on the situation.
                // For example, for keys that are not found, ou will need to respond with a 404 status code.
                return HandleError(context, ex);
            }
        }

       

        [WebApiHandler(HttpVerbs.Get, "/api/echo")]
        public bool GetEcho(WebServer server, HttpListenerContext context)
        {
            try
            {
                Console.WriteLine("Echo");
                return context.JsonResponse("Echo");
            }
            catch (Exception ex)
            {
                // here the error handler will respond with a generic 500 HTTP code a JSON-encoded object
                // with error info. You will need to handle HTTP status codes correctly depending on the situation.
                // For example, for keys that are not found, ou will need to respond with a 404 status code.
                return HandleError(context, ex);
            }
        }

        // private readonly AppDbContext _dbContext = new AppDbContext();
        private const string RelativePath = "/api/";

       
     
        /// <summary>
        /// Handles the error returning an error status code and json-encoded body.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="ex">The ex.</param>
        /// <param name="statusCode">The HTTP status code.</param>
        /// <returns></returns>
        protected bool HandleError(HttpListenerContext context, Exception ex, int statusCode = 500)
        {           
            Response response = new Response("error", "GET", statusCode, ex.ExceptionMessage());
            string jsonResponse = JsonConvert.SerializeObject(response);
            return context.JsonResponse(jsonResponse);
        }

    }
}
