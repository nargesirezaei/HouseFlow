using HouseFlowPart1.Interfaces;
using HouseFlowPart1.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HouseFlowPart1.Services
{
    public class HouseImageService : IHouseImageService
    {
        private readonly IMongoCollection<HouseImages> _imageCollection;
        private readonly MongoDBContext _db;

        public HouseImageService(MongoDBContext db)
        {
            _db = db;
            _imageCollection = _db.GetCollection<HouseImages>("houseImages");
        }

       

        public async Task<bool> AddImageAsync(HouseImages image)
        {
            try
            {
                await _imageCollection.InsertOneAsync(image);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<HouseImages> GetHouseImageAsync(ObjectId id)
        {
            try
            {
                var filter = Builders<HouseImages>.Filter.Eq("_id", id);

                var image = await _imageCollection.Find(filter).FirstOrDefaultAsync();
                return image;
            }
            catch 
            {
                return new HouseImages();
            }
        }

        public async Task<bool> DeleteImageAsync(ObjectId id)
        {
            try
            {
                var filter = Builders<HouseImages>.Filter.Eq("_id", id);
                var result = await _imageCollection.DeleteOneAsync(filter);
                return result.DeletedCount > 0;
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<HouseImages>> GetImagesByHouseIdAsync(ObjectId houseId)
        {
            try
            {
                var filter = Builders<HouseImages>.Filter.Eq("HouseId", houseId);
                var images = await _imageCollection.Find(filter).ToListAsync();
                return images;
            }
            catch 
            {
                return new List<HouseImages>();
            }
        }
    }
}
