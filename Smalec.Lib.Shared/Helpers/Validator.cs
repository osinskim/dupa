using System.Collections.Generic;
using System.Linq;
using Smalec.Lib.Shared.Abstraction.Interfaces;

namespace Smalec.Lib.Shared.Helpers
{
    public class Validator : IValidator
    {
        private readonly IEnumerable<IValidator> _validators;

        public Validator(IEnumerable<IValidator> validators)
        {
            _validators = validators;
        }

        public IEnumerable<string> Validate()
        {
            return _validators.SelectMany(validator => validator.Validate()).ToList();
        }
    }
}