// Copyright (c) 2016 William Arthur Hood
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights to
// use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies
// of the Software, and to permit persons to whom the Software is furnished
// to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included
// in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Rockabilly.Common
{
	public abstract class HttpServer
	{
		private int port = 80;
		private HttpListener listener = new HttpListener();
		public abstract string Handle(HttpListenerRequest httpRequest);
		private Thread executionThread = null;
		public bool ContinueService = false;


		public virtual void SetupService(int portToListenOn = 80)
		{
			port = portToListenOn;
			listener.Prefixes.Add(String.Format("http://*:{0}/", port));

			listener.Start();
			ContinueService = true;
			executionThread = new Thread(Run);
			executionThread.Start();
		}


		public virtual void DiscontinueService()
		{
			Console.WriteLine("HTTP SERVER DISCONTINUING SERVICE");
			ContinueService = false;

			try
			{
				executionThread.Abort();
				executionThread.Abort();
				executionThread.Abort();
			}
			catch { }
			finally
			{
				executionThread = null;
			}
		}



		public string DescribeService()
		{
			return "Listening for connections on port " + port;
		}

		private void Run()
		{
			HttpListenerContext context = null;
			Byte[] outgoingResponse = null;
			try
			{
				while (ContinueService)
				{
					try
					{
						context = listener.GetContext();
						outgoingResponse = System.Text.Encoding.UTF8.GetBytes(Handle(context.Request));
						context.Response.ContentLength64 = outgoingResponse.Length;
						context.Response.OutputStream.Write(outgoingResponse, 0, outgoingResponse.Length);
					}
					finally
					{
						context.Response.OutputStream.Close();
					}
				}
			}
			finally
			{
				listener.Stop();
				listener.Prefixes.Clear();
				context = null;
				outgoingResponse = null;
			}
		}
	}
}
