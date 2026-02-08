namespace TodoListAPI.Security
{
    public class BcryptPasswordHasher : IPasswordHasher
    {
        private readonly int _workFactor;
        public BcryptPasswordHasher(int workFactor = 12) => _workFactor = workFactor;

        public string Hash(string password) => BCrypt.Net.BCrypt.HashPassword(password, _workFactor);

        public bool Verify(string hashedPassword, string providedPassword) =>
            BCrypt.Net.BCrypt.Verify(providedPassword, hashedPassword);
    }

}
