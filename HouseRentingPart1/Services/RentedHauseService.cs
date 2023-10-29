using HouseFlowPart1.Interfaces;
using HouseFlowPart1.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace HouseFlowPart1.Services
{
    public class RentedHauseService : IRentedHauseService
    {
        private readonly IMongoCollection<RentedHauses> _rentedHouseCollection;


        private readonly MongoDBContext _db;

        public RentedHauseService(MongoDBContext db)
        {
            _db = db;
            _rentedHouseCollection = _db.GetCollection<RentedHauses>("rentedHouses");
        }
        // Add a rented house asynchronously
        public async Task<bool> AddRentedHause(RentedHauses hause)
        {
            try
            {
                await _rentedHouseCollection.InsertOneAsync(hause);
                return true;
            }
            catch
            {
                return false;
            }
        }
        // Delete a rented house asynchronously by its ID
        public async Task<bool> DeleteRentedHause(ObjectId id)
        {
            try
            {
                var filter = Builders<RentedHauses>.Filter.Eq("_id", id);
                var result = await _rentedHouseCollection.DeleteOneAsync(filter);
                return result.DeletedCount > 0;
            }
            catch
            {
                return false;
            }
        }
        // Get a rented house by its ID asynchronously
        public async Task<RentedHauses> GetRentedHause(ObjectId Id)
        {
            try
            {
                var filter = Builders<RentedHauses>.Filter.Eq("_id", Id);
                var house = await _rentedHouseCollection.Find(filter).FirstOrDefaultAsync();
                return house;
            }
            catch
            {
                throw new Exception("Id not found");
            }
        }
        // Get a rented house by HauseId and UserId asynchronously
        public async Task<RentedHauses> GetRentedHauseByHauseIdAndUserId(ObjectId hauseId, ObjectId userId)
        {
            try
            {
                var filter = Builders<RentedHauses>.Filter.And(
                    Builders<RentedHauses>.Filter.Eq("HauseId", hauseId),
                    Builders<RentedHauses>.Filter.Eq("UserId", userId));

                var rentedHause = await _rentedHouseCollection.Find(filter).FirstOrDefaultAsync();
                return rentedHause;
            }
            catch
            {
                throw new Exception("Rented house not found");
            }
        }
        // Get a list of rented houses by user ID asynchronously
        public async Task<List<RentedHauses>> GetRentedHauses(ObjectId userId)
        {
            try
            {
                var filter = Builders<RentedHauses>.Filter.Eq("UserId", userId);
                var houses = await _rentedHouseCollection.Find(filter).ToListAsync();
                return houses;
            }
            catch
            {
                throw new Exception("user Id not found");
            }
        }
        // Update a rented house asynchronously
        public async Task<bool> UpdateRentedHause(RentedHauses hause)
        {
            try
            {
                var filter = Builders<RentedHauses>.Filter.Eq("_id", hause.Id);
                var update = Builders<RentedHauses>.Update
                    .Set("FromDate", hause.FromDate)
                    .Set("ToDate", hause.ToDate)
                    .Set("Numbers", hause.Numbers);

                var result = await _rentedHouseCollection.UpdateOneAsync(filter, update);
                return result.ModifiedCount > 0;
            }
            catch
            {
                return false;
            }
        }
    }
}
