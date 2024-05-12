namespace MiddleApi.Models;

public class Article
{
    public Guid Id { get; set; }
    public required string Title { get; set; }
    public required string MdContent { get; set; }
    public string? ImageUrl { get; set; }
    public DateTime? PublishDate { get; set; }
    public bool IsDraft { get; set; }

    public Guid UserId { get; set; }
    public required User Author { get; set; }
    
    public IEnumerable<Topic> Topics { get; set; } = [];
}