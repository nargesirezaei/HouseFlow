using HouseFlowPart1.Interfaces;
using HouseFlowPart1.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace HouseFlowPart1.Services
{
    public class HouseTypesService : IHouseTypesService
    {
        private readonly IMongoCollection<HouseTypes> _houseTypesCollection;
        private static readonly List<HouseTypes> list = new();
        private readonly MongoDBContext _db;
        public HouseTypesService(MongoDBContext db)
        {
            _db = db;
            _houseTypesCollection = _db.GetCollection<HouseTypes>("houseTypes");
        }

      

        public List<HouseTypes> GetAll()
        {
            try
            {
                return list.ToList();
            }
            catch
            {
                return new List<HouseTypes>();
            }
        }

        public HouseTypes GetById(ObjectId Id)
        {
            try
            {
                var house = list.FirstOrDefault(x => x.Id == Id);
                if (house == null) throw new Exception("House Id not found");
                else return house;
            }
            catch
            {
                return new HouseTypes();
            }
        }
    }
}
