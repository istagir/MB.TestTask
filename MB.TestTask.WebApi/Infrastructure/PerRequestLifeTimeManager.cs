using Microsoft.Practices.Unity;
using System;
using System.Web;

namespace MB.TestTask.WebAPI.Infrastructure
{
    public class PerRequestLifeTimeManager : LifetimeManager
    {
        private string _key = $"PerRequestLifeTimeManager_{Guid.NewGuid()}";

        public override object GetValue()
        {
            return HttpContext.Current.Items[_key];
        }

        public override void RemoveValue()
        {
            HttpContext.Current.Items.Remove(_key);
        }

        public override void SetValue(object newValue)
        {
            HttpContext.Current.Items[_key] = newValue;
        }
    }
}