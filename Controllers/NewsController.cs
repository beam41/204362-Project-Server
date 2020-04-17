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
    public class NewsController : ControllerBase
    {
        private readonly NewsService _newsService;

        public NewsController(NewsService newsService)
        {
            _newsService = newsService;
        }

        [HttpGet("list")]
        public ActionResult<List<NewsList>> Get()
        {
            if (GetClaim(User, ClaimEnum.UserType) == "A")
            {
                return _newsService.Get().Select(news => new NewsList
                {
                    Id = news.Id,
                    Title = news.Title,
                    Writer = news.Writer,
                    Accepted = news.Accepted
                }).ToList();
            }
            return _newsService.Get(int.Parse(GetClaim(User, ClaimEnum.DeptNo))).Select(news => new NewsList
            {
                Id = news.Id,
                Title = news.Title,
                Writer = news.Writer,
                Accepted = news.Accepted
            }).ToList();
        }

        [AllowAnonymous]
        [HttpGet("visitor")]
        public ActionResult<List<NewsVisitor>> GetForVisitor()
        {
            return _newsService.GetAcceptedNews().Select(news => (NewsVisitor)news).ToList();
        }

        [AllowAnonymous]
        [HttpGet("visitor/{len}")]
        public ActionResult<List<NewsVisitor>> GetForVisitor(int len)
        {
            return _newsService
                .GetAcceptedNews()
                .OrderByDescending(news => news.AcceptedOn)
                .Take(len).Select(news => (NewsVisitor)news)
                .ToList();
        }

        [AllowAnonymous]
        [HttpGet("visitor/{id:length(24)}")]
        public ActionResult<News> GetForVisitor(string id)
        {
            News news;
            news = _newsService.Get(id);
            if (news == null && !news.Accepted)
            {
                return NotFound();
            }

            return news;
        }

        [HttpGet("{id:length(24)}", Name = "GetNews")]
        public ActionResult<News> Get(string id)
        {
            News news;
            if (GetClaim(User, ClaimEnum.UserType) == "A")
            {
                news = _newsService.Get(id);
            }
            else
            {
                news = _newsService.Get(id, int.Parse(GetClaim(User, ClaimEnum.DeptNo)));
            }


            if (news == null)
            {
                return NotFound();
            }

            return news;
        }

        [HttpPost]
        public ActionResult<News> Create(News news)
        {
            // prevent change
            news.Accepted = false;
            news.WroteOn = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            news.Writer = GetClaim(User, ClaimEnum.FirstName);
            news.DeptNo = int.Parse(GetClaim(User, ClaimEnum.DeptNo));
            // create
            _newsService.Create(news);

            return CreatedAtRoute("GetNews", new { id = news.Id.ToString() }, news);
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, News newsIn)
        {
            // fetch
            News news;
            if (GetClaim(User, ClaimEnum.UserType) == "A")
            {
                news = _newsService.Get(id);
            }
            else
            {
                news = _newsService.Get(id, int.Parse(GetClaim(User, ClaimEnum.DeptNo)));
            }

            // not found (bc wrong dept or no news record)
            if (news == null)
            {
                return NotFound();
            }

            // prevent change
            newsIn.Id = news.Id;
            newsIn.Accepted = false;
            newsIn.Writer = news.Writer;
            newsIn.DeptNo = news.DeptNo;
            _newsService.Update(id, newsIn);

            return NoContent();
        }

        [HttpPatch("{id:length(24)}")]
        public IActionResult Accept(string id)
        {
            // fetch
            News news;
            if (GetClaim(User, ClaimEnum.UserType) == "A")
            {
                news = _newsService.Get(id);
            }
            else
            {
                news = _newsService.Get(id, int.Parse(GetClaim(User, ClaimEnum.DeptNo)));
            }

            // not found (bc wrong dept or no news record)
            if (news == null)
            {
                return NotFound();
            }

            // no reaccept
            if (news.Accepted == false && GetClaim(User, ClaimEnum.UserType) == "A")
            {
                news.Accepted = true;
                news.AcceptedBy = GetClaim(User, ClaimEnum.FirstName);
                news.AcceptedOn = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                _newsService.Update(id, news);
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
            News news;
            if (GetClaim(User, ClaimEnum.UserType) == "A")
            {
                news = _newsService.Get(id);
            }
            else
            {
                news = _newsService.Get(id, int.Parse(GetClaim(User, ClaimEnum.DeptNo)));
            }

            if (news == null)
            {
                return NotFound();
            }

            _newsService.Remove(news);

            return NoContent();
        }
    }
}