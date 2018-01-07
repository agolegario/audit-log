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
            var logModel = CreateLog(request);
            if (request.Content != null)
            {
                await request.Content.ReadAsStringAsync()
                    .ContinueWith(task =>
                    {
                        logModel.RequestContentBody = JsonConvert.DeserializeObject(task.Result);
                    }, cancellationToken);
            }

            return await base.SendAsync(request, cancellationToken)
                .ContinueWith(task =>
                {
                    var response = task.Result;
                    
                    logModel.ResponseStatusCode = (int)response.StatusCode;
                    if (logModel.ResponseStatusCode >= 500)
                    {
                        logModel.HttpError = response.Content.ReadAsAsync<HttpError>(cancellationToken).Result;
                    }
                    logModel.ResponseTimestamp = DateTime.Now;

                    if (response.Content != null)
                    {
                        logModel.LogId = request.GetCorrelationId();
                        logModel.ResponseContentBody = JsonConvert.DeserializeObject(response.Content.ReadAsStringAsync().Result);
                        logModel.ResponseHeaders = GetHeaders(response.Content.Headers);
                    }
                    
                    // TODO: ADD DATABASE OR WRITE IN FILE
                    Debug.Write(JsonConvert.SerializeObject(logModel, Formatting.Indented));

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
                RequestHeaders = GetHeaders(request.Headers),
                RequestTimestamp = DateTime.Now,
                RequestUri = request.RequestUri.ToString()              
            };
        }

        private static IDictionary<string, string> GetHeaders(HttpHeaders headers)
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