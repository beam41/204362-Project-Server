using System.Collections.Generic;
using System.Linq;
using MheanMaa.Enum;
using MheanMaa.Models;
using MheanMaa.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static MheanMaa.Util.ClaimSearch;

namespace MheanMaa.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class DogController : ControllerBase
    {
        private readonly DogService _dogService;

        public DogController(DogService dogService)
        {
            _dogService = dogService;
        }

        [HttpGet("list")]
        public ActionResult<List<DogList>> Get()
        {
            return _dogService.Get(int.Parse(GetClaim(User, ClaimEnum.DeptNo))).Select(dog => new DogList
            {
                Id = dog.Id,
                Name = dog.Name,
                AgeYear = dog.AgeYear,
                AgeMonth = dog.AgeMonth,
                Sex = dog.Sex,
                Description = dog.Description,
                IsAlive = dog.IsAlive,
                CollarColor = dog.CollarColor,
                Caretaker = dog.Caretaker
            }).ToList();
        }

        [HttpGet("{id:length(24)}", Name = "GetDog")]
        public ActionResult<Dog> Get(string id)
        {
            Dog dog = _dogService.Get(id, int.Parse(GetClaim(User, ClaimEnum.DeptNo)));

            if (dog == null)
            {
                return NotFound();
            }

            return dog;
        }

        [HttpPost]
        public ActionResult<Dog> Create(Dog dog)
        {
            dog.DeptNo = int.Parse(GetClaim(User, ClaimEnum.DeptNo));
            _dogService.Create(dog);

            return CreatedAtRoute("GetDog", new { id = dog.Id.ToString() }, dog);
        }

        [AllowAnonymous]
        [HttpGet("visitor")]
        public ActionResult<List<DogVisitor>> GetForVisitor()
        {
            return _dogService.Get().Select(dog => new DogVisitor
            {
                Id = dog.Id,
                Name = dog.Name,
                ImgPath = dog.ImgPath,

            }).ToList();
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, Dog dogIn)
        {
            Dog dog = _dogService.Get(id, int.Parse(GetClaim(User, ClaimEnum.DeptNo)));

            if (dog == null)
            {
                return NotFound();
            }
            dogIn.Id = dog.Id;
            dogIn.DeptNo = int.Parse(GetClaim(User, ClaimEnum.DeptNo));
            _dogService.Update(id, dogIn);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            Dog dog = _dogService.Get(id, int.Parse(GetClaim(User, ClaimEnum.DeptNo)));

            if (dog == null)
            {
                return NotFound();
            }

            _dogService.Remove(dog);

            return NoContent();
        }

    }
}