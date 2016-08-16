using System.Web.Mvc;
using InvoiceCqrs.Persistence;

namespace InvoiceCqrs.Web.Actions
{
    public class UnitOfWorkAttribute : ActionFilterAttribute, IExceptionFilter
    {
        public IUnitOfWork UnitOfWork { get; set; }

        public void OnException(ExceptionContext filterContext)
        {
            UnitOfWork.Rollback();
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (filterContext.Exception == null)
            {
                UnitOfWork.Commit();
            }
        }
    }
}