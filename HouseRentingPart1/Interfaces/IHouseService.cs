using HouseFlowPart1.Models;
using MongoDB.Bson;

namespace HouseFlowPart1.Interfaces
{
    public interface IHouseService
    {
        Task<List<Houses>> GetAllHousesAsync();
        Task<List<Houses>> GetUserHousesAsync(ObjectId ownerId);
        Task<Houses> GetHouseAsync(ObjectId id);

        Task<bool> AddHouseAsync(Houses house);
        Task<bool> DeleteHouseAsync(Houses house);
        Task<bool> UpdateHouseAsync(Houses house);
        Task<List<HouseImagesViewModel>> GetAllHousesWithImages();
        Task<HouseDetailViewModel> GetHouseByImagesAsync(ObjectId objectId);
       
    }
}
