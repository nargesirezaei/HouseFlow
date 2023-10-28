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

       /* public void SeedHouseTypes()
        {
            var types = _houseTypesCollection.Find(_ => true);
            if (types.Any()) list.AddRange(types.ToList());
            else
            {
                var new_types = new List<HouseTypes>
                {
                    //it's better and safer to add static and predefined Id because of relation to other collections

                    new HouseTypes { Title = "Detached House",Id = new MongoDB.Bson.ObjectId("6516a18447331b1448c897f9") },
                    new HouseTypes { Title = "Apartment",Id = new MongoDB.Bson.ObjectId("6516a18447331b1448c897fa") },
                    new HouseTypes { Title = "Cottage",Id = new MongoDB.Bson.ObjectId("6516a18447331b1448c897fb") }
                };

                _houseTypesCollection.InsertMany(new_types);
                list.AddRange(new_types);
            }
        }
       */

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
