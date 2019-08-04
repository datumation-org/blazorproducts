using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace datumation_products.Shared.Models {
    public class FileUpload {
        [Required]
        [Display (Name = "Title")]
        [StringLength (60, MinimumLength = 3)]
        public string Title { get; set; }

        [Required]
        [Display (Name = "Public Schedule")]

        public IFormFile UploadPublicSchedule { get; set; }

    }
}