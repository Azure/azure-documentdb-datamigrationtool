using Microsoft.DataTransfer.WpfHost.Extensibility;
using System;
using System.Collections.Concurrent;

namespace Microsoft.DataTransfer.WpfHost.Model
{
    sealed class ImportSharedStorage : IImportSharedStorage
    {
        private ConcurrentDictionary<object, object> cache;

        public ImportSharedStorage()
        {
            cache = new ConcurrentDictionary<object, object>();
        }

        public T GetOrAdd<T>(object key, Func<object, T> valueFactory)
            where T : class
        {
            return (T)cache.GetOrAdd(key, (Func<object, object>)valueFactory);
        }
    }
}
