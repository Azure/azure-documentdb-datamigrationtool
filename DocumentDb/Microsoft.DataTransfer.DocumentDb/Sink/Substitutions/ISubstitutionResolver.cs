using System.Collections.Generic;

namespace Microsoft.DataTransfer.DocumentDb.Sink.Substitutions
{
    interface ISubstitutionResolver
    {
        IEnumerable<string> Resolve(string pattern);
    }
}
