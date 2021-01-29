using System;
using System.Threading.Tasks;
using Smalec.Lib.Shared.Helpers;

namespace Smalec.Lib.Shared.Abstraction.Interfaces
{
    public interface ICommonRabbitService
    {
        Task PerformActionAfterFileUploaded(SynchronizedRequest actionRequest, Func<object, object, Task> actionAfterAck);
    }
}