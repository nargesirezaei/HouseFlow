using HouseFlowPart1.Models;
using MongoDB.Bson;

namespace HouseFlowPart1.Interfaces
{
    public interface IHouseImageService
    {
        Task<bool> AddImageAsync(HouseImages image);
        Task<bool> DeleteImageAsync(ObjectId id);
        Task<HouseImages> GetHouseImageAsync(ObjectId id);
        Task<List<HouseImages>> GetImagesByHouseIdAsync(ObjectId houseId);
<<<<<<< HEAD
        
=======
       // void SeedData();
>>>>>>> comments
    }
}
