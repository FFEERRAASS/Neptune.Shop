using System;
using System.Collections.Generic;

#nullable disable

namespace NewwProject.Models
{
    public partial class ReviewFp
    {
        public decimal ReviewId { get; set; }
        public string Review { get; set; }
        public string Rate { get; set; }
        public decimal? UserFk { get; set; }
        public decimal? Checks { get; set; }

        public virtual UserFp UserFkNavigation { get; set; }
    }
}
