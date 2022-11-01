using System;
using System.Collections.Generic;

#nullable disable

namespace NewwProject.Models
{
    public partial class Email
    {
        public decimal EmailId { get; set; }
        public string EmailSubject { get; set; }
        public string EmailMsg { get; set; }
        public decimal? UserFk { get; set; }
        public decimal? SenderReciver { get; set; }

        public virtual UserFp UserFkNavigation { get; set; }
    }
}
