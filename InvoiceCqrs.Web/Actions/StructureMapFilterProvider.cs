using System.Collections.Generic;
using System.Web.Mvc;
using StructureMap;

namespace InvoiceCqrs.Web.Actions
{
    // There a better way to do this nowadays?
    public class StructureMapFilterProvider : FilterAttributeFilterProvider
    {
        private readonly IContainer _Container;

        public StructureMapFilterProvider(IContainer container)
        {
            _Container = container;
        }

        public override IEnumerable<Filter> GetFilters(ControllerContext controllerContext, ActionDescriptor actionDescriptor)
        {
            var filters = base.GetFilters(controllerContext, actionDescriptor);

            foreach (var filter in filters)
            {
                _Container.BuildUp(filter.Instance);
            }

            return filters;
        }
    }
}