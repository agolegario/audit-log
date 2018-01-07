using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Company.Poc.WebApi.Models;
using Newtonsoft.Json;

namespace Company.Poc.WebApi.Handlers
{
    public class LogHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var apiLogEntry = CreateLog(request);
            if (request.Content != null)
            {
                await request.Content.ReadAsStringAsync()
                    .ContinueWith(task =>
                    {
                        apiLogEntry.RequestContentBody = JsonConvert.DeserializeObject(task.Result);
                    }, cancellationToken);
            }

            return await base.SendAsync(request, cancellationToken)
                .ContinueWith(task =>
                {
                    var response = task.Result;
                    
                    apiLogEntry.ResponseStatusCode = (int)response.StatusCode;
                    if (apiLogEntry.ResponseStatusCode >= 500)
                    {
                        apiLogEntry.HttpError = response.Content.ReadAsAsync<HttpError>(cancellationToken).Result;
                    }
                    apiLogEntry.ResponseTimestamp = DateTime.Now;

                    if (response.Content != null)
                    {
                        apiLogEntry.LogId = request.GetCorrelationId();
                        apiLogEntry.ResponseContentBody = JsonConvert.DeserializeObject(response.Content.ReadAsStringAsync().Result);
                        apiLogEntry.ResponseHeaders = SerializeHeaders(response.Content.Headers);
                    }
                    
                    // TODO: ADD DATABASE OR WRITE IN FILE
                    Debug.Write(JsonConvert.SerializeObject(apiLogEntry, Formatting.Indented));

                    return response;
                }, cancellationToken);
        }

        private LogModel CreateLog(HttpRequestMessage request)
        {
            var context = ((HttpContextBase)request.Properties["MS_HttpContext"]);
           
            return new LogModel
            {
                Application = "COMPANY-POC-WEBAPI",
                User = context.User.Identity.Name,
                Machine = Environment.MachineName,
                RequestIpAddress = context.Request.UserHostAddress,
                RequestMethod = request.Method.Method,
                RequestHeaders = SerializeHeaders(request.Headers),
                RequestTimestamp = DateTime.Now,
                RequestUri = request.RequestUri.ToString()              
            };
        }

        private static IDictionary<string, string> SerializeHeaders(HttpHeaders headers)
        {
            var retorno = new Dictionary<string, string>();

            foreach (var item in headers.ToList())
            {
                if (item.Value == null) continue;
                var header = item.Value.Aggregate(string.Empty, (current, value) => current + (value + " "));

                header = header.TrimEnd(" ".ToCharArray());
                retorno.Add(item.Key, header);
            }
            return retorno;
        }
    }
}