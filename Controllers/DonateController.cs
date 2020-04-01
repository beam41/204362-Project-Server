using System;
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
    public class DonateController : ControllerBase
    {
        private readonly DonateService _donateService;
        private readonly UserService _userService;

        public DonateController(DonateService donateService, UserService userService)
        {
            _donateService = donateService;
            _userService = userService;
        }

        [HttpGet("list")]
        public ActionResult<List<DonateList>> Get()
        {
            User user = _userService.Find(User.Identity.Name);

            return _donateService.Get(user.DeptNo).Select(don => new DonateList
            {
                Id = don.Id,
                Title = don.Title,
                Creator = don.Creator,
                Accepted = don.Accepted
            }).ToList();
        }

        [HttpGet("{id:length(24)}", Name = "GetDonate")]
        public ActionResult<Donate> Get(string id)
        {
            User user = _userService.Find(User.Identity.Name);
            Donate don = _donateService.Get(id, user.DeptNo);

            if (don == null)
            {
                return NotFound();
            }

            return don;
        }

        [HttpPost]
        public ActionResult<Donate> Create(Donate don)
        {
            //fetch
            User user = _userService.Find(User.Identity.Name);

            // prevent change
            don.Accepted = false;
            don.Creator = user.FirstName;
            don.DeptNo = user.DeptNo;
            // create
            _donateService.Create(don);

            return CreatedAtRoute("GetDonate", new { id = don.Id.ToString() }, don);
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, Donate donIn)
        {
            // fetch
            User user = _userService.Find(User.Identity.Name);
            Donate don = _donateService.Get(id, user.DeptNo);

            // not found (bc wrong dept or no donate record)
            if (don == null)
            {
                return NotFound();
            }

            // prevent change
            don.Accepted = false;
            donIn.Creator = don.Creator;
            donIn.DeptNo = don.DeptNo;
            _donateService.Update(id, donIn);

            return NoContent();
        }

        [HttpPatch("{id:length(24)}")]
        public IActionResult Accept(string id)
        {
            // fetch
            User user = _userService.Find(User.Identity.Name);
            Donate don = _donateService.Get(id, user.DeptNo);

            // not found (bc wrong dept or no donate record)
            if (don == null)
            {
                return NotFound();
            }

            // no reaccept
            if (don.Accepted == false && user.UserType == "A")
            {
                don.Accepted = true;
                don.AcceptedOn = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                _donateService.Update(id, don);
            } 
            else if (user.UserType != "A")
            {
                return Unauthorized();
            }
            
            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            User user = _userService.Find(User.Identity.Name);
            Donate don = _donateService.Get(id, user.DeptNo);

            if (don == null)
            {
                return NotFound();
            }

            _donateService.Remove(don);

            return NoContent();
        }
    }
}