using System.Collections.Concurrent;
using DSFactory.Core;

namespace DSFactory.Api.Services
{
    public class DataStore
    {
        public ConcurrentDictionary<string, IDataStructure<string>> Structures { get; } 
            = new ConcurrentDictionary<string, IDataStructure<string>>();
    }
}