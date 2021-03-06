﻿using MheanMaa.Enums;
using MheanMaa.Models;
using MheanMaa.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using static MheanMaa.Utils.ClaimSearch;

namespace MheanMaa.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly ReportService _reportService;
        private readonly ImageService _imageService;

        public ReportController(ReportService reportService, ImageService imageService)
        {
            _reportService = reportService;
            _imageService = imageService;
        }

        [HttpGet("list")]
        public ActionResult<List<ReportList>> Get()
        {
            return _reportService.Get().Select(rep => new ReportList
            {
                Id = rep.Id,
                Title = rep.Title,
                Reporter = rep.Reporter,
                Accepted = rep.Accepted,
            }).ToList();
        }

        [HttpGet("{id:length(24)}", Name = "GetReport")]
        public ActionResult<Report> Get(string id)
        {
            Report rep = _reportService.Get(id);

            if (rep == null)
            {
                return NotFound();
            }

            return rep;
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult<Report> Post([FromForm]ReportFormData rep)
        {
            Report newRep;
            if (rep.Img != null && (rep.Img.ContentType == "image/jpeg" || rep.Img.ContentType == "image/png"))
            {
                newRep = new Report
                {
                    Reporter = rep.Reporter,
                    ReporterContact = rep.ReporterContact,
                    Title = rep.Title,
                    Body = rep.Body,
                    ImgPath = _imageService.SaveImg(rep.Img)
                };
            }
            else
            {
                newRep = new Report
                {
                    Reporter = rep.Reporter,
                    ReporterContact = rep.ReporterContact,
                    Title = rep.Title,
                    Body = rep.Body,
                };
            }

            _reportService.Create(newRep);

            return CreatedAtRoute("GetDonate", new { id = newRep.Id.ToString() }, newRep);
        }

        [HttpPatch("{id:length(24)}")]
        public IActionResult Accept(string id)
        {
            // fetch
            Report rep = _reportService.Get(id);

            // not found (bc wrong dept or no rep record)
            if (rep == null)
            {
                return NotFound();
            }

            // no reaccept
            if (rep.Accepted == false && GetClaim(User, ClaimEnum.UserType) == "A")
            {
                rep.Accepted = true;
                rep.AcceptedOn = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                rep.AcceptedBy = GetClaim(User, ClaimEnum.FirstName);
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
            Report rep = _reportService.Get(id);


            if (rep == null)
            {
                return NotFound();
            }

            if (!rep.Accepted)
            {
                return Forbid();
            }

            if (GetClaim(User, ClaimEnum.UserType) != "A")
            {
                return Unauthorized();
            }

            _reportService.Remove(rep);

            return NoContent();
        }
    }
}