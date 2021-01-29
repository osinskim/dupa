using System.Collections.Generic;

namespace Smalec.Lib.Shared.Abstraction.Interfaces
{
    public interface IValidator
    {
        IEnumerable<string> Validate();
    }
}