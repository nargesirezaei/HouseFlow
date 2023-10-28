using HouseFlowPart1.Models;
using MongoDB.Bson;

namespace HouseFlowPart1.Interfaces
{
    public interface IHouseTypesService
    {
        List<HouseTypes> GetAll();
        //void SeedHouseTypes();
        HouseTypes GetById(ObjectId Id);
    }
}
