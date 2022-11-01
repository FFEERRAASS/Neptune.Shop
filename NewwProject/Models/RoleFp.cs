using System;
using System.Collections.Generic;

#nullable disable

namespace NewwProject.Models
{
    public partial class RoleFp
    {
        public RoleFp()
        {
            UserFps = new HashSet<UserFp>();
            UserloginFps = new HashSet<UserloginFp>();
        }

        public decimal RoleId { get; set; }
        public string RoleName { get; set; }

        public virtual ICollection<UserFp> UserFps { get; set; }
        public virtual ICollection<UserloginFp> UserloginFps { get; set; }
    }
}
