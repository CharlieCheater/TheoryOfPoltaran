using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheoryOfPoltaran.Models;

namespace TheoryOfPoltaran.Controllers
{
    public class DevBlogController : Controller
    {
        private const int PageSize = 1;
        public int TotalItems => _context.Publications.Count();
        public int TotalPages => (int)Math.Ceiling((decimal)TotalItems / PageSize);
        private MainContext _context;
        public DevBlogController(MainContext mainContext)
        {
            _context = mainContext;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<object> GetPage(int page = 1)
        {
            if (page < 1 || page > TotalPages)
                return null;
            var data = (await _context.Publications.OrderByDescending(x => x.Date)
                                                    .Skip((page - 1) * PageSize)
                                                    .Take(PageSize)
                                                    .ToListAsync())
                                                    .Select(x => new { x.Title, x.Description, x.Text, x.Date, });
            return data;
        }
        public object GetFirstData()
        {
            return new { TotalPages, ShowBtn = User.Identity.IsAuthenticated };
        }
    }
}
