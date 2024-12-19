namespace API.Modules.Identity.Constants;

internal static class RegexConstant
{
    public static KeyValuePair<string, string> ValidUsername = new(
        @"^[a-z0-9](?!.*[_.]{2})(?!.*[.]$)[a-z0-9.]*$",
        "Username must start and end with a letter or digit, cannot contain consecutive underscores or periods, and can only include letters, digits, underscores, and periods.");

    public static KeyValuePair<string, string> ValidPassword = new(
        @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*])[A-Za-z\d!@#$%^&*]*$",
        "Password must contain at least one uppercase letter, one lowercase letter, one digit, and one special character (!@#$%^&*), and can only include letters, digits, and the specified special characters.");

    public static KeyValuePair<string, string> ValidFullname = new(
        @"^[A-Za-z]+(?:[ '\s][A-Za-z]+)*$",
        "Full name must contain only letters, spaces, and apostrophes, and cannot have double spaces or other symbols.");
}