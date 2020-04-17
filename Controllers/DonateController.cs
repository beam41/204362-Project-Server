using MheanMaa.Enum;
using MheanMaa.Models;
using MheanMaa.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using static MheanMaa.Util.ClaimSearch;

namespace MheanMaa.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class DonateController : ControllerBase
    {
        private readonly DonateService _donateService;

        public DonateController(DonateService donateService)
        {
            _donateService = donateService;
        }

        [HttpGet("list")]
        public ActionResult<List<DonateList>> Get()
        {
            if (GetClaim(User, ClaimEnum.UserType) == "A")
            {
                return _donateService.Get().Select(don => new DonateList
                {
                    Id = don.Id,
                    Title = don.Title,
                    Creator = don.Creator,
                    Accepted = don.Accepted
                }).ToList();
            }
            return _donateService.Get(int.Parse(GetClaim(User, ClaimEnum.DeptNo))).Select(don => new DonateList
            {
                Id = don.Id,
                Title = don.Title,
                Creator = don.Creator,
                Accepted = don.Accepted
            }).ToList();
        }

        [AllowAnonymous]
        [HttpGet("visitor")]
        public ActionResult<List<DonateVisitor>> GetForVisitor()
        {
            return _donateService.GetAcceptedDonates().Select(don => (DonateVisitor)don).ToList();
        }

        [AllowAnonymous]
        [HttpGet("random/{len}")]
        public ActionResult<List<DonateHome>> GetRandom(int len)
        {
            Random rng = new Random();
            return _donateService.Get().OrderBy(_ => rng.Next()).Take(len).Select(don => new DonateHome 
            { 
                Id= don.Id,
                Title = don.Title,
                ImgPath = don.ImgPath
            }).ToList();
        }

        [HttpGet("{id:length(24)}", Name = "GetDonate")]
        public ActionResult<Donate> Get(string id)
        {
            Donate don;
            if (GetClaim(User, ClaimEnum.UserType) == "A")
            {
                don = _donateService.Get(id);
            }
            else
            {
                don = _donateService.Get(id, int.Parse(GetClaim(User, ClaimEnum.DeptNo)));
            }


            if (don == null)
            {
                return NotFound();
            }

            return don;
        }

        [HttpPost]
        public ActionResult<Donate> Create(Donate don)
        {
            // prevent change
            don.Accepted = false;
            don.Creator = GetClaim(User, ClaimEnum.FirstName);
            don.DeptNo = int.Parse(GetClaim(User, ClaimEnum.DeptNo));
            // create
            _donateService.Create(don);

            return CreatedAtRoute("GetDonate", new { id = don.Id.ToString() }, don);
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, Donate donIn)
        {
            // fetch
            Donate don;
            if (GetClaim(User, ClaimEnum.UserType) == "A")
            {
                don = _donateService.Get(id);
            }
            else
            {
                don = _donateService.Get(id, int.Parse(GetClaim(User, ClaimEnum.DeptNo)));
            }

            // not found (bc wrong dept or no donate record)
            if (don == null)
            {
                return NotFound();
            }

            // prevent change
            donIn.Id = don.Id;
            donIn.Accepted = false;
            donIn.Creator = don.Creator;
            donIn.DeptNo = don.DeptNo;
            _donateService.Update(id, donIn);

            return NoContent();
        }

        [HttpPatch("{id:length(24)}")]
        public IActionResult Accept(string id)
        {
            // fetch
            Donate don;
            if (GetClaim(User, ClaimEnum.UserType) == "A")
            {
                don = _donateService.Get(id);
            }
            else
            {
                don = _donateService.Get(id, int.Parse(GetClaim(User, ClaimEnum.DeptNo)));
            }

            // not found (bc wrong dept or no donate record)
            if (don == null)
            {
                return NotFound();
            }

            // no reaccept
            if (don.Accepted == false && GetClaim(User, ClaimEnum.UserType) == "A")
            {
                don.Accepted = true;
                don.AcceptedOn = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                don.AcceptedBy = GetClaim(User, ClaimEnum.FirstName);
                _donateService.Update(id, don);
            }
            else if (GetClaim(User, ClaimEnum.UserType) != "A")
            {
                return Unauthorized();
            }

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            Donate don;
            if (GetClaim(User, ClaimEnum.UserType) == "A")
            {
                don = _donateService.Get(id);
            }
            else
            {
                don = _donateService.Get(id, int.Parse(GetClaim(User, ClaimEnum.DeptNo)));
            }

            if (don == null)
            {
                return NotFound();
            }

            _donateService.Remove(don);

            return NoContent();
        }
    }
}