using System.ComponentModel.DataAnnotations;

namespace SchoolHub.Models
{
    public class CreateProjectDto
    {
        [Required(ErrorMessage = "Enter title")]
        public string Title { get; set; } = string.Empty;
        [Required(ErrorMessage = "Enter description")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Enter category")]
        public string Category { get; set; } = string.Empty;
        [Required(ErrorMessage = "Enter idea")]
        public string Status { get; set; } = "Идея";

    }
}
