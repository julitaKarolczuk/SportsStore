using SportsStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportsStore.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<Category> Categories {get; set;}

        public IEnumerable<Product> Products { get; set; }

    }
}