using HouseFlowPart1.Interfaces;
using HouseFlowPart1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace HouseFlowPart1.Controllers
{
    [Authorize]
    public class RentedHauseController : Controller
    {
        private readonly IRentedHauseService _rentedHauseService;
        private readonly IAuthenticationService authenticationService;
        private readonly IHouseService houseService;
        private readonly IHouseImageService houseImageService;
        private readonly IHouseTypesService houseTypesService;

        public RentedHauseController(IRentedHauseService rentedHauseService, IAuthenticationService authenticationService, IHouseService houseService, IHouseImageService houseImageService, IHouseTypesService houseTypesService)
        {
            _rentedHauseService = rentedHauseService;
            this.authenticationService = authenticationService;
            this.houseService = houseService;
            this.houseImageService = houseImageService;
            this.houseTypesService = houseTypesService;
        }

        public async Task<IActionResult> Index()
        {
            if (User.Identity != null && !string.IsNullOrEmpty(User.Identity.Name) && User.Identity.IsAuthenticated)
            {
                var user = await authenticationService.GetCurrentUserByUsername(User.Identity.Name);
                //List<HouseImagesViewModel> homesWithImages = await houseService.GetAllHousesWithImages();
                // Retrieve all rented houses
                var rentedHauses = await _rentedHauseService.GetRentedHauses(user.Id);

                var model = new List<RentedHausesDTO>();

                foreach (var x in rentedHauses)
                {

                    var hause = await houseService.GetHouseAsync(x.HauseId);

                    var hauseImage = new HouseImages();

                    var tmp = await houseImageService.GetImagesByHouseIdAsync(hause.Id);
                    if (tmp is not null && tmp.Any())
                    {
                        hauseImage = tmp.First();
                    }

                    model.Add(new RentedHausesDTO
                    {
                        FromDate = $"{x.FromDate:yyyy-MM-dd}",
                        ToDate = $"{x.ToDate:yyyy-MM-dd}",
                        Numbers = x.Numbers,
                        Id = x.Id.ToString(),
                        RequestDate = x.RequestDate,
                        TotalPrice = (x.ToDate - x.FromDate).Days * hause.Price,
                        HouseImage = hauseImage,
                        House = new HousesDTO
                        {
                            Id = hause.Id,
                            Title = hause.Title,
                            TypeTitle = houseTypesService.GetById(hause.TypeId).Title,
                        }
                    });
                }


                return View(model);
            }
            else
                return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public async Task<IActionResult> Create(string id)
        {
            if (!ObjectId.TryParse(id, out ObjectId hauseId))
            {
                return BadRequest("Invalid ID format.");
            }
            if (User.Identity != null && !string.IsNullOrEmpty(User.Identity.Name) && User.Identity.IsAuthenticated)
            {
                var user = await authenticationService.GetCurrentUserByUsername(User.Identity.Name);
                var model = new RentedHauses
                {
                    UserId = user.Id,
                    HauseId = hauseId,
                };

                return View(model);
            }
            else
            {
                return BadRequest();
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(RentedHauses rentedHauses, string id)
        {

            if (!ObjectId.TryParse(id, out ObjectId hauseId))
            {
                return BadRequest("Invalid ID format.");
            }

            if (ModelState.IsValid)
            {
                if (User.Identity != null && !string.IsNullOrEmpty(User.Identity.Name) && User.Identity.IsAuthenticated)
                {
                    rentedHauses.RequestDate = DateTime.Now;

                    var user = await authenticationService.GetCurrentUserByUsername(User.Identity.Name);

                    rentedHauses.UserId = user.Id;
                    rentedHauses.HauseId = hauseId;

                    // Add the rented house
                    bool result = await _rentedHauseService.AddRentedHause(rentedHauses);
                    if (result)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Failed to add the rented house.");
                    }
                }
            }
            return View(rentedHauses);
        }
        [Authorize]
        public async Task<IActionResult> Edit(string id)
        {
            if (!ObjectId.TryParse(id, out ObjectId objectId))
            {
                return BadRequest("Invalid ID format.");
            }

            // Retrieve the rented house by ID
            var rentedHauses = await _rentedHauseService.GetRentedHause(objectId);
            if (rentedHauses == null)
            {
                return NotFound();
            }


            if (ModelState.IsValid)
            {
                if (User.Identity != null && !string.IsNullOrEmpty(User.Identity.Name) && User.Identity.IsAuthenticated)
                {
                    var user = await authenticationService.GetCurrentUserByUsername(User.Identity.Name);
                    if (rentedHauses.UserId != user.Id) return BadRequest();
                }
                var model = new RentedHausesDTO
                {
                    Numbers = rentedHauses.Numbers,
                    FromDate = $"{rentedHauses.FromDate:yyyy-MM-dd}",
                    ToDate = $"{rentedHauses.ToDate:yyyy-MM-dd}",
                    HauseId = rentedHauses.HauseId.ToString(),
                    UserId = rentedHauses.UserId.ToString(),
                };

                return View(model);
            }
            else
            {
                return BadRequest();
            }

        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Edit(RentedHausesDTO rentedHauses)
        {
            var RentedHause = new RentedHauses()
            {
                Id = ObjectId.Parse(rentedHauses.Id),
                FromDate = DateTime.Parse(rentedHauses.FromDate),
                ToDate = DateTime.Parse(rentedHauses.ToDate),
                Numbers = rentedHauses.Numbers,
                HauseId = ObjectId.Parse(rentedHauses.HauseId),
                UserId = ObjectId.Parse(rentedHauses.UserId),
            };

            // Update the rented house
            bool result = await _rentedHauseService.UpdateRentedHause(RentedHause);
            if (result)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Failed to update the rented house.");
            }
            return View(rentedHauses);
        }
        [Authorize]
        public async Task<IActionResult> PreDelete(string id)
        {
            if (!ObjectId.TryParse(id, out ObjectId objectId))
            {
                return BadRequest("Invalid ID format.");
            }
            var rentedHause = await _rentedHauseService.GetRentedHause(objectId);
            if (rentedHause == null) return NotFound();

            var House = await houseService.GetHouseAsync(rentedHause.HauseId);

            var model = new RentedHausesDTO()
            {
                Id = objectId.ToString(),
                House = new HousesDTO()
                {
                    Id = House.Id,
                    Title = House.Title,
                }
            };

            return View("delete", model);
        }


        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {

            if (!ObjectId.TryParse(id, out ObjectId objectId))
            {
                return BadRequest("Invalid ID format.");
            }
            // Delete the rented house by ID
            bool result = await _rentedHauseService.DeleteRentedHause(objectId);
            if (result)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return NotFound();
            }
        }
    }
}
