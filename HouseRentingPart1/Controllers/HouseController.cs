using HouseFlowPart1.Interfaces;
using HouseFlowPart1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using System.Security.Principal;

namespace HouseFlowPart1.Controllers
{
    public class HouseController : Controller
    {
        private readonly IHouseService _houseService;
        private readonly IHouseTypesService _houseTypesService;
        private readonly IAuthenticationService authenticationService;
        private readonly IRentedHauseService rentedHauseService;

        public HouseController(IHouseService houseService, IAuthenticationService authenticationService,
            IHouseTypesService houseTypesService, IRentedHauseService rentedHauseService)
        {
            _houseService = houseService;
            this.authenticationService = authenticationService;
            _houseTypesService = houseTypesService;
            this.rentedHauseService = rentedHauseService;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            IIdentity? identity = User.Identity;
            var username = identity != null ? identity.Name : "";
            if (string.IsNullOrEmpty(username)) return BadRequest();

            var user = await authenticationService.GetCurrentUserByUsername(username);
            if (!user.IsAdmin)
            {
                var tmp = await _houseService.GetUserHousesAsync(user.Id);

                List<HousesDTO> houses = new();

                tmp.ForEach(x =>
                {
                    houses.Add(new HousesDTO
                    {
                        Address = x.Address,
                        Description = x.Description,
                        FromDate = $"{x.FromDate:yyyy-MM-dd}",
                        ToDate = $"{x.ToDate:yyyy-MM-dd}",
                        Id = x.Id,
                        Price = x.Price,
                        Title = x.Title,
                        TypeTitle = _houseTypesService.GetById(x.TypeId)?.Title
                    });

                });

                return View(houses);
            }
            else
            {
                var houses = await _houseService.GetAllHousesAsync();
                return View(houses);
            }
        }

        public async Task<IActionResult> Details(string id)
        {
            if (!ObjectId.TryParse(id, out ObjectId objectId))
            {
                return BadRequest("Invalid house ID format.");
            }
            var model = await _houseService.GetHouseByImagesAsync(objectId);

            if (model == null) return NotFound();


            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                var userName = User.Identity != null ? User.Identity.Name : string.Empty;

                var user = await authenticationService.GetCurrentUserByUsername(userName ?? "");

                var rentedHauses = await rentedHauseService.GetRentedHauseByHauseIdAndUserId(model.House.Id, user.Id);

                model.RentedHauses = rentedHauses;
            }

            return View(model);
        }

        [Authorize]
        public IActionResult Create()
        {
            var model = _houseTypesService.GetAll();
            return View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(HousesDTO dto)
        {
            IIdentity? identity = User.Identity;
            var username = identity != null ? identity.Name : "";
            if (string.IsNullOrEmpty(username)) return BadRequest();

            var user = await authenticationService.GetCurrentUserByUsername(username);


            Houses house = new()
            {

                Address = dto.Address,
                Description = dto.Description,
                FromDate = DateTime.Parse(dto.FromDate).Date,
                ToDate = DateTime.Parse(dto.ToDate).Date,
                OwnerId = user.Id,
                Price = dto.Price,
                Title = dto.Title,
                TypeId = ObjectId.Parse(dto.TypeId)
            };


            var success = await _houseService.AddHouseAsync(house);

            if (success)
            {
                return RedirectToAction("Index");
            }
            else
            {
                // Optionally, you can display an error message on the same view
                ModelState.AddModelError(string.Empty, "Failed to add the house.");
                var model = _houseTypesService.GetAll();
                return View(model);
            }
        }

        [Authorize]
        public async Task<IActionResult> Edit(string id)
        {
            if (!ObjectId.TryParse(id, out ObjectId objectId))
            {
                return BadRequest("Invalid house ID format.");
            }

            var house = await _houseService.GetHouseAsync(objectId);

            if (house == null)
            {
                return NotFound();
            }

            HousesEditDTO model = new()
            {
                Data = house,
                Types = _houseTypesService.GetAll()
            };

            return View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Edit(string id, HousesDTO dto)
        {
            if (!ObjectId.TryParse(id, out ObjectId objectId))
            {
                return BadRequest("Invalid house ID format.");
            }

            Houses house = new()
            {
                Id = objectId,
                Address = dto.Address,
                Description = dto.Description,
                FromDate = DateTime.Parse(dto.FromDate).Date,
                ToDate = DateTime.Parse(dto.ToDate).Date,
                OwnerId = ObjectId.Parse(dto.OwnerId),
                Price = dto.Price,
                Title = dto.Title,
                TypeId = ObjectId.Parse(dto.TypeId)
            };



            if (!ModelState.IsValid)
            {
                HousesEditDTO model = new()
                {
                    Data = house,
                    Types = _houseTypesService.GetAll()
                };
                return View(model);
            }

            var success = await _houseService.UpdateHouseAsync(house);

            if (success)
            {
                return RedirectToAction("Index");
            }
            else
            {
                // Optionally, you can display an error message on the same view
                ModelState.AddModelError(string.Empty, "Failed to update the house.");
                return View(house);
            }
        }

        [Authorize]
        public async Task<IActionResult> Delete(string id)
        {
            if (!ObjectId.TryParse(id, out ObjectId objectId))
            {
                return BadRequest("Invalid house ID format.");
            }

            var house = await _houseService.GetHouseAsync(objectId);

            if (house == null)
            {
                return NotFound();
            }

            return View(house);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (!ObjectId.TryParse(id, out ObjectId objectId))
            {
                return BadRequest("Invalid house ID format.");
            }

            var house = await _houseService.GetHouseAsync(objectId);

            if (house == null)
            {
                return NotFound();
            }

            var success = await _houseService.DeleteHouseAsync(house);

            if (success)
            {
                return RedirectToAction("Index");
            }
            else
            {
                // Optionally, you can display an error message on the same view
                ModelState.AddModelError(string.Empty, "Failed to delete the house.");
                return View(house);
            }
        }
    }
}
