using HouseFlowPart1.Interfaces;
using HouseFlowPart1.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace HouseFlowPart1.Services
{
    public class AuthenticationService: IAuthenticationService
    {
        private readonly IMongoCollection<Users> _userCollection;

        private readonly MongoDBContext db;


        public AuthenticationService(MongoDBContext db)
        {
            this.db = db;
            _userCollection = this.db.GetCollection<Users>("user");
        }

        public void SeedData()
        {
            var Users = _userCollection.Find(_ => true);
            if (Users.Any()) return;

            var user = new Users
            {
                Id = ObjectId.Parse("651464f29e9afbfdf56a555d"),
                Email = "test@gmail.com",
                Password = "123456",
                FirstName = "narges",
                LastName = "rezaei"
            };

            _userCollection.InsertOne(user);
        }

        public async Task Register(string email, string password, string firstName, string lastName)
        {
            // Check if the user with the given email already exists
            var user = await _userCollection.FindAsync(u => u.Email == email);
            if (await user.AnyAsync())
            {
                throw new Exception("User with the same email already exists.");
            }

            Users newUser = new()
            {
                Email = email,
                Password = password,
                FirstName = firstName,
                LastName = lastName
            };

            _userCollection.InsertOne(newUser);
        }

        public async Task<Boolean> Login(string email, string password)
        {
            // Find the user with the given email
            var user = await _userCollection.FindAsync(u => u.Email == email);

            if (user == null || user.FirstOrDefault().Password != password)
            {
                return false;
            }

            return true;
        }
        public async Task<Users>  GetCurrentUserByUsername(string? username)
        {
            // Find the user with the given username
            var user = await _userCollection.FindAsync(u => u.Email == username);

            if (user == null)
            {
                throw new Exception("User not found.");
            }

            return await user.FirstOrDefaultAsync();
        }
    }
}
