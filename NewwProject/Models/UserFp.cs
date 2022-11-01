using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace NewwProject.Models
{
    public partial class UserFp
    {
        public UserFp()
        {
            Emails = new HashSet<Email>();
            FavouriteFps = new HashSet<FavouriteFp>();
            HandcraftFps = new HashSet<HandcraftFp>();
            OrdersFps = new HashSet<OrdersFp>();
            OrdersdoneFps = new HashSet<OrdersdoneFp>();
            ReviewFps = new HashSet<ReviewFp>();
            UserloginFps = new HashSet<UserloginFp>();
        }

        public decimal UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string ImagePath { get; set; }
        public decimal? Gender { get; set; }
        public string CraftName { get; set; }
        public decimal? RoleFk { get; set; }
        [NotMapped]
        public IFormFile ImageFile { get; set; }
        public virtual RoleFp RoleFkNavigation { get; set; }
        public virtual ICollection<Email> Emails { get; set; }
        public virtual ICollection<FavouriteFp> FavouriteFps { get; set; }
        public virtual ICollection<HandcraftFp> HandcraftFps { get; set; }
        public virtual ICollection<OrdersFp> OrdersFps { get; set; }
        public virtual ICollection<OrdersdoneFp> OrdersdoneFps { get; set; }
        public virtual ICollection<ReviewFp> ReviewFps { get; set; }
        public virtual ICollection<UserloginFp> UserloginFps { get; set; }
    }
}
