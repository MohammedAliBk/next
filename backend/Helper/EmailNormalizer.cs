namespace TodoListAPI.Helper
{
    public static class EmailNormalizer
    {
        public static string NormalizeEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return string.Empty;

            var parts = email.Trim().ToLowerInvariant().Split('@');
            if (parts.Length != 2)
                return email.Trim().ToLowerInvariant();

            var local = parts[0].Replace(".", "");
            var domain = parts[1];

            return $"{local}@{domain}";
        }
    }
}
