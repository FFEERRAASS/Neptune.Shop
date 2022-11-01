using System;
using System.Collections.Generic;

#nullable disable

namespace NewwProject.Models
{
    public partial class VisaFp
    {
        public decimal VisaId { get; set; }
        public string FullName { get; set; }
        public string Iban { get; set; }
        public string Cvv { get; set; }
        public decimal? Balance { get; set; }
        public string ExpiredDate { get; set; }
    }
}
