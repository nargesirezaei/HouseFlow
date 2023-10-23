using HouseFlowPart1.Interfaces;
using HouseFlowPart1.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace HouseFlowPart1.Controllers
{
    public class HouseImagesController : Controller
    {
        private readonly IHouseImageService _houseImageService;

        public HouseImagesController(IHouseImageService houseImageService)
        {
            _houseImageService = houseImageService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string houseId)
        {
            if (!ObjectId.TryParse(houseId, out ObjectId objectId))
            {
                return BadRequest("Invalid house ID format.");
            }

            List<HouseImages> images = await _houseImageService.GetImagesByHouseIdAsync(objectId);
            
            var model = new HouseImagesDTO { HouseId = houseId, Images = images };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Index(string houseId, IFormFile imageFile)
        {
            if (!ObjectId.TryParse(houseId, out ObjectId objectId))
            {
                return BadRequest("Invalid house ID format.");
            }

            if (imageFile != null && imageFile.Length > 0)
            {
                try
                {
                    using var memoryStream = new MemoryStream();
                    await imageFile.CopyToAsync(memoryStream);
                    var imageBytes = memoryStream.ToArray();

                    HouseImages newImage = new()
                    {
                        HouseId = objectId,
                        ImageUrl = Convert.ToBase64String(imageBytes) // Store image as base64 string or a URL as per your requirements
                    };

                    await _houseImageService.AddImageAsync(newImage);
                }
                catch
                {
                    return BadRequest("Error uploading image.");
                }
            }

            return RedirectToAction("Index", new { houseId });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteImage(string imageId)
        {
            if (!ObjectId.TryParse(imageId, out ObjectId objectId))
            {
                return BadRequest("Invalid house ID format.");
            }

            var image = await _houseImageService.GetHouseImageAsync(objectId);

            bool success = await _houseImageService.DeleteImageAsync(objectId);

            if (success)
            {
                return RedirectToAction("Index", new { houseId = image.HouseId });
            }
            else
            {
                return BadRequest("Error deleting image.");
            }
        }
    }
}
