using System;
using System.Collections.Generic;

#nullable disable

namespace NewwProject.Models
{
    public partial class FavouriteFp
    {
        public decimal FavId { get; set; }
        public decimal? Handfk { get; set; }
        public decimal? Userfk { get; set; }

        public virtual HandcraftFp HandfkNavigation { get; set; }
        public virtual UserFp UserfkNavigation { get; set; }
    }
}
