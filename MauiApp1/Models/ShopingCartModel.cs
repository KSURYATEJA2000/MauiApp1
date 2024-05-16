using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp1.Models
{
    internal class ShopingCartModel
    {
        public List<Carts> CartItems { get; set; }
    }

    public partial class Carts
    {
        public int Id { get; set; }
        public string CartId { get; set; }
        public int ItemId { get; set; }
        public int Count { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
