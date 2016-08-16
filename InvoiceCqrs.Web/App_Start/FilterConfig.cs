using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InvoiceCqrs.Web.Actions;
using InvoiceCqrs.Web.DependencyResolution;

namespace InvoiceCqrs.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());

            var existing = FilterProviders.Providers.FirstOrDefault(f => f is FilterAttributeFilterProvider);
            FilterProviders.Providers.Remove(existing);
            FilterProviders.Providers.Add(new StructureMapFilterProvider(IoC.Current));
        }
    }
}
