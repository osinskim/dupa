using System.Collections.Generic;
using System.Text.RegularExpressions;
using Smalec.Service.UserManagement.Enums;
using Smalec.Lib.Shared.Abstraction.Interfaces;

namespace Smalec.Service.UserManagement.Validators
{
    public class PasswordValidator : IValidator
    {
        private readonly string _password;

        public PasswordValidator(string password)
        {
            _password = password;
        }
        
        public IEnumerable<string> Validate()
        {
            var hasOnlyAlphanumericCharacters = new Regex("^[a-zA-Z0-9]*$");
            var hasNumbers = new Regex("[0-9]");
            var hasLowerCase = new Regex("[a-z]");
            var hasUpperCase = new Regex("[A-Z]");

            if(_password.Length < 10)
                yield return ErrorCodes.PASSWORD_SHOULD_HAVE_AT_LEAST_10_DIGITS.ToString();
            
            if(hasOnlyAlphanumericCharacters.IsMatch(_password))
                yield return ErrorCodes.PASSWORD_SHOULD_HAVE_SPECIAL_CHAR.ToString();
            
            if(!hasNumbers.IsMatch(_password))
                yield return ErrorCodes.PASSWORD_SHOULD_HAVE_NUMBER.ToString();
            
            if(!hasLowerCase.IsMatch(_password))
                yield return ErrorCodes.PASSWORD_SHOULD_HAVE_LOWERCASE_CHAR.ToString();

            if(!hasUpperCase.IsMatch(_password))
                yield return ErrorCodes.PASSWORD_SHOULD_HAVE_UPPERCASE_CHAR.ToString();
        }
    }
}