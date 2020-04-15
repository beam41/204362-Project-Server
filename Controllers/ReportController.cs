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
    public class ReportController : ControllerBase
    {
        private readonly ReportService _reportService;
        private readonly UserService _userService;

        public ReportController(ReportService reportService, UserService userService)
        {
            _reportService = reportService;
            _userService = userService;
        }

        [HttpGet("list")]
        public ActionResult<List<ReportList>> Get()
        {
            User user = _userService.Find(User.Identity.Name);

            return _reportService.Get(user.DeptNo).Select(rep => new ReportList
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
            User user = _userService.Find(User.Identity.Name);
            Report rep = _reportService.Get(id, user.DeptNo);

            if (rep == null)
            {
                return NotFound();
            }

            return rep;
        }

        [HttpPost]
        public ActionResult<Report> Create(Report rep)
        {
            //fetch
            User user = _userService.Find(User.Identity.Name);

            // prevent change
            rep.Accepted = false;
            rep.Reporter = user.FirstName;
            rep.DeptNo = user.DeptNo;
            // create
            _reportService.Create(rep);

            return CreatedAtRoute("GetReport", new { id = rep.Id.ToString() }, rep);
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, Report repIn)
        {
            // fetch
            User user = _userService.Find(User.Identity.Name);
            Report rep = _reportService.Get(id, user.DeptNo);

            // not found (bc wrong dept or no report record)
            if (rep == null)
            {
                return NotFound();
            }

            // prevent change
            rep.Accepted = false;
            repIn.Reporter = rep.Reporter;
            repIn.DeptNo = rep.DeptNo;
            _reportService.Update(id, repIn);

            return NoContent();
        }

        [HttpPatch("{id:length(24)}")]
        public IActionResult Accept(string id)
        {
            // fetch
            User user = _userService.Find(User.Identity.Name);
            Report rep = _reportService.Get(id, user.DeptNo);

            // not found (bc wrong dept or no report record)
            if (rep == null)
            {
                return NotFound();
            }

            // no reaccept
            if (rep.Accepted == false)
            {
                rep.Accepted = true;
                rep.AcceptedDate = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                _reportService.Update(id, rep);
            }


            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            User user = _userService.Find(User.Identity.Name);
            Report rep = _reportService.Get(id, user.DeptNo);

            if (rep == null)
            {
                return NotFound();
            }

            _reportService.Remove(rep);

            return NoContent();
        }
    }
}