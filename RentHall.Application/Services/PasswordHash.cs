using RentHall.Application.AbstractionsServices;

namespace RentHall.Application.Services
{
    public class PasswordHash: IPasswordHash
    {
        public string Generate(string password)
        {
            return BCrypt.Net.BCrypt.EnhancedHashPassword(password);
        }
        public bool Verify(string password, string hashPassword)
        {
            return BCrypt.Net.BCrypt.EnhancedVerify(password, hashPassword);
        }
    }
}
