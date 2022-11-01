using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace NewwProject.Models
{
    public partial class HandcraftFp
    {
        public HandcraftFp()
        {
            FavouriteFps = new HashSet<FavouriteFp>();
            OrdersFps = new HashSet<OrdersFp>();
            OrdersdoneFps = new HashSet<OrdersdoneFp>();
        }

        public decimal HandId { get; set; }
        public string Handcraft { get; set; }
        public decimal Price { get; set; }
        public decimal Sale { get; set; }
        public string Descriptions { get; set; }
        public string ImagePath { get; set; }
        public decimal? UserFk { get; set; }
        public decimal? CategoryId { get; set; }
        [NotMapped]
        public IFormFile ImageFile { get; set; }
        public virtual CategoryFp Category { get; set; }
        public virtual UserFp UserFkNavigation { get; set; }
        public virtual ICollection<FavouriteFp> FavouriteFps { get; set; }
        public virtual ICollection<OrdersFp> OrdersFps { get; set; }
        public virtual ICollection<OrdersdoneFp> OrdersdoneFps { get; set; }
    }
}
