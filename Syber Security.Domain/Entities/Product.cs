using SyberSecurity.Domain.Utilities;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace SyberSecurity.Domain.Entities
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(55)]
        public string? Title { get; set; }
        public string? Description { get; set; }

       
        [Required]
        public double Price { get; set; }

        [MaxFileSize(5 * 1024 * 1024, ErrorMessage = "Maximum allowed file size is 5 MB")]
        [AllowedExtensions(new string[] { ".png", ".jpg", ".jpeg", ".svg" })]
        [DataType(DataType.Upload)]
        [Display(Name = "Image")]
        public string? ImageUrl { get; set; }

        [Required]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }
        [ValidateNever]
        public Category? Category { get; set; }

        [Range(0, 500)]
        public int InStock { get; set; }

    }
}
