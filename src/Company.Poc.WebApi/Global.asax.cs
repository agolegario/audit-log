using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
using Company.Poc.WebApi.Handlers;
using Company.Poc.WebApi.Mocks;

namespace Company.Poc.WebApi
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            GlobalConfiguration.Configuration.MessageHandlers.Add(new LogHandler());
            ProductMock.BindProducts();
        }
    }
}
