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
        private const int PageSize = 5;
        public int TotalItems => _mainContext.Publications.Count();
        public int TotalPages => (int)Math.Ceiling((decimal)TotalItems / PageSize);
        private MainContext _mainContext;
        public DevBlogController(MainContext mainContext)
        {
            _mainContext = mainContext;
        }
        public IActionResult Index(int page = 1)
        {
            return View();
        }
        public int GetTotalPages()
        {
            return TotalPages;
        }
    }
}
