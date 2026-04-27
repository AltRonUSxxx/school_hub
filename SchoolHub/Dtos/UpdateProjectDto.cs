using System.ComponentModel.DataAnnotations;

namespace SchoolHub.Dtos
{
    public class UpdateProjectDto
    {
        [Required(ErrorMessage = "Enter title")]
        public string Title { get; set; } = string.Empty;
        [Required(ErrorMessage = "Enter description")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Enter category")]
        public string Category { get; set; } = string.Empty;
        [Required(ErrorMessage = "Enter Status")]
        public string Status { get; set; } = string.Empty;
    }
}
