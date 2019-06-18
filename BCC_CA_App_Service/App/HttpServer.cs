﻿using System;
using System.Net;
using System.Threading;
using System.Text;

namespace BCC_CA_App_Service.App
{
    public class HttpServer
    {
        private readonly HttpListener _listener = new HttpListener();
        private readonly Func<HttpListenerRequest,HttpListenerResponse, string> _responderMethod;

        public HttpServer(string[] prefixes, Func<HttpListenerRequest,HttpListenerResponse, string> method)
        {
            if (!HttpListener.IsSupported)
                throw new NotSupportedException(
                    "Needs Windows XP SP2, Server 2003 or later.");

            // URI prefixes are required, for example 
            // "http://localhost:8080/index/".
            if (prefixes == null || prefixes.Length == 0)
                throw new ArgumentException("prefixes");

            // A responder method is required
            if (method == null)
                throw new ArgumentException("method");

            foreach (string s in prefixes)
                _listener.Prefixes.Add(s);

            _responderMethod = method;
            _listener.Start();
        }

        public HttpServer(Func<HttpListenerRequest,HttpListenerResponse, string> method, params string[] prefixes) : this(prefixes, method) { }

        public void Run()
        {
            ThreadPool.QueueUserWorkItem((o) =>
            {
                Console.WriteLine("Webserver running...");
                try
                {
                    while (_listener.IsListening)
                    {
                        ThreadPool.QueueUserWorkItem((c) =>
                        {
                            var ctx = c as HttpListenerContext;
                            try
                            {
                                string rstr = _responderMethod(ctx.Request, ctx.Response);
                                byte[] buf = Encoding.UTF8.GetBytes(rstr);

                                ctx.Response.AppendHeader("Access-Control-Allow-Origin", "*");
                                ctx.Response.AppendHeader("Access-Control-Allow-Methods", "*");
                                ctx.Response.AppendHeader("Access-Control-Allow-Credentials", "true");
                                ctx.Response.AppendHeader("Access-Control-Allow-Headers", "Origin, X-Requested-With, Content-Type, Accept");

                                ctx.Response.ContentLength64 = buf.Length;
                                ctx.Response.OutputStream.Write(buf, 0, buf.Length);

                            }
                            catch (Exception ex){
                                Console.WriteLine(ex);
                            } // suppress any exceptions
                            finally
                            {
                                // always close the stream
                                ctx.Response.OutputStream.Close();
                            }
                        }, _listener.GetContext());
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                } // suppress any exceptions
            });
        }

        public void Stop()
        {
            _listener.Stop();
            _listener.Close();
        }
    }
}