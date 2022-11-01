using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace NewwProject.Models
{
    public partial class CategoryFp
    {
        public CategoryFp()
        {
            HandcraftFps = new HashSet<HandcraftFp>();
        }

        public decimal CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string CategoryImage { get; set; }
        [NotMapped]
        public IFormFile ImageFile { get; set; }
        public virtual ICollection<HandcraftFp> HandcraftFps { get; set; }
    }
}
