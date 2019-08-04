using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace datumation_products.Shared.Models
{
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