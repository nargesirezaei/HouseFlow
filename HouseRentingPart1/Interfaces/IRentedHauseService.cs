using HouseFlowPart1.Models;
using MongoDB.Bson;

namespace HouseFlowPart1.Interfaces
{
    public interface IRentedHauseService
    {
        Task<List<RentedHauses>> GetRentedHauses(ObjectId userId);
        Task<bool> DeleteRentedHause(ObjectId id);
        Task<bool> AddRentedHause(RentedHauses hause);

        Task<bool> UpdateRentedHause(RentedHauses hause);
        Task<RentedHauses> GetRentedHause(ObjectId Id);
        Task<RentedHauses> GetRentedHauseByHauseIdAndUserId(ObjectId hauseId, ObjectId userId);
    }
}
