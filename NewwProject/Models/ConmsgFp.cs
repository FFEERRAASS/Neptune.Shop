using System;
using System.Collections.Generic;

#nullable disable

namespace NewwProject.Models
{
    public partial class ConmsgFp
    {
        public decimal ContId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Subject { get; set; }
        public string Msg { get; set; }
    }
}
