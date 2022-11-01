using System;
using System.Collections.Generic;

#nullable disable

namespace NewwProject.Models
{
    public partial class OrdersFp
    {
        public decimal OrderId { get; set; }
        public decimal? HandcraftFk { get; set; }
        public decimal? UserFk { get; set; }
        public decimal? Quantity { get; set; }

        public virtual HandcraftFp HandcraftFkNavigation { get; set; }
        public virtual UserFp UserFkNavigation { get; set; }
    }
}
