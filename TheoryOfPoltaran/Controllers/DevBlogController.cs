using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheoryOfPoltaran.Models;
using TheoryOfPoltaran.ViewModels;

namespace TheoryOfPoltaran.Controllers
{
    public class DevBlogController : Controller
    {
        private const int PageSize = 10;
        public int TotalItems => _context.Publications.Count();
        public int TotalPages => (int)Math.Ceiling((decimal)TotalItems / PageSize);
        private MainContext _context;
        public DevBlogController(MainContext mainContext)
        {
            _context = mainContext;
        }
        public async Task<IActionResult> Index(int pg = 1)
        {
            int tp = TotalPages;
            if (pg < 1 || pg > tp)
                pg = 1;
            var posts = (await _context.Publications.OrderByDescending(x => x.Date)
                                                    .Skip((pg - 1) * PageSize)
                                                    .Take(PageSize)
                                                    .ToListAsync());
            DevBlogDataViewModel data = new DevBlogDataViewModel
            {
                CurrentPage = pg,
                IsAdmin = User.Identity.IsAuthenticated,
                Publications = posts,
                TotalPages = tp
            };
            return View(data);
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Publications.FirstOrDefaultAsync(m => m.Id == id);
            if (post == null)
            {
                return NotFound();
            }
            return View(post);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return NotFound();
            }
            if (id == null)
            {
                return BadRequest();
            }

            var post = await _context.Publications.FirstOrDefaultAsync(m => m.Id == id);
            if (post == null)
            {
                return BadRequest();
            }
            _context.Publications.Remove(post);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
