using System.ComponentModel.DataAnnotations;

namespace BlogApp.Models;

public class Blog
{
    public int Id { get; set; }
    [Required]
    [MaxLength(100)]
    public string Title { get; set; }
    [Required]
    public string Body { get; set; }
}
