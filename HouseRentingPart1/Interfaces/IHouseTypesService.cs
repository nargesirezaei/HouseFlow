using HouseFlowPart1.Models;
using MongoDB.Bson;

namespace HouseFlowPart1.Interfaces
{
    public interface IHouseTypesService
    {
        List<HouseTypes> GetAll();
        
        HouseTypes GetById(ObjectId Id);
    }
}
