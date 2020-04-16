using System;
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
    public class ReportController : ControllerBase
    {
        private readonly ReportService _reportService;

        public ReportController(ReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet("list")]
        public ActionResult<List<ReportList>> Get()
        {
            if (GetClaim(User, ClaimEnum.UserType) == "A")
            {
                return _reportService.Get().Select(rep => new ReportList
                {
                    Id = rep.Id,
                    Title = rep.Title,
                    Reporter = rep.Reporter,
                    Accepted = rep.Accepted
                }).ToList();
            }
            return _reportService.Get(int.Parse(GetClaim(User, ClaimEnum.DeptNo))).Select(rep => new ReportList
            {
                Id = rep.Id,
                Title = rep.Title,
                Reporter = rep.Reporter,
                Accepted = rep.Accepted
            }).ToList();
        }

        [AllowAnonymous]
        [HttpGet("visitor")]
        public ActionResult<List<ReportVisitor>> GetForVisitor()
        {
            return _reportService.GetAcceptedReports().Select(rep => (ReportVisitor)rep).ToList();
        }

        [HttpGet("{id:length(24)}", Name = "GetReport")]
        public ActionResult<Report> Get(string id)
        {
            Report rep;
            if (GetClaim(User, ClaimEnum.UserType) == "A")
            {
                rep = _reportService.Get(id);
            }
            else
            {
                rep = _reportService.Get(id, int.Parse(GetClaim(User, ClaimEnum.DeptNo)));
            }


            if (rep == null)
            {
                return NotFound();
            }

            return rep;
        }

        [HttpPost]
        public ActionResult<Report> Create(Report rep)
        {
            // prevent change
            rep.Accepted = false;
            rep.Reporter = GetClaim(User, ClaimEnum.FirstName);
            rep.DeptNo = int.Parse(GetClaim(User, ClaimEnum.DeptNo));
            // create
            _reportService.Create(rep);

            return CreatedAtRoute("GetReport", new { id = rep.Id.ToString() }, rep);
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, Report repIn)
        {
            // fetch
            Report rep;
            if (GetClaim(User, ClaimEnum.UserType) == "A")
            {
                rep = _reportService.Get(id);
            }
            else
            {
                rep = _reportService.Get(id, int.Parse(GetClaim(User, ClaimEnum.DeptNo)));
            }

            // not found (bc wrong dept or no donate record)
            if (rep == null)
            {
                return NotFound();
            }

            // prevent change
            repIn.Id = rep.Id;
            repIn.Accepted = false;
            repIn.Reporter = rep.Reporter;
            repIn.DeptNo = rep.DeptNo;
            _reportService.Update(id, repIn);

            return NoContent();
        }

        [HttpPatch("{id:length(24)}")]
        public IActionResult Accept(string id)
        {
            // fetch
            Report rep;
            if (GetClaim(User, ClaimEnum.UserType) == "A")
            {
                rep = _reportService.Get(id);
            }
            else
            {
                rep = _reportService.Get(id, int.Parse(GetClaim(User, ClaimEnum.DeptNo)));
            }

            // not found (bc wrong dept or no donate record)
            if (rep == null)
            {
                return NotFound();
            }

            // no reaccept
            if (rep.Accepted == false && GetClaim(User, ClaimEnum.UserType) == "A")
            {
                rep.Accepted = true;
                rep.AcceptedDate = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                _reportService.Update(id, rep);
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
            Report rep;
            if (GetClaim(User, ClaimEnum.UserType) == "A")
            {
                rep = _reportService.Get(id);
            }
            else
            {
                rep = _reportService.Get(id, int.Parse(GetClaim(User, ClaimEnum.DeptNo)));
            }

            if (rep == null)
            {
                return NotFound();
            }

            _reportService.Remove(rep);

            return NoContent();
        }
    }
}