namespace WebApp.Common;

public static class ErrorCodes
{
    public static class Auth
    {
        public static Error EmailExists => Error.Conflict("AUTH_001", "Email is already registered.");
        public static Error UsernameExists => Error.Conflict("AUTH_002", "Username is already taken.");
        public static Error InvalidCredentials => Error.Unauthorized("AUTH_003", "Invalid email or password.");
    }

    public static class Products
    {
        public static Error NotFound(int id) => Error.NotFound("PROD_001", $"Product with id {id} not found.");
    }
}
