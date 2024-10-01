namespace RentHall.Application.AbstractionsServices
{
    public interface IPasswordHash
    {
        public string Generate(string password);

        public bool Verify(string password, string hashPassword);
    }
}
