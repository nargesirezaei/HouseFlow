using HouseFlowPart1.Models;

namespace HouseFlowPart1.Interfaces
{
    public interface IAuthenticationService
    {
        Task<Users> GetCurrentUserByUsername(string username);
        Task<bool> Login(string email, string password);
        Task Register(string email, string password, string firstName, string lastName);
        void SeedData();
    }
}
