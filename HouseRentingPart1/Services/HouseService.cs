using HouseFlowPart1.Interfaces;
using HouseFlowPart1.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace HouseFlowPart1.Services
{
    public class HouseService : IHouseService
    {
        private readonly IMongoCollection<Houses> _houseCollection;
        private readonly IMongoCollection<HouseImages> _houseImagesCollection;
        private readonly IHouseTypesService _houseTypesService;

        private readonly MongoDBContext _db;

        public HouseService(MongoDBContext db, IHouseTypesService houseTypesService)
        {
            _db = db;
            _houseCollection = _db.GetCollection<Houses>("houses");
            _houseImagesCollection = _db.GetCollection<HouseImages>("houseImages");
            _houseTypesService = houseTypesService;
        }


       

        public async Task<bool> AddHouseAsync(Houses house)
        {
            try
            {
                await _houseCollection.InsertOneAsync(house);
                return true;
            }
            catch 
            {
                return false;
            }
        }

        public async Task<bool> DeleteHouseAsync(Houses house)
        {
            try
            {
                var filter = Builders<Houses>.Filter.Eq("_id", house.Id);
                var result = await _houseCollection.DeleteOneAsync(filter);
                return result.DeletedCount > 0;
            }
            catch 
            {
                return false;
            }
        }

        public async Task<List<Houses>> GetAllHousesAsync()
        {
            try
            {
                var houses = await _houseCollection.Find(_ => true).ToListAsync();
                return houses;
            }
            catch
            {
                return new List<Houses>();
            }
        }

        public async Task<List<HouseImagesViewModel>> GetAllHousesWithImages()
        {
            try
            {
                List<HouseImagesViewModel> housesWithImages = new();

                var houses = await _houseCollection.Find(_ => true).ToListAsync();

                foreach (var house in houses)
                {
                    var images = await _houseImagesCollection.Find(i => i.HouseId == house.Id).ToListAsync();
                    if (!images.Any()) return new List<HouseImagesViewModel>();

                    var image = images.FirstOrDefault() ?? new HouseImages();

                    var houseWithImages = new HouseImagesViewModel
                    {
                        House = new HousesDTO
                        {
                            Id = house.Id,
                            Title = house.Title,
                            Price = house.Price,
                            Address = house.Address,
                            TypeTitle = _houseTypesService.GetById(house.TypeId)?.Title,
                            
                        },
                        Image = image,
                    };

                    housesWithImages.Add(houseWithImages);
                }

                return housesWithImages;
            }
            catch 
            {
                return new List<HouseImagesViewModel>();
            }
        }

        public async Task<Houses> GetHouseAsync(ObjectId id)
        {
            try
            {
                var filter = Builders<Houses>.Filter.Eq("_id", id);
                var house = await _houseCollection.Find(filter).FirstOrDefaultAsync();
                return house;
            }
            catch 
            {
                throw new Exception("House Id not found");
            }
        }

        public async Task<HouseDetailViewModel> GetHouseByImagesAsync(ObjectId houseId)
        {
            try
            {
                var data = await _houseCollection.FindAsync(h => h.Id == houseId);

                var house = await data.FirstOrDefaultAsync();

                if (house == null)
                {
                    throw new Exception("House Id not found");
                }

                var _images = await _houseImagesCollection.FindAsync(i => i.HouseId == house.Id);

                var images = await _images.ToListAsync();

                var houseDetailViewModel = new HouseDetailViewModel
                {
                    House = new HousesDTO
                    {
                        Id = house.Id,
                        Title = house.Title,
                        Price = house.Price,
                        Address = house.Address,
                        Description = house.Description,
                        ToDate = $"{house.ToDate:yyyy-MM-dd}",
                        FromDate = $"{house.FromDate:yyyy-MM-dd}",
                        TypeTitle = _houseTypesService.GetById(house.TypeId)?.Title,
                        // Add other properties as needed
                    },
                    Images = images
                };

                return houseDetailViewModel;
            }
            catch (Exception ex)
            {
                // Handle exception or log error
                throw new Exception("Error retrieving house details by images", ex);
            }
        }

        public async Task<List<Houses>> GetUserHousesAsync(ObjectId ownerId)
        {
            try
            {
                var filter = Builders<Houses>.Filter.Eq("OwnerId", ownerId);
                var houses = await _houseCollection.Find(filter).ToListAsync();
                return houses;
            }
            catch 
            {
                return new List<Houses>();
            }
        }

        public async Task<bool> UpdateHouseAsync(Houses house)
        {
            try
            {
                var filter = Builders<Houses>.Filter.Eq("_id", house.Id);
                var update = Builders<Houses>.Update
                    .Set("Title", house.Title)
                    .Set("Address", house.Address)
                    .Set("Description", house.Description)
                    .Set("FromDate", house.FromDate)
                    .Set("ToDate", house.ToDate)
                    .Set("TypeId", house.TypeId)
                    .Set("Price", house.Price);

                var result = await _houseCollection.UpdateOneAsync(filter, update);
                return result.ModifiedCount > 0;
            }
            catch
            {
                return false;
            }
        }

    }
}
