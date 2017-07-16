using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using System.Web.Http;
using Unity.WebApi;

namespace MB.TestTask.WebAPI
{
    public static class UnityConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var container = new UnityContainer();
            container.LoadConfiguration();

            // TODO: insert here interceptors

            config.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}