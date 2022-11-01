using System;
using System.Collections.Generic;

#nullable disable

namespace NewwProject.Models
{
    public partial class UserloginFp
    {
        public decimal UloginId { get; set; }
        public string Username { get; set; }
        public string UserPassword { get; set; }
        public decimal? UserFk { get; set; }
        public decimal? RoleFk { get; set; }
        public decimal? IsAccepted { get; set; }

        public virtual RoleFp RoleFkNavigation { get; set; }
        public virtual UserFp UserFkNavigation { get; set; }
    }
}
