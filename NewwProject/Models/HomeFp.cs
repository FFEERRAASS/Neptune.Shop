using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace NewwProject.Models
{
    public partial class HomeFp
    {
        public decimal HomeId { get; set; }
        public string Homeparagraph1 { get; set; }
        public string Homeparagraph2 { get; set; }
        public string Homeparagraph3 { get; set; }
        public string Location1 { get; set; }
        public string Location2 { get; set; }
        public string Email { get; set; }
        public string Photo1 { get; set; }
        public string Photo2 { get; set; }
        public string Photo3 { get; set; }
        public string Photo4 { get; set; }
        public string Photo5 { get; set; }
        public string Photo6 { get; set; }
        public string Photo7 { get; set; }
        public string Footerparagraph2 { get; set; }
        public string Footerparagraph3 { get; set; }
        [NotMapped]
        public IFormFile ImageFile { get; set; }
    }
}
