using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp1.Services
{
    public class CartState
    {
        public List<Item> Items { get; set; } = new();

        public double? GetTotal()
        {
            return Items.Sum(item => item.Price * item.Quantity);
        }

        public class Item
        {
            public int? ID { get; set; }
            public string Name { get; set; }
            public double? Price { get; set; }
            public string InternalImage { get; set; }
            public int? Quantity { get; set; }
        }
    }

}
