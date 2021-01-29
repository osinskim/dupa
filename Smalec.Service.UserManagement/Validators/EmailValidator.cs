using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using Smalec.Service.UserManagement.Enums;
using Smalec.Lib.Shared.Abstraction.Interfaces;

namespace Smalec.Service.UserManagement.Validators
{
    public class EmailValidator : IValidator
    {
        private readonly string _email;

        public EmailValidator(string email)
        {
            _email = email;
        }

        public IEnumerable<string> Validate()
        {
            try
            {
                MailAddress m = new MailAddress(_email);
                return Enumerable.Empty<string>();
            }
            catch (FormatException)
            {
                return new [] { ErrorCodes.INCORRECT_EMAIL_FORMAT.ToString() };
            }
        }
    }
}