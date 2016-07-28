using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceCqrs.Validation
{
    public interface IValidator<in TEntity>
    {
        IList<string> Validate(TEntity entity);
    }
}
