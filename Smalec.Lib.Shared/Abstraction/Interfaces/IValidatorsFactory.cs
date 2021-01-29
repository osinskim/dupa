using Smalec.Lib.Shared.Helpers;

namespace Smalec.Lib.Shared.Abstraction.Interfaces
{
    public interface IValidatorsFactory
    {
        Validator GetValidator<T>(T model) where T : class;
    }
}