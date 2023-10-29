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
        // Display a list of houses
        [Authorize]
        public async Task<IActionResult> Index()
        {
            // Check user identity and access rights
            IIdentity? identity = User.Identity;
            var username = identity != null ? identity.Name : "";
            if (string.IsNullOrEmpty(username)) return BadRequest();

            var user = await authenticationService.GetCurrentUserByUsername(username);
            if (!user.IsAdmin)
            {
                // Retrieve and display houses for non-admin users
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
                // Retrieve and display all houses for admin users
                var houses = await _houseService.GetAllHousesAsync();
                return View(houses);
            }
        }
        // Display details of a house
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
                // Retrieve and include rented houses information
                var rentedHauses = await rentedHauseService.GetRentedHauseByHauseIdAndUserId(model.House.Id, user.Id);

                model.RentedHauses = rentedHauses;
            }

            return View(model);
        }
        // Create a new house
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
            // Check user identity and access rights
            IIdentity? identity = User.Identity;
            var username = identity != null ? identity.Name : "";
            if (string.IsNullOrEmpty(username)) return BadRequest();

            var user = await authenticationService.GetCurrentUserByUsername(username);


            Houses house = new()
            {
                // Create a new house and save it
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
        // Edit an existing house
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
            // Display the house data for editing
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
            // Prepare the updated house object
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
                // Display the editing form with errors
                HousesEditDTO model = new()
                {
                    Data = house,
                    Types = _houseTypesService.GetAll()
                };
                return View(model);
            }
            // Update the house
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
        // Delete confirmation view
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
        // Handle house deletion
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
            // Delete the house and return to the house list
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
