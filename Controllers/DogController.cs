using System.Collections.Generic;
using System.Linq;
using MheanMaa.Models;
using MheanMaa.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MheanMaa.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class DogController : ControllerBase
    {
        private readonly DogService _dogService;
        private readonly UserService _userService;

        public DogController(DogService dogService, UserService userService)
        {
            _dogService = dogService;
            _userService = userService;
        }

        [HttpGet("list")]
        public ActionResult<List<DogList>> Get()
        {
            User user = _userService.Find(User.Identity.Name);

            return _dogService.Get(user.DeptNo).Select(dog => new DogList
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
            User user = _userService.Find(User.Identity.Name);
            Dog dog = _dogService.Get(id, user.DeptNo);

            if (dog == null)
            {
                return NotFound();
            }

            return dog;
        }

        [HttpPost]
        public ActionResult<Dog> Create(Dog dog)
        {
            User user = _userService.Find(User.Identity.Name);
            dog.DeptNo = user.DeptNo;
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
            User user = _userService.Find(User.Identity.Name);
            Dog dog = _dogService.Get(id, user.DeptNo);

            if (dog == null)
            {
                return NotFound();
            }
            dogIn.DeptNo = user.DeptNo;
            _dogService.Update(id, dogIn);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            User user = _userService.Find(User.Identity.Name);
            Dog dog = _dogService.Get(id, user.DeptNo);

            if (dog == null)
            {
                return NotFound();
            }

            _dogService.Remove(dog);

            return NoContent();
        }

    }
}