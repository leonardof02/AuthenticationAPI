namespace MiddleApi.Models;

public class Topic
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public IEnumerable<Article> Articles { get; set; } = [];
}