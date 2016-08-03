using System.Collections.Generic;

namespace InvoiceCqrs.Persistence.EventStore.Util
{
    public class ObjectToPropertyDictionaryMapper
    {
        public IDictionary<string, string> Map(object obj)
        {
            var dict = new Dictionary<string, string>();
            var props = obj.GetType().GetProperties();

            foreach (var prop in props)
            {
                var key = prop.Name;
                var value = prop.GetValue(obj).ToString();
                dict[key] = value;
            }

            return dict;
        }
    }
}