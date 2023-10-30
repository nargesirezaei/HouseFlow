using HouseFlowPart1.Models;
using MongoDB.Bson;

namespace HouseFlowPart1.Interfaces
{
    public interface IRentedHauseService
    {
        Task<List<RentedHauses>> GetRentedHauses(ObjectId userId); // Get a list of houses rented by a specific user
        Task<bool> DeleteRentedHause(ObjectId id); // Delete a rented house by its unique identifier// Delete a rented house by its unique identifier
        Task<bool> AddRentedHause(RentedHauses hause); // Add a new rented house

        Task<bool> UpdateRentedHause(RentedHauses hause); // Update rented house details
        Task<RentedHauses> GetRentedHause(ObjectId Id); // Get a rented house by its unique identifier
        Task<RentedHauses> GetRentedHauseByHauseIdAndUserId(ObjectId hauseId, ObjectId userId); // Get a rented house by its unique identifier and user ID
    }
}
