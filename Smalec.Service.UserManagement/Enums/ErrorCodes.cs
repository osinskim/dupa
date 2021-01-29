namespace Smalec.Service.UserManagement.Enums
{
    public enum ErrorCodes
    {
        USER_EXISTS,
        PASSWORD_SHOULD_HAVE_AT_LEAST_10_DIGITS,
        PASSWORD_SHOULD_HAVE_SPECIAL_CHAR,
        PASSWORD_SHOULD_HAVE_NUMBER,
        PASSWORD_SHOULD_HAVE_UPPERCASE_CHAR,
        PASSWORD_SHOULD_HAVE_LOWERCASE_CHAR,
        INCORRECT_EMAIL_FORMAT
    }
}