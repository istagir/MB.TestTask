using Microsoft.Owin;
using Owin;
using System.Web.Http;

[assembly: OwinStartup(typeof(MB.TestTask.WebAPI.Startup))]

namespace MB.TestTask.WebAPI
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            UnityConfig.Register(GlobalConfiguration.Configuration);
            WebApiConfig.Register(GlobalConfiguration.Configuration);

            GlobalConfiguration.Configuration.EnsureInitialized();
        }
    }
}
