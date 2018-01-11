using SportsStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportsStore.ViewModels
{
    public class OrderViewModel
    {
        public IEnumerable<CartItem> Card { get; set; }
        public string UserAddress { get; set; }
    }
}