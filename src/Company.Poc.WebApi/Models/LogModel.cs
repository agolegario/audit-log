using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using System.Web.Http;

namespace Company.Poc.WebApi.Models
{
    public class LogModel
    {
        public Guid LogId { get; set; }            
        public string Application { get; set; }           
        public string User { get; set; }                 
        public string Machine { get; set; }           
        public string RequestIpAddress { get; set; }     
        public dynamic RequestContentBody { get; set; }   
        public string RequestUri { get; set; }     
        public string RequestMethod { get; set; }    
        public int? ResponseStatusCode { get; set; }  
        public IDictionary<string, string> RequestHeaders { get; set; } 
        public IDictionary<string, string> ResponseHeaders { get; set; } 
        public dynamic ResponseContentBody { get; set; }    
        public DateTime? RequestTimestamp { get; set; }
        public DateTime? ResponseTimestamp { get; set; }

        public string TotalTime
        {
            get
            {
                if (ResponseTimestamp == null || RequestTimestamp == null) return null;
                var time = ResponseTimestamp.Value.Subtract(RequestTimestamp.Value);
                return $"{time.TotalHours:00}:{time.TotalMinutes:00}:{time.TotalSeconds:00}.{time.TotalMilliseconds/10:000}";
              

            }
            set => throw new NotImplementedException();
        }

        public HttpError HttpError { get; set; }

    }
}