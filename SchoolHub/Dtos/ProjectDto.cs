using SchoolHub.Models;

namespace SchoolHub.Dtos
{
    public class ProjectDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public int AuthorId { get; set; }
        public string Category { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public string Status { get; set; } = "Идея";
        public string? Author { get; set; }
    }
}
