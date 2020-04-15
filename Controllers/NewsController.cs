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
    public class NewsController : ControllerBase
    {
        private readonly NewsService _newsService;
        private readonly UserService _userService;

        public NewsController(NewsService newsService, UserService userService)
        {
            _newsService = newsService;
            _userService = userService;
        }

        [HttpGet("list")]
        public ActionResult<List<NewsList>> Get()
        {
            User user = _userService.Find(User.Identity.Name);

            return _newsService.Get(user.DeptNo).Select(news => new NewsList
            {
                Id = news.Id,
                Title = news.Title,
                Writer = news.Writer,
                Accepted = news.Accepted
            }).ToList();
        }

        [HttpGet("{id:length(24)}", Name = "GetNews")]
        public ActionResult<News> Get(string id)
        {
            User user = _userService.Find(User.Identity.Name);
            News news = _newsService.Get(id, user.DeptNo);

            if (news == null)
            {
                return NotFound();
            }

            return news;
        }

        [HttpPost]
        public ActionResult<News> Create(News news)
        {
            //fetch
            User user = _userService.Find(User.Identity.Name);

            // prevent change
            news.Accepted = false;
            news.Writer = user.FirstName;
            news.DeptNo = user.DeptNo;
            // create
            _newsService.Create(news);

            return CreatedAtRoute("GetNews", new { id = news.Id.ToString() }, news);
        }

        [AllowAnonymous]
        [HttpGet("visitor")]
        public ActionResult<List<NewsVisitor>> GetForVisitor()
        {
            return _newsService.GetAcceptedNews().Select(news => (NewsVisitor)news).ToList();
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, News newsIn)
        {
            // fetch
            User user = _userService.Find(User.Identity.Name);
            News news = _newsService.Get(id, user.DeptNo);

            // not found (bc wrong dept or no donate record)
            if (news == null)
            {
                return NotFound();
            }

            // prevent change
            news.Accepted = false;
            newsIn.Writer = news.Writer;
            newsIn.DeptNo = news.DeptNo;
            _newsService.Update(id, newsIn);

            return NoContent();
        }

        [HttpPatch("{id:length(24)}")]
        public IActionResult Accept(string id)
        {
            // fetch
            User user = _userService.Find(User.Identity.Name);
            News news = _newsService.Get(id, user.DeptNo);

            // not found (bc wrong dept or no donate record)
            if (news == null)
            {
                return NotFound();
            }

            // no reaccept
            if (news.Accepted == false)
            {
                news.Accepted = true;
                news.AcceptedDate = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                _newsService.Update(id, news);
            }
            

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            User user = _userService.Find(User.Identity.Name);
            News news = _newsService.Get(id, user.DeptNo);

            if (news == null)
            {
                return NotFound();
            }

            _newsService.Remove(news);

            return NoContent();
        }
    }
}