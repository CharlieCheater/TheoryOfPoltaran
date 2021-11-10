using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheoryOfPoltaran.Models;

namespace TheoryOfPoltaran.ViewModels
{
    public class DevBlogDataViewModel
    {
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
        public bool IsAdmin { get; set; }
        public List<Publication> Publications { get; set; }
    }
}
