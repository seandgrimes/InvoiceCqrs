using System.Collections.Generic;
using InvoiceCqrs.Messages.Shared;

namespace InvoiceCqrs.Extensions
{
    public static class SearchOptionsExtensions
    {
        public static string ToSqlOperator(this SearchOptions searchOption)
        {
            var operators = new Dictionary<SearchOptions, string>
            {
                {SearchOptions.MatchAll, "AND"},
                {SearchOptions.MatchAny, "OR"}
            };

            return operators[searchOption];
        }
    }
}
